# Tareas 008 — Retiro de endpoints legacy

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
| T008-01 | Implementar: Eliminar MapGroup y endpoints legacy | Api | `FR-008-01` | — | Pendiente |
| T008-02 | Implementar: Actualizar README, .http y OpenAPI | Documentación | `FR-008-01` | T008-01 | Pendiente |
| T008-03 | Eliminar el grupo legacy y sus handlers | Api | `FR-008-01` | T008-02 | Pendiente |
| T008-04 | Actualizar README y Sensus.Api.http | Documentación | `FR-008-04` | T008-03 | Pendiente |
| T008-05 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-008-01` | T008-04 | Pendiente |
| T008-06 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-008-01` | T008-05 | Pendiente |
| T008-07 | Registrar evidencias del Constitution Check | Docs | `FR-008-01` | T008-06 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
