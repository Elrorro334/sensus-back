# Tareas 004 — Cierre de sesión y revocación de token

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Plan | [`plan.md`](plan.md) |
| Checklist | [`checklist.md`](checklist.md) |
| Estado | Completado |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## Tareas

| ID | Tarea | Capa | Requisito | Depende | Estado |
|---|---|---|---|---|---|
| T004-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-004-01` | — | Completado |
| T004-02 | Diseñar estrategia de revocación con rotación de SecurityStamp | Domain | `FR-004-01` | T004-01 | Completado |
| T004-03 | Implementar: LogoutCommand y LogoutCommandHandler | Application | `FR-004-01` | T004-02 | Completado |
| T004-04 | Validar SecurityStamp revocado en OnTokenValidated del pipeline JWT | Api | `FR-004-03` | T004-03 | Completado |
| T004-05 | Exponer endpoint POST /api/v1/auth/logout idempotente | Api | `FR-004-01` | T004-04 | Completado |
| T004-06 | Pruebas unitarias de LogoutCommandHandler (aislamiento y errores) | Pruebas | `FR-004-01` | T004-05 | Completado |
| T004-07 | Pruebas de integración del flujo de logout e invalidación posterior | Pruebas | `FR-004-02` | T004-06 | Completado |
| T004-08 | Pruebas de idempotencia de logout repetido (204 continuo) | Pruebas | `FR-004-04` | T004-07 | Completado |
| T004-09 | Actualizar checklist y contratos OpenAPI | Docs | `FR-004-01` | T004-08 | Completado |
| T004-10 | Registrar evidencias del Constitution Check | Docs | `FR-004-01` | T004-09 | Completado |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md`.
- Verificado con 100 pruebas unitarias y 34 pruebas de integración en verde.
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
