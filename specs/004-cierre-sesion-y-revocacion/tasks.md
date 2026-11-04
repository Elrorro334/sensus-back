# Tareas 004 — Cierre de sesión y revocación de token

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
| T004-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-004-01` | — | Pendiente |
| T004-02 | Implementar: Contrato ITokenRevocationStore | Domain | `FR-004-01` | T004-01 | Pendiente |
| T004-03 | Implementar: LogoutCommand y handler | Application | `FR-004-01` | T004-02 | Pendiente |
| T004-04 | Implementar: Colección token_revocations con índice TTL | Infrastructure | `FR-004-01` | T004-03 | Pendiente |
| T004-05 | Implementar: Endpoint y validación de jti revocado en el pipeline JWT | Api | `FR-004-01` | T004-04 | Pendiente |
| T004-06 | Crear colección e índice TTL de revocaciones | Infrastructure | `FR-004-02` | T004-05 | Pendiente |
| T004-07 | Validar jti revocado en el pipeline de autenticación | Api | `FR-004-03` | T004-06 | Pendiente |
| T004-08 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-004-01` | T004-07 | Pendiente |
| T004-09 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-004-01` | T004-08 | Pendiente |
| T004-10 | Registrar evidencias del Constitution Check | Docs | `FR-004-01` | T004-09 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
