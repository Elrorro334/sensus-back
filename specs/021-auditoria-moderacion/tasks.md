# Tareas 021 — Auditoría de moderación

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
| T021-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-021-01` | — | Pendiente |
| T021-02 | Implementar: Modelo de registro de auditoría | Domain | `FR-021-01` | T021-01 | Pendiente |
| T021-03 | Implementar: Consulta paginada | Application | `FR-021-01` | T021-02 | Pendiente |
| T021-04 | Implementar: Colección append-only | Infrastructure | `FR-021-01` | T021-03 | Pendiente |
| T021-05 | Implementar: Endpoint administrativo | Api | `FR-021-01` | T021-04 | Pendiente |
| T021-06 | Crear colección append-only con índices | Infrastructure | `FR-021-01` | T021-05 | Pendiente |
| T021-07 | Restringir el endpoint al rol Admin | Api | `FR-021-03` | T021-06 | Pendiente |
| T021-08 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-021-01` | T021-07 | Pendiente |
| T021-09 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-021-01` | T021-08 | Pendiente |
| T021-10 | Registrar evidencias del Constitution Check | Docs | `FR-021-01` | T021-09 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
