# Tareas 023 — Entrega de contenido por CDN

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
| T023-01 | Implementar: Distribución CloudFront y política de caché en CDK | Infrastructure | `FR-023-01` | — | Pendiente |
| T023-02 | Implementar: Generación de URLs firmadas | Application | `FR-023-01` | T023-01 | Pendiente |
| T023-03 | Definir distribución y política de caché | Infrastructure | `FR-023-01` | T023-02 | Pendiente |
| T023-04 | Generar URLs firmadas para contenido privado | Application | `FR-023-02` | T023-03 | Pendiente |
| T023-05 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-023-01` | T023-04 | Pendiente |
| T023-06 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-023-01` | T023-05 | Pendiente |
| T023-07 | Registrar evidencias del Constitution Check | Docs | `FR-023-01` | T023-06 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
