# Tareas 010 — Paginación estándar

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
| T010-01 | Implementar: Modelo PagedResult y validadores de paginación | Application | `FR-010-01` | — | Pendiente |
| T010-02 | Implementar: Binding de parámetros y respuesta uniforme | Api | `FR-010-01` | T010-01 | Pendiente |
| T010-03 | Implementar y probar PagedResult | Application | `FR-010-01` | T010-02 | Pendiente |
| T010-04 | Aplicar el contrato en los endpoints existentes de colección | Api | `FR-010-04` | T010-03 | Pendiente |
| T010-05 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-010-01` | T010-04 | Pendiente |
| T010-06 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-010-01` | T010-05 | Pendiente |
| T010-07 | Registrar evidencias del Constitution Check | Docs | `FR-010-01` | T010-06 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
