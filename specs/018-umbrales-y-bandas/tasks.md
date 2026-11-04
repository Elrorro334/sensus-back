# Tareas 018 — Umbrales y bandas de decisión

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
| T018-01 | Implementar: Política de bandas como regla de negocio | Domain | `FR-018-01` | — | Pendiente |
| T018-02 | Implementar: Evaluación y decisión | Application | `FR-018-01` | T018-01 | Pendiente |
| T018-03 | Implementar: Configuración de umbrales por entorno | Infraestructura | `FR-018-01` | T018-02 | Pendiente |
| T018-04 | Modelar la política de bandas | Domain | `FR-018-01` | T018-03 | Pendiente |
| T018-05 | Externalizar umbrales por entorno | Infraestructura | `FR-018-02` | T018-04 | Pendiente |
| T018-06 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-018-01` | T018-05 | Pendiente |
| T018-07 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-018-01` | T018-06 | Pendiente |
| T018-08 | Registrar evidencias del Constitution Check | Docs | `FR-018-01` | T018-07 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
