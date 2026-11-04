# AGENTS.md — Sensus Backend

| Campo | Valor |
|---|---|
| Proyecto | Sensus — Backend API RESTful |
| Documento normativo | `constitution.md` **v1.2.0** |
| Fuente de arquitectura | `docs/architecture/arquitecturaNva.docx` |
| Alcance | Todo el repositorio |
| Idioma | Documentación en español; código e identificadores en inglés |
| Última actualización | 2026-10-29 |

> Este archivo es **operativo**. En caso de conflicto con `constitution.md`, prevalece la constitución y
> este archivo MUST corregirse en el mismo cambio.

---

## 1. Contexto

Sensus es una plataforma de ofertas locales geolocalizadas con una capa social. Este repositorio
contiene **únicamente el backend**: una API RESTful autenticada, con activación de comerciantes,
inventario de ofertas, apartado con código QR, feed social, recomendaciones, moderación de contenido
previa a la publicación, almacenamiento multimedia y notificaciones push.

**Fuera del repositorio:** aplicación móvil, página web, *landing*, visión artificial local, pasarelas de
pago, bases de datos relacionales y cualquier nube distinta de AWS.

---

## 2. Documentos y precedencia

| Documento | Rol | Cuándo consultarlo |
|---|---|---|
| `constitution.md` | Normativo, máxima autoridad | Siempre, antes de cualquier cambio |
| `docs/architecture/arquitecturaNva.docx` | Contexto de arquitectura (fuente autoritativa) | Dudas de diseño o alcance |
| `docs/architecture/overview.md` | Resumen navegable de la arquitectura | Onboarding y dudas rápidas |
| `specs/NNN-slug/spec.md` | Alcance, requisitos y criterios de aceptación | Antes de implementar una funcionalidad |
| `specs/NNN-slug/plan.md` | Diseño técnico y Constitution Check | Antes de escribir código |
| `specs/NNN-slug/tasks.md` | Tareas atómicas trazables | Para ejecutar el trabajo |
| `specs/NNN-slug/checklist.md` | Definition of Ready/Done y evidencia | Al abrir y cerrar cada tarea |
| `specs/requirements.md` | Registro consolidado de requisitos | Para localizar un `FR`/`NFR` |
| `specs/README.md` | Índice de las specs (43 activas y 1 en backlog) | Para ubicar una capacidad |
| `docs/adr/` | Decisiones de arquitectura | Antes de proponer un cambio estructural |
| `memory.md` | Estado vivo del proyecto y próximos pasos | Al inicio y al cierre de cada sesión |
| `AGENTS.md` | Reglas operativas (este archivo) | En cada sesión de trabajo |

Orden de precedencia: `constitución > spec > plan > tasks > checklist > código > este archivo`.

---

## 3. Flujo obligatorio

1. Si no existe `specs/NNN-slug/spec.md`, **no escribas código**: crea la spec o pide confirmación.
2. Una spec válida exige **cuatro artefactos**: `spec.md`, `plan.md`, `tasks.md` y `checklist.md`.
   Si falta uno, la spec está incompleta y no se implementa.
3. Toda tarea MUST referenciar un requisito con formato `FR-<spec>-<nn>` o `NFR-<spec>-<nn>`
   (ejemplo `FR-017-03`). El prefijo MUST coincidir con el ID de la carpeta de la spec.
4. Si un criterio de aceptación no es medible, **pregunta** antes de implementar. No inventes alcance.
5. Si el cambio exige violar la constitución, **detente** y propón una enmienda (`constitution.md` §4.3).
   Única excepción: *hotfix* de seguridad, con spec retroactiva en un máximo de 48 horas.
6. Implementa en el orden `specify → clarify → plan → tasks → checklist → implement`. No adelantes fases.
7. Al cerrar la sesión, actualiza `memory.md` con lo hecho, lo que quedó abierto y el siguiente paso.

---

## 4. Estructura de una spec

