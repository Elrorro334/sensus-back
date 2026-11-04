# Tareas 006 — Consulta de mi comercio

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Plan | [`plan.md`](plan.md) |
| Checklist | [`checklist.md`](checklist.md) |
| Estado | Remediación pendiente |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## Tareas

| ID | Tarea | Capa | Requisito | Depende | Estado |
|---|---|---|---|---|---|
| T006-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-006-01` | — | Pendiente |
| T006-02 | Retirar el endpoint legacy /api/v1/comercio/mi-comercio | Api | `FR-006-01` | T006-01 | Pendiente |
| T006-03 | Probar el caso 404 | Application | `FR-006-02` | T006-02 | Pendiente |
| T006-04 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-006-01` | T006-03 | Pendiente |
| T006-05 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-006-01` | T006-04 | Pendiente |
| T006-06 | Registrar evidencias del Constitution Check | Docs | `FR-006-01` | T006-05 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
