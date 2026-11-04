# Tareas 037 — Inventario de ofertas del comerciante

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
| T037-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-037-01` | — | Pendiente |
| T037-02 | Implementar: GetMerchantOffersQuery y proyección con métricas | Application | `FR-037-01` | T037-01 | Pendiente |
| T037-03 | Implementar: Agregación por merchantId y conteo de claims/vistas | Infrastructure | `FR-037-01` | T037-02 | Pendiente |
| T037-04 | Implementar: Endpoint GET /api/v1/merchants/my-offers | Api | `FR-037-01` | T037-03 | Pendiente |
| T037-05 | Definir el DTO con métricas agregadas | Application | `FR-037-03` | T037-04 | Pendiente |
| T037-06 | Optimizar la agregación con índices | Infrastructure | `FR-037-02` | T037-05 | Pendiente |
| T037-07 | Probar el aislamiento entre comercios | Api | `FR-037-06` | T037-06 | Pendiente |
| T037-08 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-037-01` | T037-07 | Pendiente |
| T037-09 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-037-01` | T037-08 | Pendiente |
| T037-10 | Registrar evidencias del Constitution Check | Docs | `FR-037-01` | T037-09 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
