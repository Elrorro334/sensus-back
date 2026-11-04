# Arquitectura — Sensus Backend

Resumen navegable de la arquitectura. La fuente autoritativa es
[`arquitecturaNva.docx`](arquitecturaNva.docx); este documento la interpreta y la conecta con las specs.
Ante conflicto, prevalece la fuente original y después `constitution.md`.

| Campo | Valor |
|---|---|
| Documento | Resumen de arquitectura (no normativo) |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |
| Fuente autoritativa | `arquitecturaNva.docx` |
| Documento derogado | `Arquitectura Back.docx` (incluida su referencia a *Load Balancer*) |
| Fecha | 2026-10-29 |

---

## 1. Visión general

Sensus es una plataforma de ofertas locales geolocalizadas con una capa social. El backend es una **API
RESTful autenticada** que permite activar comercios, publicar ofertas, apartarlas con un código QR,
descubrir contenido cercano, recomendar alternativas y moderar **antes** de exponer cualquier contenido.
No incluye interfaz de usuario, visión artificial local, pasarelas de pago ni bases de datos relacionales.

```
Cliente móvil / web
        │  HTTPS + JWT
        ▼
  Amazon API Gateway
        │
        ▼
  AWS Lambda (Sensus.Api)
        │
        ├── Sensus.Application ──► Sensus.Domain
        │                              │
        ├── Sensus.Infrastructure ─────┘
        │        ├── MongoDB 8 (documentos)
        │        ├── Amazon S3 (multimedia privada)
        │        ├── OpenAI omni-moderation-latest
        │        ├── Amazon SES (recuperación de contraseña)
        │        ├── IPushDispatcher → FCM | SNS
        │        └── Motor de ranking determinista (Application)
        │
        ├── Amazon CloudFront ──► S3 (contenido aprobado)
        └── Amazon CloudWatch (logs, métricas, alarmas)
```

---

## 2. Capas y responsabilidades

| Capa | Responsabilidad | Depende de | Prohibido |
|---|---|---|---|
| `Sensus.Domain` | Entidades, invariantes, contratos (interfaces) | Nada | Frameworks, ORMs, SDKs de nube |
| `Sensus.Application` | Casos de uso CQRS, DTOs, validación de borde, mapeos | Domain | Infrastructure, acceso directo a MongoDB |
| `Sensus.Infrastructure` | MongoDB, BCrypt, JWT, S3, SES, moderación, push, geolocalización, caché | Application | Reglas de negocio |
| `Sensus.Api` | Endpoints, middleware, autenticación HTTP, composición | Application (+ Infrastructure solo en `Program.cs`) | Reglas de negocio, acceso a persistencia |

Dirección de dependencias: `Application → Domain`, `Infrastructure → Application`, `Api → Application`.
Las referencias inversas no existen y se verifican con NetArchTest en cada build.

Detalle en [ADR 0001](../adr/0001-clean-architecture.md).

---

## 3. Flujo de una petición

1. **API Gateway** termina TLS, aplica *throttling* y reenvía a Lambda.
2. **Middleware de correlación** toma o genera `correlation-id` y lo propaga (spec 014).
3. **Autenticación JWT**: firma, emisor, audiencia, expiración y `jti` no revocado (spec 004).
4. **Autorización** por rol y por recurso, siempre en el servidor.
5. **Validación de borde**: formato, presencia y rangos (FluentValidation).
6. **Handler CQRS**: orquesta el caso de uso; delega las reglas al dominio.
7. **Dominio**: aplica invariantes; devuelve errores de negocio tipados.
8. **Infraestructura**: persiste o consulta con índice declarado y `CancellationToken`.
9. **Respuesta**: DTO serializado y comprimido; errores en `ProblemDetails` con `correlationId`.
10. **Observabilidad**: log estructurado sin PII y métricas de latencia y resultado.

---

## 4. Modelo de datos

MongoDB 8.x, una base por entorno. Colecciones previstas y sus índices obligatorios:

| Colección | Contenido | Índices | Spec |
|---|---|---|---|
| `users` | Cuenta unificada con roles y estado | Único en `email` | 001–003 |
| `password_reset_tokens` | Token de recuperación hasheado | TTL en `expiresAt` | 029 |
| `token_revocations` | `jti` revocados | Único en `jti`; TTL en `expiresAt` | 004 |
| `merchants` | Comercio con ubicación | Único en `userId`; `2dsphere` en `location` | 005, 006, 031 |
| `offers` | Oferta con precio, stock y vigencia | `merchantId` + `status`; `status` + `endsAt`; `2dsphere` en `location` | 007, 032–037 |
| `offer_claims` | Apartado con código QR y estado | Único `userId` + `offerId` + `status`; TTL en `expiresAt` | 033 |
| `offer_views` | Vista deduplicada por día | Único `userId` + `offerId` + `dayKey` | 032 |
| `favorites` | Favorito por usuario y oferta | Único `userId` + `offerId` | 038 |
| `post_likes` | Me gusta por usuario y publicación | Único `postId` + `userId` | 039 |
| `moderation_decisions` | Veredicto, categorías, puntajes, umbral, acción | Por `contentId` y fecha | 017–019 |
| `moderation_audit` | Evidencia inmutable (*append-only*) | Por fecha y usuario | 021 |
| `sanctions` | Sanciones con vigencia | Por `userId` y vigencia | 020 |
| `media_assets` | Archivo, propietario, estado, clave S3 | Por propietario y estado | 022, 023 |
| `posts` | Publicación con estado y ubicación | `status` + `createdAt`; `authorId` + `createdAt` | 024, 026 |
| `comments` | Comentarios por publicación | Compuesto `postId` + `createdAt` | 025 |
| `reports` | Reportes de contenido | Único `reporterId` + `targetId` + `dayKey` | 040 |
| `devices` | Tokens push | Único por `token`; por `userId` + `deviceId` | 027 |
| `notification_settings` | Preferencias de notificación | Único en `userId` | 044 |
| `webhook_events` | Eventos entrantes (backlog) | Único en `eventId`; estado | 028 |

