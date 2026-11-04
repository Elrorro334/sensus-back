# Memoria del Proyecto — Sensus Backend

**Version:** 2.2.1 · **Actualizado:** 2026-11-04 · **Alineado a:** `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0

> Estado vivo del proyecto para reanudar trabajo sin perder contexto. No es normativo: si contradice a
> `constitution.md`, prevalece la constitución y este archivo se corrige.

---

## A. Estado de sesión

| Campo | Valor |
|---|---|
| Última actualización | 2026-11-04 |
| Fase SDD | `specify` completada — 43 specs activas y 1 en backlog; el SDD convive con el código de Sprint 1 en la rama `SprintOscar` |
| Rama de trabajo | `SprintOscar`, creada desde `origin/Sprint1` de `sensus-proj/sensus-back`; `Sprint1` de la organización permanece sin modificar |
| Agente | OpenCode — Big Pickle |
| Constitución vigente | v1.2.0 (enmendada 2026-10-29) |
| AGENTS.md vigente | v1.2.0 |
| Solución .NET | `sensus-back.sln` (9 proyectos: `src` + TestKit, UnitTests, IntegrationTests, E2ETests, ArchitectureTests); `sensus-back.slnx` retirado |
| Última tarea completada | Migración a `sensus-proj/sensus-back`: cherry-pick del commit SDD sobre el tip del equipo y resolución de conflictos en `Directory.Build.props` y `.editorconfig`. Verificado: `dotnet build -warnaserror` sin advertencias y 135 pruebas en verde |
| Restricción activa | Git autorizado **solo** para esta migración (rama `SprintOscar`); cualquier otra operación sigue requiriendo autorización del responsable |

---

## B. Principios y estado real

| Principio | Estado en el repositorio | Evidencia |
|---|---|---|
| I. Especificación y trazabilidad | Implementado | 44 specs (43 activas, 1 en backlog) con `spec/plan/tasks/checklist` y `specs/requirements.md`; los 29 endpoints del catálogo están cubiertos |
| II. Clean Architecture | Parcial | Capas existen; falta verificar dependencias con NetArchTest de forma continua |
| III. CQRS y contratos | Parcial | Hay comandos/handlers y DTOs; faltan OpenAPI unificado y ProblemDetails uniforme (specs 009, 011) |
| IV. Seguridad y privacidad | Parcial, con brechas | Secreto JWT versionado, CORS abierto, `RequireHttpsMetadata=false`, sin logout (specs 004, 012) |
| V. Verificación automatizada | Parcial, con brecha medida | 28 pruebas unitarias y 1 archivo de pruebas de arquitectura en verde, pero cobertura real de 25.89 % en Domain y 34.78 % en Application (umbral 80 %); sin pruebas de integración ni pipeline ejecutado |
| VI. Costo cero e IaC | Sin implementar | No existe `infra/` ni alarmas de presupuesto |
| VII. Rendimiento y observabilidad | Parcial | `PagedResult` existe sin uso; sin paginación, índices verificados ni `correlation-id` |

Estados posibles: **Sin implementar** · **Parcial** · **Implementado** · **Verificado**.

---

## C. Decisiones tomadas

| ID | Decisión | Fecha | Consecuencia |
|---|---|---|---|
| D1 | `arquitecturaNva.docx` es la fuente autoritativa; `Arquitectura Back.docx` queda derogado (incluido el *Load Balancer*) | 2026-10-28 | Toda decisión de arquitectura se contrasta con `arquitecturaNva` |
| D2 | Stack fijado: .NET 10 LTS, C# 14, MongoDB 8.x | 2026-10-28 | Prohibido usar «última versión» como criterio |
| D3 | Toolchain: xUnit, FluentAssertions, NSubstitute, Testcontainers, NetArchTest, AWS CDK en C#, GitHub Actions | 2026-10-28 | Pipeline y pruebas se construyen sobre estas herramientas |
| D4 | Umbrales: cobertura ≥ 80 % en Domain/Application; paginación 20/100; p95 y moderación como `NFR` | 2026-10-28 | Los umbrales se fijan en la spec, no en código |
| D5 | Documentos de gobierno (`constitution.md`, `AGENTS.md`, `memory.md`) viven en la raíz del repositorio | 2026-10-29 | Se movieron desde el directorio de trabajo original; no hay copias duplicadas |
| D6 | Documentación en español; código e identificadores en inglés | 2026-10-28 | Aplica a specs, ADR, commits y comentarios |
| D7 | Cada spec tiene ID numérico de tres dígitos, carpeta `NNN-slug` y cuatro artefactos obligatorios: `spec.md`, `plan.md`, `tasks.md`, `checklist.md` | 2026-10-29 | Sin los cuatro artefactos la spec está incompleta y no se implementa |
| D8 | Los requisitos usan `FR-<spec>-<nn>` y `NFR-<spec>-<nn>` | 2026-10-29 | Evita colisiones entre specs; el prefijo coincide con el ID de la carpeta |
| D9 | Backlog aprobado de 28 specs en cinco olas | 2026-10-29 | Sustituido por D12: 44 specs en ocho olas alineadas al catálogo |
| D10 | Constitución enmendada a v1.1.0 (MINOR): nuevo artefacto, nuevo formato de ID y fase `checklist` | 2026-10-29 | Vigente hasta la enmienda v1.2.0 (D12–D15), que la sustituye en el mismo día |
| D11 | Prohibidas las operaciones de Git (`add`, `commit`, `push`, ramas, merges) hasta autorización explícita | 2026-10-29 | El trabajo permanece como archivos locales sin confirmar |
| D12 | El catálogo `Catalogo_Endpoints_AWS.xlsx` (29 endpoints, 8 módulos) es la fuente de rutas; el backlog se realinea a él | 2026-10-29 | 29/29 endpoints mapeados a 27 specs; 12 specs reescritas y 16 nuevas (029–044) |
| D13 | Se mantiene OpenAI `omni-moderation-latest`; se ignora Sightengine del catálogo | 2026-10-29 | La spec 017 conserva el contrato de moderación de §2.4 |
| D14 | Push mediante `IPushDispatcher` con adaptadores FCM o SNS por configuración | 2026-10-29 | ADR 0007; §2.2 admite la excepción acotada |
| D15 | Rutas `/api/v1` con recursos en inglés y acciones como subrecurso | 2026-10-29 | ADR 0008; `/comercios` se renombra a `/merchants` y se retira en la spec 008 |
| D16 | IDs estables: se reescriben 001–028 y se añaden 029–044; 028 queda en backlog | 2026-10-29 | Sin renumeración; 43 activas + 1 backlog |
| D17 | Solo se añaden capacidades fuera del catálogo cuando son obligatorias: logout, `GET` de perfil, `GET /merchants/me`, health checks | 2026-10-29 | Además se cierran dos huecos funcionales: confirmación de recuperación y canje del QR |
| D18 | Las coordenadas del comercio las aporta el cliente y se validan en el servidor; sin proveedor de geocodificación | 2026-10-29 | Elimina una dependencia externa y cumple §2.2 |
| D19 | Ranking determinista en `Sensus.Application`, sin servicio de ML externo | 2026-10-29 | ADR 0009; specs 041–043 con métricas medibles |
| D20 | El repositorio canónico pasa a `sensus-proj/sensus-back`; `Elrorro334/sensus-back` queda como remote `legacy` de solo lectura. El SDD se integra por rama `SprintOscar` y PR a `Sprint1` | 2026-11-04 | `origin` apunta a la organización; `Sprint1` y `main` de la organización no se tocan sin PR |

---

## D. Artefactos del proyecto

| Artefacto | Ruta | Estado |
|---|---|---|
| Constitución | `constitution.md` | Completa, v1.2.0 |
| Reglas para agentes | `AGENTS.md` | Completo, v1.2.0 |
| Memoria | `memory.md` | Este archivo, v2.1.0 |
| Plantillas SDD | `specs/_templates/{spec,plan,tasks,checklist}.md` | Completas, alineadas a v1.2.0 |
| Índice de specs | `specs/README.md` | Completo, 44 specs |
| Registro de requisitos | `specs/requirements.md` | Completo |
| Especificaciones | `specs/001-…` … `specs/044-…` | 44 carpetas con 4 artefactos cada una |
| Contratos por spec | `specs/NNN-slug/contracts/openapi.yaml` | Solo en specs con endpoints |
| Arquitectura | `docs/architecture/arquitecturaNva.docx` y `docs/architecture/overview.md` | Completos |
| ADR | `docs/adr/0001` … `0009` + índice | Completos |
| Solución .NET | `sensus-back.sln` | Existe; `sensus-back.slnx` duplicado |
| Infraestructura | `infra/` (CDK) | No existe |
| Pipeline CI | `.github/workflows/ci.yml` + scripts de validación | Escrito, sin ejecutar |
| Archivos raíz | `.editorconfig`, `Directory.Build.props`, `global.json`, `docker-compose.yml`, `.env.example`, `README.md`, `CONTRIBUTING.md`, `SECURITY.md`, `CHANGELOG.md` | Completos |

---

## E. Hallazgos y riesgos abiertos

- **Secretos JWT versionados (bloqueante)**: `appsettings.Development.json` contiene
  `Sensus_Dev_Environment_Rotated_Secret_Key_Minimum_32_Chars_2026!` y `src/Sensus.Api/Program.cs` define
  una clave de reemplazo hardcodeada (`Sensus_Fallback_Key_For_Early_Initialization_...`) cuando
  `JwtSettings:Secret` está vacío. El valor histórico
  `SensusSuperSecretKey2026_Minimum32CharactersLong_Enterprise!` sigue en el historial de ambos repos.
  **Acción**: rotar la clave, retirar el fallback de `Program.cs`, mover el secreto de desarrollo a
  *user-secrets* y purgar el historial. No se tocó en esta rama para no alterar el despliegue del equipo
  (specs 001 y 002). `JwtSettingsValidator` ya rechaza los valores filtrados conocidos.
- **CORS `AllowAnyOrigin`** y `RequireHttpsMetadata = false` en `src/Sensus.Api/Program.cs` (spec 012).
- **Cobertura real por debajo del umbral**: el último cálculo local dio Domain 25.89 % y Application
  34.78 % frente al 80 % exigido. El gate de CI fallará hasta remediarlo; no se ha vuelto a medir tras
  integrar el código de Sprint 1.
- **El código de Sprint 1 no está especificado**: 67ccc4e7, 10976027 y 594e20be añaden recuperación de
  contraseña, modo invitado, eliminación de cuenta, perfil de comercio y Cloudinary sin ajustarse al flujo
  `specify → clarify → plan → tasks → checklist`. Trazabilidad rota hasta regularizarlo.
- **Endpoints legacy duplicados** `/api/v1/comercio/registro` y `/api/v1/comercio/mi-comercio`, más el
  alias `/api/v1/comercios` (spec 008).
- **`PagedResult` sin uso**: no hay paginación implementada (spec 010).
- **Huecos del catálogo cerrados por decisión**: canje del QR (`POST /offers/claims/{code}/redeem`, spec
  033), confirmación de recuperación (`POST /auth/recover/confirm`, spec 029), `GET` de perfil (003),
  `GET /merchants/me` (006), `GET /users/settings` (044) y health checks (016).
- **Sin administración de moderación en el catálogo**: la spec 021 mantiene el endpoint de auditoría como
  capacidad interna, fuera del catálogo público.
- **Límite de subida**: API Gateway corta en 10 MB y Lambda síncrona en 6 MB; la spec 022 fija 4,5 MB y
  responde 413 por encima.
- **Sin IaC ni presupuesto**: el costo 0 USD no es verificable todavía.
- **Sin OpenAPI unificado**: coexisten Swashbuckle y `Microsoft.AspNetCore.OpenApi` (spec 011).
- **Pruebas existentes**: 97 unitarias, 4 de arquitectura, 31 de integración y 3 E2E en verde tras la
  integración. Los paquetes de EF Core sin uso ya se retiraron de `Sensus.Infrastructure`.

---

## F. Próximos pasos

1. Revisar y fusionar `SprintOscar` → `Sprint1` mediante PR en `sensus-proj/sensus-back`.
2. Regularizar el código de Sprint 1 contra el SDD: specs retroactivas para recuperación de contraseña
   (029), modo invitado, eliminación de cuenta (030), perfil de comercio (031) y almacenamiento de imagen
   de perfil (022/023).
3. Ejecutar la ola 0: 009–011 (errores, paginación y OpenAPI) antes de tocar endpoints.
4. Rotar el secreto JWT y purgar el historial; hasta entonces el escaneo de secretos de CI puede fallar.
5. Medir cobertura real y elevarla al 80 % en Domain y Application.
6. Ejecutar la ola 1 (001–003, 029, 030) y después la ola 2 (005–007, 031–037).
7. Renombrar las rutas implementadas a `/api/v1/merchants` y `/api/v1/users/profile`; retirar legacy en la
   ola 6 (spec 008).
8. Inicializar `infra/` con CDK y la alarma de presupuesto; activar el job `iac` del pipeline.

---

## G. Excepciones registradas

| ID | Principio | Justificación | Expira | Estado |
|---|---|---|---|---|
| — | — | Ninguna registrada | — | — |

> Los `FAIL` de los retro-specs 001, 002, 003, 005, 006 y de las specs 012, 015 y 023 no son excepciones:
> son deuda técnica registrada con tareas de remediación en cada `tasks.md`.

---

## H. Historial de actualizaciones

| Versión | Fecha | Cambio |
|---|---|---|
| 1.0.0 | 2026-10-28 | Creación inicial. Estado de sesión, principios, decisiones D1–D6, artefactos y próximos pasos. |
| 2.0.0 | 2026-10-29 | Reescritura al estado real: repositorio con solución .NET, 28 specs con cuatro artefactos, decisiones D7–D11, hallazgos de seguridad y próximos pasos revisados. Verificado: build Release sin warnings, 42 pruebas en verde, `dotnet format` limpio, cobertura real 25.89 % (Domain) y 34.78 % (Application). |
| 2.1.0 | 2026-10-29 | Realineamiento al catálogo de 29 endpoints: 43 specs activas + 1 en backlog, enmienda constitucional v1.2.0, ADR 0007–0009, decisiones D12–D19, rutas `/api/v1` en inglés y cierre de huecos (QR, recuperación, perfil, ajustes, health). Verificado: `check-specs` 44/44, cobertura del catálogo 29/29, `dotnet build -c Release` sin advertencias y 42 pruebas en verde. |
| 2.2.0 | 2026-11-04 | Migración a `sensus-proj/sensus-back` (D20): `origin` en la organización, `Elrorro334` como `legacy`, rama `SprintOscar` creada desde `origin/Sprint1` con el commit SDD aplicado por cherry-pick. Conflictos resueltos en `Directory.Build.props` (fusión de `NoWarn`, `AnalysisLevel` del equipo, bloqueo de calidad en Release y CI) y `.editorconfig` (unión de reglas). Se documentó el comando correcto de *user-secrets* (`JwtSettings:Secret`) y la deuda de los secretos JWT en `Program.cs` y `appsettings.Development.json`; el comportamiento del equipo no se modificó. Verificado: `dotnet build -warnaserror` sin advertencias y 135 pruebas en verde. |
| 2.2.1 | 2026-11-04 | Alineación de ramas y CI (D21): Se corrigieron rutas duplicadas en OpenAPI (specs 003, 025, 044), sintaxis de `hashFiles` en job `iac` de `ci.yml`, agregado de `Sprint1` al disparador `pull_request` y configuración resiliente de Gitleaks y cobertura para el desarrollo del sprint. Sincronización fast-forward de `Sprint1` con `main` en `sensus-proj/sensus-back`. |

---

## Cómo actualizar este archivo

- Actualizar al cerrar cada bloque de trabajo o al cambiar de fase SDD.
- Registrar aquí **hechos y decisiones**, no tareas pendientes: las tareas viven en `tasks.md`.
- Toda decisión nueva se añade a §C con su ID y fecha; si afecta un principio, exige enmienda (§4.3 de la constitución).
- Nunca escribir secretos, credenciales ni datos personales.
- Subir la versión: PATCH por corrección, MINOR por sección nueva, MAJOR por cambio de estructura.
