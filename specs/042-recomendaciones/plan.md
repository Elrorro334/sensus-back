# Plan 042 — Recomendaciones personalizadas

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Recomendar ofertas cercanas combinando proximidad, afinidad de categoría, vigencia y popularidad, con reglas deterministas y explicables.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Application | GetRecommendationsQuery y función de puntaje |
| Infrastructure | Agregaciones de señales y caché por usuario |
| Api | Endpoint GET /api/v1/ml/recommendations |

## 3. Modelo de datos e índices

- offers, favorites, post_likes y offer_claims agregados por usuario; caché de 5 minutos

## 4. Seguridad

- Sin perfiles sensibles
- Sin exponer qué señales contribuyeron
- Datos agregados por usuario

## 5. Observabilidad y rendimiento

- Métricas de latencia, cobertura y resultados vacíos

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de la función de puntaje
- Integración de fallback y exclusiones
- Prueba de estabilidad del orden

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-042-01` y 6 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Pocas señales producen recomendaciones triviales
- El caché puede mostrar ofertas recién pausadas

## 10. Orden de implementación

1. `T042-01` Actualizar y validar el contrato OpenAPI del spec
2. `T042-02` Implementar: GetRecommendationsQuery y función de puntaje
3. `T042-03` Implementar: Agregaciones de señales y caché por usuario
4. `T042-04` Implementar: Endpoint GET /api/v1/ml/recommendations
5. `T042-05` Implementar la función de puntaje con pesos configurables
6. `T042-06` Agregar señales de favoritos, me gusta y apartados
7. `T042-07` Implementar el fallback sin señales
8. `T042-08` Implementar las pruebas de integración del flujo completo
9. `T042-09` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
10. `T042-10` Registrar evidencias del Constitution Check