Reglas: ningún endpoint puede consultar sin índice declarado; toda colección expuesta se pagina
(20 por defecto, 100 máximo) con metadatos; los datos de producción no se usan en desarrollo.

---

## 5. Flujo de moderación

```
Usuario ──► POST /offers | /feed/posts | /comments ──► validación de borde
                                     │
                                     ▼
                       moderación (imagen + texto, misma solicitud)
                                     │
              ┌──────────────────────┼──────────────────────┐
              ▼                      ▼                      ▼
        banda baja             banda intermedia         banda alta
     publicar automático       revisión manual        rechazo + sanción
                                     │                      │
                                     └──────────┬───────────┘
                                                ▼
                                      registro de auditoría
```

- Ante error, timeout o respuesta inválida del proveedor: **cuarentena**, nunca publicación (fail-closed).
- Los umbrales viven en configuración por entorno y se registran con cada decisión.
- Las sanciones son auditables y reversibles.

Detalle en [ADR 0004](../adr/0004-moderacion-multimodal.md).

---

## 6. Seguridad

| Control | Implementación | Spec |
|---|---|---|
| Autenticación | JWT sin estado, validado en firma, emisor, audiencia y expiración | 002 |
| Cierre de sesión | Denylist por `jti` con TTL | 004 |
| Transporte | TLS 1.2+, HSTS, redirección fuera de desarrollo | 012 |
| CORS | Orígenes explícitos por entorno; prohibido comodín | 012 |
| Abuso | Rate limiting por cliente y por endpoint sensible | 013 |
| Secretos | AWS Secrets Manager o SSM; *user-secrets* en local | 001, 002 |
| Privacidad | Logs sin PII, sin tokens y sin contenido moderado | 014 |
| Multimedia | Bucket privado; carga multipart ≤ 4,5 MB validada por firma de bytes | 022, 023 |
| Push | `IPushDispatcher` con adaptador FCM o SNS según configuración | 027, 044 |
| Webhooks | Firma HMAC e idempotencia por `eventId` (backlog) | 028 |

Deuda abierta en [`SECURITY.md`](../../SECURITY.md).

---

## 7. Despliegue y costo

| Componente | Servicio | Límite del Free Tier |
|---|---|---|
| Cómputo | AWS Lambda | 1 M de invocaciones y 400 000 GB-s al mes |
| API | AWS API Gateway | 1 M de llamadas al mes |
| Multimedia | Amazon S3 | 5 GB de almacenamiento |
| CDN | Amazon CloudFront | 1 TB de salida al mes |
| Observabilidad | Amazon CloudWatch | 10 alarmas y 5 GB de logs |
| Moderación | OpenAI | Sin costo por token; validar política de uso |
| Push | FCM o SNS (adaptador) | FCM sin costo; SNS dentro del Free Tier |

Todo recurso se define con AWS CDK en `infra/`, se etiqueta con `project`, `environment` y `owner`, y se
valida con `cdk synth` y `cdk diff`. El despliegue solo ocurre desde el pipeline. Ver
[ADR 0005](../adr/0005-serverless-aws-costo-cero.md).

---

## 8. Observabilidad

- Logs JSON con `timestamp`, `level`, `correlationId`, `message`, `duration` y resultado.
- Métricas de latencia (p50/p95), tasa de error, *throttling*, invocaciones y saturación.
- Alarmas definidas como código con umbral y destinatario.
- CloudWatch es la única plataforma de observabilidad; no se integran proveedores externos.
- `/health/live` y `/health/ready` no revelan detalles internos.

---

## 9. Mapa de specs por ola

| Ola | Foco | Specs |
|---|---|---|
| 0 | Enmienda constitucional y plataforma de contratos | 009–011 |
| 1 | Identidad y cuenta | 001–003, 029, 030 |
| 2 | Comercio, ofertas y apartado con QR | 005–007, 031–037 |
| 3 | Red social | 024–026, 039, 040 |
| 4 | Media, moderación y push | 017–023, 027, 044 |
| 5 | Favoritos y ranking | 038, 041–043 |
| 6 | Plataforma, seguridad y retiro legacy | 004, 008, 012–016 |
| Backlog | Webhooks entrantes | 028 |

Índice completo en [`specs/README.md`](../../specs/README.md).

---

## 10. Documentos relacionados

| Documento | Contenido |
|---|---|
| [`../../constitution.md`](../../constitution.md) | Principios normativos y gobernanza |
| [`../../AGENTS.md`](../../AGENTS.md) | Reglas operativas |
| [`../../memory.md`](../../memory.md) | Estado vivo y próximos pasos |
| [`../adr/README.md`](../adr/README.md) | Índice de decisiones |
| [`../../specs/requirements.md`](../../specs/requirements.md) | Registro de requisitos |
