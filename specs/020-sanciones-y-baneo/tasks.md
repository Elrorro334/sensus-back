# Tareas 020 — Sanciones y baneo de usuario

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
| T020-01 | Implementar: Entidad Sancion con reglas de vigencia | Domain | `FR-020-01` | — | Pendiente |
| T020-02 | Implementar: Casos de uso de aplicar y revertir | Application | `FR-020-01` | T020-01 | Pendiente |
| T020-03 | Implementar: Persistencia y consulta en autenticación | Infrastructure | `FR-020-01` | T020-02 | Pendiente |
| T020-04 | Implementar: Endpoints administrativos de sanción | Api | `FR-020-01` | T020-03 | Pendiente |
| T020-05 | Modelar sanción y vigencia | Domain | `FR-020-01` | T020-04 | Pendiente |
| T020-06 | Implementar reversión con auditoría | Application | `FR-020-02` | T020-05 | Pendiente |
| T020-07 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-020-01` | T020-06 | Pendiente |
| T020-08 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-020-01` | T020-07 | Pendiente |
| T020-09 | Registrar evidencias del Constitution Check | Docs | `FR-020-01` | T020-08 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
