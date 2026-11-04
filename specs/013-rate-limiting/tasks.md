# Tareas 013 — Control de tasa de peticiones

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
| T013-01 | Implementar: Middleware de rate limiting y políticas por endpoint | Api | `FR-013-01` | — | Pendiente |
| T013-02 | Implementar: Configuración de umbrales por entorno | Infraestructura | `FR-013-01` | T013-01 | Pendiente |
| T013-03 | Implementar políticas de límite por endpoint | Api | `FR-013-01` | T013-02 | Pendiente |
| T013-04 | Externalizar umbrales a configuración | Infraestructura | `FR-013-04` | T013-03 | Pendiente |
| T013-05 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-013-01` | T013-04 | Pendiente |
| T013-06 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-013-01` | T013-05 | Pendiente |
| T013-07 | Registrar evidencias del Constitution Check | Docs | `FR-013-01` | T013-06 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
