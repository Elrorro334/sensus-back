# Tareas 002 — Inicio de sesión

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Plan | [`plan.md`](plan.md) |
| Checklist | [`checklist.md`](checklist.md) |
| Estado | Remediación pendiente |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## Tareas

| ID | Tarea | Capa | Requisito | Depende | Estado |
|---|---|---|---|---|---|
| T002-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-002-01` | — | Pendiente |
| T002-02 | Sacar el secreto JWT del repositorio y rotarlo | Seguridad | `FR-002-02` | T002-01 | Pendiente |
| T002-03 | Prueba de integración de login y de mensaje genérico | Api | `FR-002-03` | T002-02 | Pendiente |
| T002-04 | Cubrir el caso de cuenta suspendida | Application | `FR-002-05` | T002-03 | Pendiente |
| T002-05 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-002-01` | T002-04 | Pendiente |
| T002-06 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-002-01` | T002-05 | Pendiente |
| T002-07 | Registrar evidencias del Constitution Check | Docs | `FR-002-01` | T002-06 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
