# Tareas 040 — Reportes de contenido

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
| T040-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-040-01` | — | Pendiente |
| T040-02 | Implementar: Entidad Report con motivos y reglas de deduplicación | Domain | `FR-040-01` | T040-01 | Pendiente |
| T040-03 | Implementar: CreateReportCommand y umbral de revisión | Application | `FR-040-01` | T040-02 | Pendiente |
| T040-04 | Implementar: Persistencia e índice único reporterId + targetId + día | Infrastructure | `FR-040-01` | T040-03 | Pendiente |
| T040-05 | Implementar: Endpoint POST /api/v1/reports | Api | `FR-040-01` | T040-04 | Pendiente |
| T040-06 | Modelar motivos y deduplicación | Domain | `FR-040-04` | T040-05 | Pendiente |
| T040-07 | Implementar el umbral de revisión | Application | `FR-040-05` | T040-06 | Pendiente |
| T040-08 | Bloquear el persistido de material de explotación infantil | Seguridad | `FR-040-06` | T040-07 | Pendiente |
| T040-09 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-040-01` | T040-08 | Pendiente |
| T040-10 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-040-01` | T040-09 | Pendiente |
| T040-11 | Registrar evidencias del Constitution Check | Docs | `FR-040-01` | T040-10 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
