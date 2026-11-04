# Tareas 034 — Creación de oferta

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
| T034-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-034-01` | — | Pendiente |
| T034-02 | Implementar: Entidad Offer con invariantes de precio, vigencia y stock | Domain | `FR-034-01` | T034-01 | Pendiente |
| T034-03 | Implementar: CreateOfferCommand, validador y orquestación de moderación | Application | `FR-034-01` | T034-02 | Pendiente |
| T034-04 | Implementar: Persistencia, referencias a media y encolado de moderación | Infrastructure | `FR-034-01` | T034-03 | Pendiente |
| T034-05 | Implementar: Endpoint POST /api/v1/offers | Api | `FR-034-01` | T034-04 | Pendiente |
| T034-06 | Modelar Offer y sus invariantes | Domain | `FR-034-02` | T034-05 | Pendiente |
| T034-07 | Encadenar moderación y publicación | Application | `FR-034-03` | T034-06 | Pendiente |
| T034-08 | Implementar el límite de ofertas activas | Infrastructure | `FR-034-05` | T034-07 | Pendiente |
| T034-09 | Probar el 202 de moderación pendiente | Api | `FR-034-07` | T034-08 | Pendiente |
| T034-10 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-034-01` | T034-09 | Pendiente |
| T034-11 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-034-01` | T034-10 | Pendiente |
| T034-12 | Registrar evidencias del Constitution Check | Docs | `FR-034-01` | T034-11 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