```
specs/
  NNN-slug/
    spec.md        Contexto, alcance, FR/NFR, criterios de aceptación, contratos
    plan.md        Diseño por capa, datos, seguridad, pruebas, Constitution Check
    tasks.md       Tareas atómicas con capa, requisito y dependencia
    checklist.md   DoR, criterios específicos, DoD y registro de evidencia
    contracts/
      openapi.yaml Contrato del spec (solo si expone endpoints)
  _templates/      Plantillas de los cuatro artefactos
  README.md        Índice de specs
  requirements.md  Registro de todos los requisitos
```

Reglas:

- Una spec = una capacidad, implementable en un máximo aproximado de una semana.
- El ID (`NNN`) es estable: no se reutiliza ni se renumera. La carpeta es `NNN-slug`.
- Los cuatro artefactos citan el mismo ID y la misma versión de `constitution.md`.
- El estado real del trabajo vive en `tasks.md` y `checklist.md`; no en documentos paralelos.
- Una spec nueva toma el siguiente número libre. No se abren specs sin necesidad identificada.

---

## 5. Mapa del repositorio (estructura objetivo)

```
sensus-back.sln
src/
  Sensus.Domain/           Entidades, invariantes, contratos. Sin dependencias externas.
  Sensus.Application/      Casos de uso CQRS, DTOs, mapeos, validación de borde.
  Sensus.Infrastructure/   MongoDB, S3, moderación, correo, push, caché, geolocalización.
  Sensus.Api/              Endpoints, middleware, composición (Program.cs).
tests/
  Sensus.Application.UnitTests/
  Sensus.IntegrationTests/       (a crear — ver spec 001 y siguientes)
  Sensus.ArchitectureTests/
infra/                     AWS CDK en C#. (a crear)
specs/                     Una carpeta por capacidad.
docs/adr/                  Decisiones de arquitectura.
docs/architecture/         Fuente autoritativa y resumen.
.github/                   CI, plantillas de PR e issues.
```

Si una carpeta aún no existe, créala solo cuando la tarea lo requiera.

---

## 6. Comandos

> Ejecutar desde la raíz del repositorio. `cdk deploy` NUNCA se ejecuta sin autorización explícita.

| Acción | Comando |
|---|---|
| Restaurar | `dotnet restore sensus-back.sln` |
| Compilar | `dotnet build sensus-back.sln -warnaserror` |
| Pruebas | `dotnet test sensus-back.sln --collect:"XPlat Code Coverage"` |
| Pruebas unitarias | `dotnet test tests/Sensus.Application.UnitTests` |
| Arquitectura | `dotnet test tests/Sensus.ArchitectureTests` |
| Formato | `dotnet format sensus-back.sln --verify-no-changes` |
| IaC sintaxis | `cd infra; cdk synth` |
| IaC diferencias | `cd infra; cdk diff` |
| MongoDB local | `docker compose up -d mongo` |
| Servicios locales | `docker compose up -d` |

`Directory.Build.props` centraliza `TargetFramework`, `Nullable`, `TreatWarningsAsErrors` y el
tratamiento de warnings; `global.json` fija el SDK. No los contradigas por proyecto.

---

## 7. Reglas de arquitectura

### 7.1 Dónde va cada cosa

| Si trabajas en… | Escribe en… | Nunca en… |
|---|---|---|
| Reglas de negocio, invariantes, entidades | `Sensus.Domain` | Endpoints, handlers, repositorios |
| Casos de uso, comandos, consultas, DTOs | `Sensus.Application` | Endpoints |
| MongoDB, S3, OpenAI, correo, push, caché | `Sensus.Infrastructure` | Domain, Application |
| Endpoints, middleware, autenticación HTTP | `Sensus.Api` | — |

### 7.2 Dependencias

- Dirección permitida: `Application → Domain`, `Infrastructure → Application`, `Api → Application`.
- **Prohibido** referenciar `Infrastructure` desde `Domain` o `Application`.
- Toda dependencia externa se declara como interfaz en Domain o Application.
- La inyección de dependencias se compone **solo** en `Sensus.Api/Program.cs`.

### 7.3 CQRS

