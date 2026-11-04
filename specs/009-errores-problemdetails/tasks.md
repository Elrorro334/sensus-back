# Tareas 009 — Errores uniformes con ProblemDetails

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
| T009-01 | Implementar: GlobalExceptionHandler y middleware de correlación | Api | `FR-009-01` | — | Pendiente |
| T009-02 | Implementar: Excepciones de dominio mapeadas a códigos | Application | `FR-009-01` | T009-01 | Pendiente |
| T009-03 | Implementar: Sin dependencias de presentación | Domain | `FR-009-01` | T009-02 | Pendiente |
| T009-04 | Unificar el mapeo de Result y excepciones a ProblemDetails | Api | `FR-009-01` | T009-03 | Pendiente |
| T009-05 | Añadir correlationId a todas las respuestas de error | Api | `FR-009-02` | T009-04 | Pendiente |
| T009-06 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-009-01` | T009-05 | Pendiente |
| T009-07 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-009-01` | T009-06 | Pendiente |
| T009-08 | Registrar evidencias del Constitution Check | Docs | `FR-009-01` | T009-07 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
