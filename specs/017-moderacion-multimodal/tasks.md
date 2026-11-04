# Tareas 017 — Moderación multimodal de contenido

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
| T017-01 | Implementar: Contrato IModerationService y modelo de veredicto | Domain | `FR-017-01` | — | Pendiente |
| T017-02 | Implementar: Caso de uso de moderación previa a la publicación | Application | `FR-017-01` | T017-01 | Pendiente |
| T017-03 | Implementar: Cliente de OpenAI con timeout, reintentos y mapeo de respuesta | Infrastructure | `FR-017-01` | T017-02 | Pendiente |
| T017-04 | Implementar: Integración en los flujos de oferta, publicación y comentario | Api | `FR-017-01` | T017-03 | Pendiente |
| T017-05 | Definir IModerationService y el modelo de veredicto | Domain | `FR-017-01` | T017-04 | Pendiente |
| T017-06 | Implementar el cliente con timeout y fail-closed | Infrastructure | `FR-017-04` | T017-05 | Pendiente |
| T017-07 | Registrar la decisión completa para auditoría | Application | `FR-017-05` | T017-06 | Pendiente |
| T017-08 | Encadenar la moderación a la creación de ofertas | Application | `FR-017-06` | T017-07 | Pendiente |
| T017-09 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-017-01` | T017-08 | Pendiente |
| T017-10 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-017-01` | T017-09 | Pendiente |
| T017-11 | Registrar evidencias del Constitution Check | Docs | `FR-017-01` | T017-10 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
