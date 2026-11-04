# Sensus Backend

API RESTful del backend de **Sensus**, una plataforma social con contenido generado por usuarios.
Construida con **.NET 10 (LTS)** y **C# 14** sobre **MongoDB 8.x**, con **Clean Architecture**,
**CQRS**, autenticación **JWT** y moderación de contenido previa a la publicación.

> Este repositorio contiene **solo el backend**. La aplicación móvil, la web, la *landing*, la visión
> artificial local y las pasarelas de pago están fuera de su alcance (`constitution.md` §2.2).

---

## Índice

1. [Estado del proyecto](#1-estado-del-proyecto)
2. [Documentos de gobierno](#2-documentos-de-gobierno)
3. [Arquitectura](#3-arquitectura)
4. [Requisitos previos](#4-requisitos-previos)
5. [Puesta en marcha](#5-puesta-en-marcha)
6. [Endpoints disponibles](#6-endpoints-disponibles)
7. [Pruebas](#7-pruebas)
8. [Especificaciones (SDD)](#8-especificaciones-sdd)
9. [Seguridad](#9-seguridad)
10. [Contribuir](#10-contribuir)

---

## 1. Estado del proyecto

| Aspecto | Estado |
|---|---|
| Autenticación (`register`, `login`) | Implementado (001, 002); falta alinear `GET /users/profile` (003) |
| Comercios (`POST /comercios`, `GET /comercios/me`) | Implementado con rutas en español; el contrato vigente es `/merchants` (005, 006, 008) |
| Ofertas, apartado con QR, inventario | Especificado, sin implementar (007, 032–037) |
| Feed social, favoritos, reportes | Especificado, sin implementar (024–026, 038–040) |
| Búsqueda y recomendaciones | Especificado, sin implementar (041–043) |
| Moderación, multimedia, push | Especificado, sin implementar (017–022, 027, 044) |
| Pruebas de integración | Pendientes — bloquean 8 specs en `FAIL` de Constitution Check |
| Infraestructura como código (AWS CDK) | Pendiente |
| Pipeline CI | Escrito (`.github/workflows/ci.yml`), sin ejecutar |

El estado vivo y los siguientes pasos se mantienen en [`memory.md`](memory.md).

---

## 2. Documentos de gobierno

| Documento | Propósito |
|---|---|
| [`constitution.md`](constitution.md) | Norma de máxima autoridad: siete principios, restricciones de plataforma y gobernanza |
| [`AGENTS.md`](AGENTS.md) | Reglas operativas para trabajar en el repositorio |
| [`memory.md`](memory.md) | Estado actual, decisiones y próximos pasos |
| [`docs/architecture/arquitecturaNva.docx`](docs/architecture/arquitecturaNva.docx) | Fuente autoritativa de arquitectura |
| [`docs/adr/`](docs/adr/) | Decisiones de arquitectura (ADR) |

Orden de precedencia: `constitución > spec > plan > tasks > checklist > código > convenciones locales`.

---

## 3. Arquitectura

```
src/
  Sensus.Domain/           Entidades, invariantes y contratos. Sin dependencias externas.
  Sensus.Application/      Casos de uso CQRS (MediatR), DTOs, validación con FluentValidation.
  Sensus.Infrastructure/   MongoDB, BCrypt, JWT, S3, moderación, correo, push.
  Sensus.Api/              Endpoints mínimos, middleware y composición (Program.cs).
tests/
  Sensus.Application.UnitTests/
  Sensus.ArchitectureTests/
```

Reglas de dependencia: `Application → Domain`, `Infrastructure → Application`, `Api → Application`.
`Domain` y `Application` **nunca** referencian `Infrastructure`. La composición se hace únicamente en
`src/Sensus.Api/Program.cs`.

Detalle ampliado en [`docs/architecture/overview.md`](docs/architecture/overview.md).

---

## 4. Requisitos previos

- [.NET SDK 10.0.401](https://dotnet.microsoft.com/download) o superior dentro del mismo *feature band*
  (fijado en [`global.json`](global.json)).
- Docker Desktop (MongoDB local y Testcontainers).
- Opcional: AWS CLI y AWS CDK para `infra/`.

---

## 5. Puesta en marcha

```bash
# 1. Dependencias
dotnet restore sensus-back.sln

# 2. MongoDB local
docker compose up -d mongo

# 3. Configuración local (copiar y completar; .env no se versiona)
cp .env.example .env

# 4. Compilar y ejecutar
dotnet build sensus-back.sln
dotnet run --project src/Sensus.Api
```

La API queda en `https://localhost:5001`. En desarrollo el documento OpenAPI está disponible en
`/openapi/v1.json` y la interfaz de Swagger en `/swagger`.

**Variables de entorno obligatorias:** `MONGO_CONNECTION_STRING`, `JWT__SECRET`, `JWT__ISSUER`,
`JWT__AUDIENCE`. En local se pueden usar *user-secrets*:

```bash
dotnet user-secrets --project src/Sensus.Api set "Jwt:Secret" "<valor-generado>"
```

> El secreto JWT que figura en `src/Sensus.Api/appsettings.json` está comprometido y debe rotarse antes
> de cualquier despliegue (specs 001 y 002).

---

## 6. Endpoints implementados

| Método | Ruta implementada | Ruta del contrato vigente | Autorización | Spec |
|---|---|---|---|---|
| `POST` | `/api/v1/auth/register` | igual | Pública | [001](specs/001-registro-usuario/spec.md) |
| `POST` | `/api/v1/auth/login` | igual | Pública | [002](specs/002-inicio-sesion/spec.md) |
| `GET` | `/api/v1/auth/me` | `GET /api/v1/users/profile` | JWT | [003](specs/003-perfil-usuario/spec.md) |
| `POST` | `/api/v1/comercios` | `POST /api/v1/merchants/activate` | JWT | [005](specs/005-activacion-comerciante/spec.md) |
| `GET` | `/api/v1/comercios/me` | `GET /api/v1/merchants/me` | JWT | [006](specs/006-consulta-mi-comercio/spec.md) |

Las rutas legacy `/api/v1/comercio/registro`, `/api/v1/comercio/mi-comercio` y los alias en español se
retiran en el spec [008](specs/008-retiro-endpoints-legacy/spec.md).

El catálogo completo —29 endpoints en 8 módulos— está mapeado en
[`specs/README.md`](specs/README.md). Los ejemplos de invocación viven en
[`src/Sensus.Api/Sensus.Api.http`](src/Sensus.Api/Sensus.Api.http).

---

## 7. Pruebas

```bash
# Toda la solución con cobertura
dotnet test sensus-back.sln --collect:"XPlat Code Coverage"

# Solo unitarias
dotnet test tests/Sensus.Application.UnitTests

# Solo arquitectura
dotnet test tests/Sensus.ArchitectureTests
```

- Cobertura mínima exigida: **80 %** de líneas en `Domain` y `Application`.
- La persistencia se prueba con **Testcontainers** contra MongoDB real, nunca con dobles del motor.
- El proyecto `tests/Sensus.IntegrationTests/` está pendiente de creación.

---

## 8. Especificaciones (SDD)

El proyecto sigue Spec-Driven Development: no se implementa nada que no esté especificado.

```
specs/
  README.md          Índice de las 44 specs (43 activas y 1 en backlog)
  requirements.md    Registro de todos los FR/NFR
  _templates/        Plantillas de spec, plan, tasks y checklist
  NNN-slug/
    spec.md          Alcance, requisitos y criterios de aceptación
    plan.md          Diseño técnico y Constitution Check
    tasks.md         Tareas atómicas trazables
    checklist.md     Definition of Ready / Done y evidencia
    contracts/       OpenAPI del spec (si expone endpoints)
```

- Cada spec tiene un ID numérico estable y cuatro artefactos obligatorios.
- Los requisitos usan el formato `FR-<spec>-<nn>` y `NFR-<spec>-<nn>` (ejemplo `FR-017-03`).
- Punto de entrada: [`specs/README.md`](specs/README.md).

---

## 9. Seguridad

- Sin secretos en el repositorio: usa *user-secrets* en local y AWS Secrets Manager en la nube.
- JWT validado en firma, emisor, audiencia y expiración; cierre de sesión con revocación (spec 004).
- TLS obligatorio fuera de desarrollo y CORS restringido por entorno (spec 012).
- Rate limiting en endpoints sensibles (spec 013).
- Moderación obligatoria y *fail-closed* antes de publicar contenido (specs 017–021).
- Logs sin PII, sin tokens y con `correlation-id` (spec 014).

Para reportar una vulnerabilidad, consulta [`SECURITY.md`](SECURITY.md). **No** abras un issue público.

---

## 10. Contribuir

Lee [`CONTRIBUTING.md`](CONTRIBUTING.md) y [`AGENTS.md`](AGENTS.md) antes de abrir un cambio.
Resumen:

1. Verifica que la capacidad está especificada en `specs/`.
2. Sigue el orden `specify → clarify → plan → tasks → checklist → implement`.
3. Referencia el requisito (`FR-<spec>-<nn>`) en el PR y en los commits.
4. No fusiones con verificaciones en rojo.

---

**Alineado a `constitution.md` v1.2.0 y `AGENTS.md` v1.2.0.**
