# Tareas 043 — Alternativas a una oferta

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
| T043-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-043-01` | — | Pendiente |
| T043-02 | Implementar: GetAlternativesQuery y proyección | Application | `FR-043-01` | T043-01 | Pendiente |
| T043-03 | Implementar: Consulta por categoría + 2dsphere + rango de precio | Infrastructure | `FR-043-01` | T043-02 | Pendiente |
| T043-04 | Implementar: Endpoint GET /api/v1/ml/alternatives | Api | `FR-043-01` | T043-03 | Pendiente |
| T043-05 | Implementar el filtro por categoría y precio | Application | `FR-043-04` | T043-04 | Pendiente |
| T043-06 | Crear el índice category + status + price | Infrastructure | `FR-043-01` | T043-05 | Pendiente |
| T043-07 | Validar el límite y el 404 | Api | `FR-043-07` | T043-06 | Pendiente |
| T043-08 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-043-01` | T043-07 | Pendiente |
| T043-09 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-043-01` | T043-08 | Pendiente |
| T043-10 | Registrar evidencias del Constitution Check | Docs | `FR-043-01` | T043-09 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
