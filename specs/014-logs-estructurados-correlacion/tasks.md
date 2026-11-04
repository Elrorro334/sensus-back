# Tareas 014 — Logs estructurados y correlación

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
| T014-01 | Implementar: Middleware de correlación y logging | Api | `FR-014-01` | — | Pendiente |
| T014-02 | Implementar: Configuración de logging por entorno | Infraestructura | `FR-014-01` | T014-01 | Pendiente |
| T014-03 | Implementar middleware de correlation-id | Api | `FR-014-02` | T014-02 | Pendiente |
| T014-04 | Aplicar redacción de datos sensibles | Infraestructura | `FR-014-03` | T014-03 | Pendiente |
| T014-05 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-014-01` | T014-04 | Pendiente |
| T014-06 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-014-01` | T014-05 | Pendiente |
| T014-07 | Registrar evidencias del Constitution Check | Docs | `FR-014-01` | T014-06 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
