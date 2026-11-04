# Plan 037 — Inventario de ofertas del comerciante

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que el comerciante consulte el historial de sus ofertas con métricas de apartados, canjes y vistas, filtrable por estado y paginado.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Application | GetMerchantOffersQuery y proyección con métricas |
| Infrastructure | Agregación por merchantId y conteo de claims/vistas |
| Api | Endpoint GET /api/v1/merchants/my-offers |

## 3. Modelo de datos e índices

- offers con índice merchantId + createdAt; offer_claims y offer_views para los conteos

## 4. Seguridad

- El comercio proviene del token
- Sin datos de otros comercios

## 5. Observabilidad y rendimiento

- Métricas de latencia de la agregación
- Log del filtro solicitado

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Integración de filtros y paginación
- Prueba de aislamiento entre comercios

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-037-01` y 5 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- La agregación de métricas puede degradarse con muchas ofertas
- El total puede desviarse si se cuenta en dos consultas

## 10. Orden de implementación

1. `T037-01` Actualizar y validar el contrato OpenAPI del spec
2. `T037-02` Implementar: GetMerchantOffersQuery y proyección con métricas
3. `T037-03` Implementar: Agregación por merchantId y conteo de claims/vistas
4. `T037-04` Implementar: Endpoint GET /api/v1/merchants/my-offers
5. `T037-05` Definir el DTO con métricas agregadas
6. `T037-06` Optimizar la agregación con índices
7. `T037-07` Probar el aislamiento entre comercios
8. `T037-08` Implementar las pruebas de integración del flujo completo
9. `T037-09` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
10. `T037-10` Registrar evidencias del Constitution Check
