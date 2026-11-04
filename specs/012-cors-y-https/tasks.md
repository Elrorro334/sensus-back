# Tareas 012 — CORS y HTTPS por entorno

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
| T012-01 | Implementar: Configuración de CORS y HTTPS por entorno | Api | `FR-012-01` | — | Pendiente |
| T012-02 | Implementar: Variables de entorno por despliegue | Infraestructura | `FR-012-01` | T012-01 | Pendiente |
| T012-03 | Mover los orígenes permitidos a configuración | Api | `FR-012-01` | T012-02 | Pendiente |
| T012-04 | Condicionar HTTPS y HSTS al entorno | Api | `FR-012-03` | T012-03 | Pendiente |
| T012-05 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-012-01` | T012-04 | Pendiente |
| T012-06 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-012-01` | T012-05 | Pendiente |
| T012-07 | Registrar evidencias del Constitution Check | Docs | `FR-012-01` | T012-06 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