- Un handler por comando o consulta. No mezcles lectura y escritura en el mismo handler.
- Los handlers no contienen reglas de negocio: las delegan al dominio.
- La API expone DTOs. **Prohibido** devolver entidades de dominio o documentos de MongoDB.
- Todo mapeo es explícito y tiene prueba.
- Los errores usan `ProblemDetails`; nunca expongas trazas, nombres de colecciones ni detalles internos.

### 7.4 Validación

- Borde: formato, autenticación, autorización.
- Dominio: invariantes del negocio.
- La validación de borde **no sustituye** las invariantes del dominio.

---

## 8. Seguridad

- Autenticación con JWT sin estado; valida firma, emisor, audiencia y expiración.
- Toda operación se autoriza en el servidor. El cliente nunca es fuente de verdad de permisos.
- El cierre de sesión debe invalidar el token de forma verificable (spec 004).
- TLS 1.2 o superior en toda comunicación; nunca desactives la validación de certificados.
- Rate limiting obligatorio en endpoints sensibles; umbrales en configuración (spec 013).
- Secretos solo en AWS Secrets Manager o SSM. **Nunca** en código, `.env` versionado, logs ni tests.
  El secreto JWT actual en `appsettings.json` está pendiente de retiro (specs 001 y 002).
- Logs sin PII, sin tokens y sin contenido moderado. Incluye siempre `correlation-id`.
- **Moderación obligatoria antes de publicar o guardar en S3**, con imagen **y** texto en la misma
  solicitud.
- **Fail-closed**: si la moderación falla o expira, el contenido va a cuarentena. Nunca se publica.
- Umbrales de moderación en configuración; nunca en código. Toda decisión se registra con veredicto,
  categorías, puntajes, umbral aplicado y acción.

---

## 9. Datos y rendimiento

- Toda colección se devuelve paginada: **20 por defecto, 100 máximo**, con metadatos de paginación.
- Toda consulta expuesta tiene un índice declarado y justificado. Sin recorridos completos de colección.
- Lecturas recurrentes con caché y TTL explícito; la invalidación debe estar documentada.
- Respuestas comprimidas y serializador de alto rendimiento.
- Consultas asíncronas con `CancellationToken` propagado hasta la base de datos.
- Los objetivos de latencia se fijan como `NFR` en la spec y se validan con pruebas de carga.

---

## 10. AWS y costo

- Todo recurso se define en `infra/` con AWS CDK en C#. **Prohibido** crear recursos en consola o por CLI.
- Solo servicios del nivel gratuito. Cómputo exclusivamente Lambda detrás de API Gateway.
- Multimedia en S3 + CloudFront; observabilidad exclusivamente en CloudWatch.
- Todo recurso etiquetado con `project`, `environment`, `owner`.
- Toda decisión técnica declara su costo mensual estimado y su consumo frente al Free Tier.
- Si una tarea requiere un servicio con costo, **detente** y solicita aprobación.

---

## 11. Pruebas

| Tipo de cambio | Prueba obligatoria |
|---|---|
| Regla o invariante de dominio | Unitaria (hoy en `Sensus.Application.UnitTests`; separar en `Sensus.Domain.Tests` cuando aplique) |
| Endpoint nuevo o modificado | Integración: éxito, entrada inválida, sin autenticación, sin permisos |
| Persistencia | Integración con Testcontainers contra MongoDB real; nunca dobles que simulen el motor |
| Integración externa (moderación, correo, push, S3) | Doble que simule fallo, timeout y respuesta malformada |
| Corrección de defecto | Regresión que falla antes del arreglo |

- Cobertura mínima: **80 %** de líneas en Domain y Application; 60 % recomendado en el resto.
- Las pruebas son deterministas y no dependen de servicios externos reales.
- El test de arquitectura debe pasar siempre.
- **Prohibido** bajar umbrales, marcar pruebas como omitidas o fusionar con CI en rojo.

---

## 12. Prohibiciones absolutas

