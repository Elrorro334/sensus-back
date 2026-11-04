# Tareas 042 — Recomendaciones personalizadas

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
| T042-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-042-01` | — | Pendiente |
| T042-02 | Implementar: GetRecommendationsQuery y función de puntaje | Application | `FR-042-01` | T042-01 | Pendiente |
| T042-03 | Implementar: Agregaciones de señales y caché por usuario | Infrastructure | `FR-042-01` | T042-02 | Pendiente |
| T042-04 | Implementar: Endpoint GET /api/v1/ml/recommendations | Api | `FR-042-01` | T042-03 | Pendiente |
| T042-05 | Implementar la función de puntaje con pesos configurables | Application | `FR-042-02` | T042-04 | Pendiente |
| T042-06 | Agregar señales de favoritos, me gusta y apartados | Infrastructure | `FR-042-03` | T042-05 | Pendiente |
| T042-07 | Implementar el fallback sin señales | Application | `FR-042-05` | T042-06 | Pendiente |
| T042-08 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-042-01` | T042-07 | Pendiente |
| T042-09 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-042-01` | T042-08 | Pendiente |
| T042-10 | Registrar evidencias del Constitution Check | Docs | `FR-042-01` | T042-09 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
