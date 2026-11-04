# Tareas 041 — Búsqueda de ofertas

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
| T041-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-041-01` | — | Pendiente |
| T041-02 | Implementar: SearchOffersQuery, cálculo de puntaje y DTO | Application | `FR-041-01` | T041-01 | Pendiente |
| T041-03 | Implementar: Índice de texto en MongoDB y consulta geoespacial combinada | Infrastructure | `FR-041-01` | T041-02 | Pendiente |
| T041-04 | Implementar: Endpoint GET /api/v1/ml/search | Api | `FR-041-01` | T041-03 | Pendiente |
| T041-05 | Definir la función de puntaje y sus pesos | Domain | `FR-041-02` | T041-04 | Pendiente |
| T041-06 | Crear el índice de texto y verificar su uso | Infrastructure | `FR-041-01` | T041-05 | Pendiente |
| T041-07 | Implementar el fallback a ofertas cercanas | Application | `FR-041-05` | T041-06 | Pendiente |
| T041-08 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-041-01` | T041-07 | Pendiente |
| T041-09 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-041-01` | T041-08 | Pendiente |
| T041-10 | Registrar evidencias del Constitution Check | Docs | `FR-041-01` | T041-09 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
