# Tareas 044 — Ajustes de notificaciones

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
| T044-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-044-01` | — | Pendiente |
| T044-02 | Implementar: GetNotificationSettingsQuery y UpdateNotificationSettingsCommand | Application | `FR-044-01` | T044-01 | Pendiente |
| T044-03 | Implementar: Persistencia de preferencias y filtro en el despacho | Infrastructure | `FR-044-01` | T044-02 | Pendiente |
| T044-04 | Implementar: Endpoints GET y PUT /api/v1/users/settings | Api | `FR-044-01` | T044-03 | Pendiente |
| T044-05 | Implementar valores por defecto | Application | `FR-044-05` | T044-04 | Pendiente |
| T044-06 | Aplicar el filtro de preferencias en el despacho | Infrastructure | `FR-044-04` | T044-05 | Pendiente |
| T044-07 | Probar el límite de radio y la actualización | Api | `FR-044-03` | T044-06 | Pendiente |
| T044-08 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-044-01` | T044-07 | Pendiente |
| T044-09 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-044-01` | T044-08 | Pendiente |
| T044-10 | Registrar evidencias del Constitution Check | Docs | `FR-044-01` | T044-09 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
