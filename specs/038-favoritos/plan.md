# Plan 038 — Favoritos de ofertas

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que un usuario marque o desmarque una oferta como favorita, alimentando la afinidad de categoría usada por las recomendaciones.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Regla de favorito único por usuario y oferta |
| Application | ToggleFavoriteCommand |
| Infrastructure | Índice único userId + offerId y contador atómico |
| Api | Endpoint POST /api/v1/users/favorites/{id} |

## 3. Modelo de datos e índices

- favorites con índice único userId + offerId y por offerId; offers.favoriteCount

## 4. Seguridad

- El usuario proviene del token
- Sin exponer la lista de usuarios que marcaron favorito

## 5. Observabilidad y rendimiento

- Métricas de favoritos y conflictos

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de alternancia
- Integración de concurrencia y 404

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-038-01` y 5 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Alimentar el ranking con datos escasos puede sesgar las recomendaciones

## 10. Orden de implementación

1. `T038-01` Actualizar y validar el contrato OpenAPI del spec
2. `T038-02` Implementar: Regla de favorito único por usuario y oferta
3. `T038-03` Implementar: ToggleFavoriteCommand
4. `T038-04` Implementar: Índice único userId + offerId y contador atómico
5. `T038-05` Implementar: Endpoint POST /api/v1/users/favorites/{id}
6. `T038-06` Modelar la unicidad del favorito
7. `T038-07` Implementar índice único y contador atómico
8. `T038-08` Publicar la afinidad de categoría al ranking
9. `T038-09` Implementar las pruebas de integración del flujo completo
10. `T038-10` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
11. `T038-11` Registrar evidencias del Constitution Check
