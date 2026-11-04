# Tareas 030 — Eliminación de cuenta y datos

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
| T030-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-030-01` | — | Pendiente |
| T030-02 | Implementar: Estado Deleted y reglas de retención | Domain | `FR-030-01` | T030-01 | Pendiente |
| T030-03 | Implementar: DeleteAccountCommand y caso de uso de purga | Application | `FR-030-01` | T030-02 | Pendiente |
| T030-04 | Implementar: Purga programada y anonimización en MongoDB | Infrastructure | `FR-030-01` | T030-03 | Pendiente |
| T030-05 | Implementar: Endpoint DELETE /api/v1/users/account | Api | `FR-030-01` | T030-04 | Pendiente |
| T030-06 | Modelar el estado Deleted y la retención | Domain | `FR-030-02` | T030-05 | Pendiente |
| T030-07 | Implementar la purga programada con evidencia | Infrastructure | `FR-030-04` | T030-06 | Pendiente |
| T030-08 | Anonimizar contenido público en lugar de borrarlo | Application | `FR-030-05` | T030-07 | Pendiente |
| T030-09 | Resolver apartados activos antes de desactivar ofertas | Application | `FR-030-06` | T030-08 | Pendiente |
| T030-10 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-030-01` | T030-09 | Pendiente |
| T030-11 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-030-01` | T030-10 | Pendiente |
| T030-12 | Registrar evidencias del Constitution Check | Docs | `FR-030-01` | T030-11 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
