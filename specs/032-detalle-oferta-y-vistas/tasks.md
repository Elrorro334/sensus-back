# Tareas 032 — Detalle de oferta y contador de vistas

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
| T032-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-032-01` | — | Pendiente |
| T032-02 | Implementar: GetOfferDetailQuery y RegisterOfferViewCommand | Application | `FR-032-01` | T032-01 | Pendiente |
| T032-03 | Implementar: Colección offer_views con índice único userId + offerId + dayOfYear | Infrastructure | `FR-032-01` | T032-02 | Pendiente |
| T032-04 | Implementar: Endpoints de detalle y vista | Api | `FR-032-01` | T032-03 | Pendiente |
| T032-05 | Modelar la vista y su clave de deduplicación | Domain | `FR-032-03` | T032-04 | Pendiente |
| T032-06 | Crear el índice único de vistas y su TTL | Infrastructure | `FR-032-03` | T032-05 | Pendiente |
| T032-07 | Devolver 404 uniforme para ofertas no visibles | Api | `FR-032-02` | T032-06 | Pendiente |
| T032-08 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-032-01` | T032-07 | Pendiente |
| T032-09 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-032-01` | T032-08 | Pendiente |
| T032-10 | Registrar evidencias del Constitution Check | Docs | `FR-032-01` | T032-09 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
