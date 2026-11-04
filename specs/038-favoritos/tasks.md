# Tareas 038 — Favoritos de ofertas

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
| T038-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-038-01` | — | Pendiente |
| T038-02 | Implementar: Regla de favorito único por usuario y oferta | Domain | `FR-038-01` | T038-01 | Pendiente |
| T038-03 | Implementar: ToggleFavoriteCommand | Application | `FR-038-01` | T038-02 | Pendiente |
| T038-04 | Implementar: Índice único userId + offerId y contador atómico | Infrastructure | `FR-038-01` | T038-03 | Pendiente |
| T038-05 | Implementar: Endpoint POST /api/v1/users/favorites/{id} | Api | `FR-038-01` | T038-04 | Pendiente |
| T038-06 | Modelar la unicidad del favorito | Domain | `FR-038-04` | T038-05 | Pendiente |
| T038-07 | Implementar índice único y contador atómico | Infrastructure | `FR-038-02` | T038-06 | Pendiente |
| T038-08 | Publicar la afinidad de categoría al ranking | Application | `FR-038-05` | T038-07 | Pendiente |
| T038-09 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-038-01` | T038-08 | Pendiente |
| T038-10 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-038-01` | T038-09 | Pendiente |
| T038-11 | Registrar evidencias del Constitution Check | Docs | `FR-038-01` | T038-10 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
