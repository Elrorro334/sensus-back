# Tareas 031 — Edición del perfil de comercio

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
| T031-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-031-01` | — | Pendiente |
| T031-02 | Implementar: Invariantes de Merchant y recálculo de ubicación | Domain | `FR-031-01` | T031-01 | Pendiente |
| T031-03 | Implementar: UpdateMerchantProfileCommand y validador | Application | `FR-031-01` | T031-02 | Pendiente |
| T031-04 | Implementar: Actualización atómica y recálculo de geohash | Infrastructure | `FR-031-01` | T031-03 | Pendiente |
| T031-05 | Implementar: Endpoint PUT /api/v1/merchants/profile | Api | `FR-031-01` | T031-04 | Pendiente |
| T031-06 | Implementar el recálculo de location y geohash | Domain | `FR-031-02` | T031-05 | Pendiente |
| T031-07 | Validar categoría y coordenadas | Application | `FR-031-03` | T031-06 | Pendiente |
| T031-08 | Probar el rechazo de edición sobre comercios ajenos | Api | `FR-031-04` | T031-07 | Pendiente |
| T031-09 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-031-01` | T031-08 | Pendiente |
| T031-10 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-031-01` | T031-09 | Pendiente |
| T031-11 | Registrar evidencias del Constitution Check | Docs | `FR-031-01` | T031-10 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
