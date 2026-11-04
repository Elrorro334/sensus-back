# Plan 007 — Ofertas cercanas

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS CON OBSERVACIONES |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que un usuario descubra ofertas activas cerca de su ubicación, con filtros por radio y categoría, resultados paginados y ordenados por distancia.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Application | GetNearbyOffersQuery, validador y proyección a DTO |
| Infrastructure | Consulta geoespacial con índice 2dsphere y filtro de estado |
| Api | Endpoint GET /api/v1/offers/nearby |

## 3. Modelo de datos e índices

- offers con location 2dsphere, status, stock, startsAt y endsAt; índice compuesto status + endsAt

## 4. Seguridad

- Solo ofertas visibles públicamente
- Sin exponer dirección exacta del comercio
- Radio y paginación acotados

## 5. Observabilidad y rendimiento

- Métricas de latencia por consulta geoespacial
- Log del radio y pageSize solicitados

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Integración de paginación y límites
- Prueba de orden por distancia
- Prueba de exclusión por estado y vigencia

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-007-01` y 6 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PARCIAL | no existe proyecto de pruebas de integración |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | paginación obligatoria aplicada |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Consultas geoespaciales sin índice degradan la latencia
- Un radio grande multiplica resultados y costo de lectura

## 10. Orden de implementación

1. `T007-01` Actualizar y validar el contrato OpenAPI del spec
2. `T007-02` Implementar: GetNearbyOffersQuery, validador y proyección a DTO
3. `T007-03` Implementar: Consulta geoespacial con índice 2dsphere y filtro de estado
4. `T007-04` Implementar: Endpoint GET /api/v1/offers/nearby
5. `T007-05` Definir el DTO de oferta cercana con distancia
6. `T007-06` Verificar el uso del índice 2dsphere con explain
7. `T007-07` Validar radio y pageSize en el borde
8. `T007-08` Implementar las pruebas de integración del flujo completo
9. `T007-09` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
10. `T007-10` Registrar evidencias del Constitution Check
