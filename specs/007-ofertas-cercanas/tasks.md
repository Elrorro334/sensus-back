# Tareas 007 — Ofertas cercanas

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
| T007-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-007-01` | — | Pendiente |
| T007-02 | Implementar: GetNearbyOffersQuery, validador y proyección a DTO | Application | `FR-007-01` | T007-01 | Pendiente |
| T007-03 | Implementar: Consulta geoespacial con índice 2dsphere y filtro de estado | Infrastructure | `FR-007-01` | T007-02 | Pendiente |
| T007-04 | Implementar: Endpoint GET /api/v1/offers/nearby | Api | `FR-007-01` | T007-03 | Pendiente |
| T007-05 | Definir el DTO de oferta cercana con distancia | Application | `FR-007-07` | T007-04 | Pendiente |
| T007-06 | Verificar el uso del índice 2dsphere con explain | Infrastructure | `FR-007-01` | T007-05 | Pendiente |
| T007-07 | Validar radio y pageSize en el borde | Api | `FR-007-06` | T007-06 | Pendiente |
| T007-08 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-007-01` | T007-07 | Pendiente |
| T007-09 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-007-01` | T007-08 | Pendiente |
| T007-10 | Registrar evidencias del Constitution Check | Docs | `FR-007-01` | T007-09 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
