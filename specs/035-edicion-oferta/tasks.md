# Tareas 035 — Edición de oferta

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
| T035-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-035-01` | — | Pendiente |
| T035-02 | Implementar: Reglas de edición y versión de la oferta | Domain | `FR-035-01` | T035-01 | Pendiente |
| T035-03 | Implementar: UpdateOfferCommand y validador | Application | `FR-035-01` | T035-02 | Pendiente |
| T035-04 | Implementar: Actualización con filtro de versión y re-moderación | Infrastructure | `FR-035-01` | T035-03 | Pendiente |
| T035-05 | Implementar: Endpoint PUT /api/v1/offers/{id} | Api | `FR-035-01` | T035-04 | Pendiente |
| T035-06 | Modelar la versión y las reglas de edición | Domain | `FR-035-05` | T035-05 | Pendiente |
| T035-07 | Calcular el stock reservado antes de validar | Application | `FR-035-04` | T035-06 | Pendiente |
| T035-08 | Aplicar el filtro de versión en la actualización | Infrastructure | `FR-035-05` | T035-07 | Pendiente |
| T035-09 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-035-01` | T035-08 | Pendiente |
| T035-10 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-035-01` | T035-09 | Pendiente |
| T035-11 | Registrar evidencias del Constitution Check | Docs | `FR-035-01` | T035-10 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
