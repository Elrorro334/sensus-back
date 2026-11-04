# Tareas 036 — Ciclo de vida de la oferta

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
| T036-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-036-01` | — | Pendiente |
| T036-02 | Implementar: Máquina de estados de Offer y transiciones válidas | Domain | `FR-036-01` | T036-01 | Pendiente |
| T036-03 | Implementar: ChangeOfferStatusCommand y validador | Application | `FR-036-01` | T036-02 | Pendiente |
| T036-04 | Implementar: Actualización atómica y registro de auditoría | Infrastructure | `FR-036-01` | T036-03 | Pendiente |
| T036-05 | Implementar: Endpoint PUT /api/v1/offers/{id}/status | Api | `FR-036-01` | T036-04 | Pendiente |
| T036-06 | Implementar la máquina de estados | Domain | `FR-036-01` | T036-05 | Pendiente |
| T036-07 | Validar apartados activos antes de archivar | Application | `FR-036-04` | T036-06 | Pendiente |
| T036-08 | Persistir el historial de estados | Infrastructure | `FR-036-06` | T036-07 | Pendiente |
| T036-09 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-036-01` | T036-08 | Pendiente |
| T036-10 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-036-01` | T036-09 | Pendiente |
| T036-11 | Registrar evidencias del Constitution Check | Docs | `FR-036-01` | T036-10 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
