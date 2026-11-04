# Tareas 026 — Feed geolocalizado

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Plan | [`plan.md`](plan.md) |
| Checklist | [`checklist.md`](checklist.md) |
| Estado | Pendiente |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## Tareas

| ID | Tarea | Capa | Requisito | Depende | Estado |
|---|---|---|---|---|---|
| T026-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-026-01` | — | Pendiente |
| T026-02 | Implementar: GetNearbyFeedQuery con proyección | Application | `FR-026-01` | T026-01 | Pendiente |
| T026-03 | Implementar: Índice geoespacial y caché de corta duración | Infrastructure | `FR-026-01` | T026-02 | Pendiente |
| T026-04 | Implementar: Endpoint GET /api/v1/feed/nearby | Api | `FR-026-01` | T026-03 | Pendiente |
| T026-05 | Crear índice geoespacial y de estado | Infrastructure | `FR-026-01` | T026-04 | Pendiente |
| T026-06 | Implementar caché con TTL e invalidación | Application | `FR-026-06` | T026-05 | Pendiente |
| T026-07 | Validar radio y paginación | Api | `FR-026-04` | T026-06 | Pendiente |
| T026-08 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-026-01` | T026-07 | Pendiente |
| T026-09 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-026-01` | T026-08 | Pendiente |
| T026-10 | Registrar evidencias del Constitution Check | Docs | `FR-026-01` | T026-09 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
