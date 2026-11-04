# Tareas 001 — Registro de cuenta unificada

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
| T001-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-001-01` | — | Pendiente |
| T001-02 | Documentar invariantes de User y sus pruebas unitarias | Domain | `FR-001-01` | T001-01 | Pendiente |
| T001-03 | Cubrir RegisterUserCommandHandler con caso feliz y duplicado | Application | `FR-001-02` | T001-02 | Pendiente |
| T001-04 | Verificar el índice único de correo contra MongoDB real | Infrastructure | `FR-001-02` | T001-03 | Pendiente |
| T001-05 | Mover el secreto JWT a user-secrets y retirarlo de appsettings.json | Seguridad | `FR-001-07` | T001-04 | Pendiente |
| T001-06 | Añadir prueba de integración del endpoint con WebApplicationFactory | Api | `FR-001-07` | T001-05 | Pendiente |
| T001-07 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-001-01` | T001-06 | Pendiente |
| T001-08 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-001-01` | T001-07 | Pendiente |
| T001-09 | Registrar evidencias del Constitution Check | Docs | `FR-001-01` | T001-08 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
