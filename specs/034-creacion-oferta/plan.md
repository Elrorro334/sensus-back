# Plan 034 — Creación de oferta

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que un comerciante publique una oferta con precio, descuento, stock, vigencia e imágenes, sujeta a moderación previa.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Entidad Offer con invariantes de precio, vigencia y stock |
| Application | CreateOfferCommand, validador y orquestación de moderación |
| Infrastructure | Persistencia, referencias a media y encolado de moderación |
| Api | Endpoint POST /api/v1/offers |

## 3. Modelo de datos e índices

- offers con índice por merchantId + status y por status + endsAt

## 4. Seguridad

- El comercio proviene del token
- Moderación previa obligatoria
- Sin publicar imágenes antes del veredicto

## 5. Observabilidad y rendimiento

- Métricas de ofertas creadas, rechazadas y en revisión

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de validación de precio y vigencia
- Integración de Draft, Published y pendiente de moderación

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-034-01` y 6 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Publicar antes del veredicto si se invierte el orden de las operaciones
- Un límite de ofertas mal calibrado bloquea comercios legítimos

## 10. Orden de implementación

1. `T034-01` Actualizar y validar el contrato OpenAPI del spec
2. `T034-02` Implementar: Entidad Offer con invariantes de precio, vigencia y stock
3. `T034-03` Implementar: CreateOfferCommand, validador y orquestación de moderación
4. `T034-04` Implementar: Persistencia, referencias a media y encolado de moderación
5. `T034-05` Implementar: Endpoint POST /api/v1/offers
6. `T034-06` Modelar Offer y sus invariantes
7. `T034-07` Encadenar moderación y publicación
8. `T034-08` Implementar el límite de ofertas activas
9. `T034-09` Probar el 202 de moderación pendiente
10. `T034-10` Implementar las pruebas de integración del flujo completo
11. `T034-11` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
12. `T034-12` Registrar evidencias del Constitution Check
