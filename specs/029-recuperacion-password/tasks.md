# Tareas 029 — Recuperación de contraseña

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
| T029-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-029-01` | — | Pendiente |
| T029-02 | Implementar: Entidad PasswordResetToken con reglas de vigencia y uso único | Domain | `FR-029-01` | T029-01 | Pendiente |
| T029-03 | Implementar: RequestPasswordResetCommand y ConfirmPasswordResetCommand | Application | `FR-029-01` | T029-02 | Pendiente |
| T029-04 | Implementar: Colección password_reset_tokens con índice TTL y cliente SES | Infrastructure | `FR-029-01` | T029-03 | Pendiente |
| T029-05 | Implementar: Endpoints de solicitud y confirmación | Api | `FR-029-01` | T029-04 | Pendiente |
| T029-06 | Modelar el token de recuperación y su vigencia | Domain | `FR-029-02` | T029-05 | Pendiente |
| T029-07 | Implementar el envío con SES y la plantilla de correo | Infrastructure | `FR-029-04` | T029-06 | Pendiente |
| T029-08 | Coordinar la revocación de sesiones tras el cambio | Application | `FR-029-07` | T029-07 | Pendiente |
| T029-09 | Probar que la respuesta no permite enumerar cuentas | Api | `FR-029-01` | T029-08 | Pendiente |
| T029-10 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-029-01` | T029-09 | Pendiente |
| T029-11 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-029-01` | T029-10 | Pendiente |
| T029-12 | Registrar evidencias del Constitution Check | Docs | `FR-029-01` | T029-11 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
