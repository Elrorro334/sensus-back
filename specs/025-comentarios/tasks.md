# Tareas 025 — Comentarios del feed

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
| T025-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-025-01` | — | Pendiente |
| T025-02 | Implementar: Entidad Comment | Domain | `FR-025-01` | T025-01 | Pendiente |
| T025-03 | Implementar: Comandos y consultas paginadas | Application | `FR-025-01` | T025-02 | Pendiente |
| T025-04 | Implementar: Persistencia con índice por postId y fecha | Infrastructure | `FR-025-01` | T025-03 | Pendiente |
| T025-05 | Implementar: Endpoints de comentarios | Api | `FR-025-01` | T025-04 | Pendiente |
| T025-06 | Modelar Comment | Domain | `FR-025-01` | T025-05 | Pendiente |
| T025-07 | Crear índice compuesto postId + createdAt | Infrastructure | `FR-025-03` | T025-06 | Pendiente |
| T025-08 | Probar el 404 en publicaciones no visibles | Api | `FR-025-04` | T025-07 | Pendiente |
| T025-09 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-025-01` | T025-08 | Pendiente |
| T025-10 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-025-01` | T025-09 | Pendiente |
| T025-11 | Registrar evidencias del Constitution Check | Docs | `FR-025-01` | T025-10 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
