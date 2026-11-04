# Tareas 005 — Activación del rol comercial

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
| T005-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-005-01` | — | Pendiente |
| T005-02 | Documentar invariantes de Merchant y ubicación | Domain | `FR-005-02` | T005-01 | Pendiente |
| T005-03 | Retirar el endpoint legacy /api/v1/comercio/registro | Api | `FR-005-01` | T005-02 | Pendiente |
| T005-04 | Probar el conflicto de comercio duplicado | Application | `FR-005-03` | T005-03 | Pendiente |
| T005-05 | Verificar el índice 2dsphere con explain | Infrastructure | `FR-005-06` | T005-04 | Pendiente |
| T005-06 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-005-01` | T005-05 | Pendiente |
| T005-07 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-005-01` | T005-06 | Pendiente |
| T005-08 | Registrar evidencias del Constitution Check | Docs | `FR-005-01` | T005-07 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