- NUNCA añadir bases de datos relacionales, ORM relacional ni migraciones SQL.
- NUNCA integrar una nube distinta de AWS.
- NUNCA crear recursos AWS fuera de `infra/`.
- NUNCA usar servicios fuera del Free Tier sin aprobación.
- NUNCA referenciar `Infrastructure` desde `Domain` o `Application`.
- NUNCA devolver entidades de dominio o documentos de MongoDB.
- NUNCA omitir, desactivar ni simular la moderación.
- NUNCA publicar o almacenar contenido sin moderar.
- NUNCA implementar moderación *fail-open*.
- NUNCA procesar ni almacenar reportes de explotación infantil: se canalizan por vía legal externa.
- NUNCA añadir UI, visión artificial local ni pasarelas de pago.
- NUNCA escribir secretos, cadenas de conexión ni tokens en el repositorio.
- NUNCA registrar PII, tokens completos ni contenido moderado.
- NUNCA exponer colecciones sin paginación.
- NUNCA ejecutar consultas sin índice declarado en rutas críticas.
- NUNCA fijar umbrales de moderación, paginación o rate limiting en código.
- NUNCA ejecutar operaciones de Git (`add`, `commit`, `push`, ramas, merges) sin autorización del
  responsable del proyecto.

---

## 13. Definition of Done

Antes de dar una tarea por terminada:

- [ ] Requisito `FR-<spec>-<nn>`/`NFR-<spec>-<nn>` referenciado y criterios de aceptación cubiertos.
- [ ] Código en la capa correcta; test de arquitectura en verde.
- [ ] Pruebas unitarias y de integración añadidas y en verde.
- [ ] Cobertura ≥ 80 % en Domain y Application.
- [ ] OpenAPI actualizado si cambió un contrato.
- [ ] IaC actualizada y `cdk diff` revisado si cambió infraestructura.
- [ ] Logs estructurados, con `correlation-id` y sin PII.
- [ ] `checklist.md` de la spec actualizado, sin casillas pendientes y con evidencia registrada.
- [ ] `tasks.md` refleja el estado real de cada tarea.
- [ ] Constitution Check del `plan.md` en PASS o brecha registrada.
- [ ] `memory.md` actualizado al cerrar la sesión.

Si algún punto no se cumple, la tarea **no está terminada**. Repórtalo, no lo omitas.

---

## 14. Escalamiento

| Situación | Acción |
|---|---|
| La petición contradice `constitution.md` | Detente y propón enmienda (§4.3) |
| La spec es ambigua o incompleta | Pregunta; no supongas alcance |
| Falta uno de los cuatro artefactos de la spec | Detente y complétalo antes de implementar |
| Se requiere una dependencia o servicio nuevo | Justifica costo, seguridad y alternativas Free Tier |
| Un test falla y no es del alcance actual | Repórtalo; no lo desactives ni lo ocultes |
| Se detecta un secreto en el repositorio | Detente, notifica y rota el secreto |
| Se pide una operación de Git | Confirma autorización explícita antes de ejecutarla |

---

## 15. Convenciones

- **Commits**: Conventional Commits con requisito — `feat(FR-012-03): ...`, `fix(FR-030-01): ...`.
- **Ramas**: una por requisito; `main` siempre desplegable.
- **C#**: `nullable` habilitado, `async/await` sin `.Result` ni `.Wait()`, `record` para DTOs,
  `CancellationToken` en toda operación de E/S.
- **Nombres**: código e identificadores en inglés; specs y documentación en español.
- **Rutas**: `/api/v1` + recurso en inglés y plural (`/merchants`, `/offers`, `/feed/posts`); acciones
  como subrecurso (`/offers/{id}/claim`). `/health/*` queda fuera del prefijo versionado.
- **Contratos**: el catálogo de endpoints vigente es la fuente de rutas; toda ruta nueva MUST añadirse a
  la spec correspondiente antes de implementarse.
- **Formato**: `dotnet format` sin cambios pendientes antes de entregar.
- **Dependencias**: no añadas paquetes sin justificar versión, licencia y costo.

---

**Alineado a `constitution.md` v1.2.0 — actualizar este archivo ante cualquier enmienda MAJOR o MINOR.**
