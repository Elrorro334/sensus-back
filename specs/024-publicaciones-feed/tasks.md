# Tareas 024 — Publicaciones del feed

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
| T024-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-024-01` | — | Pendiente |
| T024-02 | Implementar: Entidad Post con estados y regla de autoría | Domain | `FR-024-01` | T024-01 | Pendiente |
| T024-03 | Implementar: CreatePostCommand, DeletePostCommand y orquestación de moderación | Application | `FR-024-01` | T024-02 | Pendiente |
| T024-04 | Implementar: Persistencia y referencia al archivo | Infrastructure | `FR-024-01` | T024-03 | Pendiente |
| T024-05 | Implementar: Endpoints de creación y eliminación | Api | `FR-024-01` | T024-04 | Pendiente |
| T024-06 | Modelar estados de publicación | Domain | `FR-024-03` | T024-05 | Pendiente |
| T024-07 | Encadenar moderación y publicación | Application | `FR-024-02` | T024-06 | Pendiente |
| T024-08 | Autorizar el borrado por autoría o rol | Application | `FR-024-05` | T024-07 | Pendiente |
| T024-09 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-024-01` | T024-08 | Pendiente |
| T024-10 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-024-01` | T024-09 | Pendiente |
| T024-11 | Registrar evidencias del Constitution Check | Docs | `FR-024-01` | T024-10 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
