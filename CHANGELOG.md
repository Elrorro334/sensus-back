# Changelog — Sensus Backend

Todos los cambios relevantes se documentan aquí. El formato sigue
[Keep a Changelog](https://keepachangelog.com/es-ES/1.1.0/) y el versionado, SemVer.

> Mientras el proyecto no tenga versiones publicadas, los cambios se agrupan bajo `[No publicado]`.
> La constitución y los artefactos SDD tienen su propio historial en `constitution.md` §4.6 y
> `memory.md` §H.

---

## [No publicado]

### Añadido

- Gobernanza del proyecto: `constitution.md` v1.2.0, `AGENTS.md` v1.2.0 y `memory.md` v2.1.0.
- Metodología SDD con 44 especificaciones en `specs/` (43 activas y 1 en backlog), cada una con `spec.md`,
  `plan.md`, `tasks.md` y `checklist.md`.
- Alineación completa al catálogo de 29 endpoints en 8 módulos: 12 specs reescritas y 16 nuevas (029–044).
- ADR 0007 (despacho push abstracto), 0008 (rutas versionadas en inglés) y 0009 (ranking determinista).
- Índice de specs (`specs/README.md`) y registro consolidado de requisitos (`specs/requirements.md`).
- Plantillas de los cuatro artefactos en `specs/_templates/`.
- Borradores de contrato OpenAPI por spec en `specs/NNN-slug/contracts/openapi.yaml`.
- Documentación de arquitectura en `docs/architecture/`.
- Arranque local reproducible: `docker-compose.yml`, `.env.example` y `global.json`.
- `Directory.Build.props` con `Nullable`, `LangVersion` y calidad bloqueante en Release/CI.
- `.editorconfig` con reglas de estilo y de análisis alineadas a `AGENTS.md`.
- Guías `CONTRIBUTING.md` y `SECURITY.md`.

### Cambiado

- Identificadores de requisito al formato `FR-<spec>-<nn>` y `NFR-<spec>-<nn>`.
- `checklist.md` pasa a ser artefacto obligatorio de cada spec.
- Rutas públicas a `/api/v1` con recursos en inglés (`/merchants`, `/offers`, `/feed/posts`).
- `README.md` reescrito: sin datos personales, sin rutas locales y con el flujo SDD.

### Corregido

- Alineación de versiones: constitución, `AGENTS.md` y las 44 specs citan v1.2.0.
- `app.Run()` reemplazado por `await app.RunAsync()` en `Program.cs` (CA1849).
- Espaciado en `MongoDbContext.cs` corregido para que `dotnet format` quede limpio.

### Seguridad

- Documentada la deuda abierta: secreto JWT versionado, CORS abierto, `RequireHttpsMetadata` desactivado,
  ausencia de revocación de sesión y de rate limiting. Ver `SECURITY.md`.

### Pendiente

- Proyecto `tests/Sensus.IntegrationTests/` con Testcontainers.
- Pipeline CI en `.github/workflows/ci.yml`.
- Infraestructura AWS CDK en `infra/` con alarma de presupuesto.
- Rotación del secreto JWT y retiro de los paquetes de EF Core sin uso.
- ADR 0001–0006.

---

## [0.1.0] — 2026-10-28

### Añadido

- Registro de usuario con correo único, contraseña BCrypt, aceptación de términos y rol `Consumidor`
  (`POST /api/v1/auth/register`).
- Inicio de sesión con JWT y mensaje de error genérico (`POST /api/v1/auth/login`).
- Consulta del perfil autenticado (`GET /api/v1/auth/me`).
- Registro y consulta de comercio (`POST /api/v1/comercios`, `GET /api/v1/comercios/me`).
- Solución .NET 10 con capas Domain, Application, Infrastructure y Api.
- Pruebas unitarias de handlers y validadores, y pruebas de arquitectura con NetArchTest.

[No publicado]: https://example.invalid/sensus-back/compare
[0.1.0]: https://example.invalid/sensus-back/releases/tag/v0.1.0
