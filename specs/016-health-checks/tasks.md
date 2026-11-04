# Tareas 016 — Health checks

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
| T016-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-016-01` | — | Pendiente |
| T016-02 | Implementar: Endpoints de health checks | Api | `FR-016-01` | T016-01 | Pendiente |
| T016-03 | Implementar: Comprobación de conectividad con MongoDB | Infraestructura | `FR-016-01` | T016-02 | Pendiente |
| T016-04 | Separar liveness y readiness | Api | `FR-016-01` | T016-03 | Pendiente |
| T016-05 | Añadir comprobación de MongoDB | Infraestructura | `FR-016-02` | T016-04 | Pendiente |
| T016-06 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-016-01` | T016-05 | Pendiente |
| T016-07 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-016-01` | T016-06 | Pendiente |
| T016-08 | Registrar evidencias del Constitution Check | Docs | `FR-016-01` | T016-07 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
