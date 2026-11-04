# Tareas 027 — Registro de dispositivo para push

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
| T027-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-027-01` | — | Pendiente |
| T027-02 | Implementar: Entidad Device e IPushDispatcher | Domain | `FR-027-01` | T027-01 | Pendiente |
| T027-03 | Implementar: RegisterDeviceCommand y DispatchNotificationCommand | Application | `FR-027-01` | T027-02 | Pendiente |
| T027-04 | Implementar: Adaptadores FCM y SNS, repositorio de dispositivos | Infrastructure | `FR-027-01` | T027-03 | Pendiente |
| T027-05 | Implementar: Endpoint PUT /api/v1/users/device-token | Api | `FR-027-01` | T027-04 | Pendiente |
| T027-06 | Definir IPushDispatcher y la entidad Device | Domain | `FR-027-03` | T027-05 | Pendiente |
| T027-07 | Implementar los adaptadores FCM y SNS | Infrastructure | `FR-027-04` | T027-06 | Pendiente |
| T027-08 | Resolver la reasignación de token entre usuarios | Application | `FR-027-06` | T027-07 | Pendiente |
| T027-09 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-027-01` | T027-08 | Pendiente |
| T027-10 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-027-01` | T027-09 | Pendiente |
| T027-11 | Registrar evidencias del Constitution Check | Docs | `FR-027-01` | T027-10 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
