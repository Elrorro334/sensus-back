# Tareas 033 — Apartado de oferta con código QR

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
| T033-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-033-01` | — | Pendiente |
| T033-02 | Implementar: Entidad OfferClaim con estados Active, Redeemed, Expired y reglas de canje | Domain | `FR-033-01` | T033-01 | Pendiente |
| T033-03 | Implementar: ClaimOfferCommand, RedeemClaimCommand y ExpireClaimsJob | Application | `FR-033-01` | T033-02 | Pendiente |
| T033-04 | Implementar: Actualización atómica con findOneAndUpdate, firma HMAC y generación de QR | Infrastructure | `FR-033-01` | T033-03 | Pendiente |
| T033-05 | Implementar: Endpoints de apartado y canje | Api | `FR-033-01` | T033-04 | Pendiente |
| T033-06 | Modelar OfferClaim y sus transiciones | Domain | `FR-033-04` | T033-05 | Pendiente |
| T033-07 | Garantizar la reserva atómica de stock | Infrastructure | `FR-033-01` | T033-06 | Pendiente |
| T033-08 | Implementar el job de expiración y liberación | Application | `FR-033-05` | T033-07 | Pendiente |
| T033-09 | Firmar y verificar el código del QR con HMAC | Seguridad | `FR-033-06` | T033-08 | Pendiente |
| T033-10 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-033-01` | T033-09 | Pendiente |
| T033-11 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-033-01` | T033-10 | Pendiente |
| T033-12 | Registrar evidencias del Constitution Check | Docs | `FR-033-01` | T033-11 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
