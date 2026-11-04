# Plan 039 — Me gusta en publicaciones

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que un usuario marque o desmarque su me gusta en una publicación visible, con semántica de alternancia y conteo consistente.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Regla de me gusta único por usuario |
| Application | ToggleLikeCommand y cálculo del conteo |
| Infrastructure | Índice único postId + userId y contador atómico |
| Api | Endpoint POST /api/v1/feed/posts/{id}/like |

## 3. Modelo de datos e índices

- post_likes con índice único postId + userId; posts.likeCount

## 4. Seguridad

- El usuario proviene del token
- Sin exponer quién reaccionó

## 5. Observabilidad y rendimiento

- Métricas de me gusta y de conflictos de concurrencia

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de alternancia
- Integración de concurrencia y 404

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-039-01` y 5 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Un contador desincronizado rompe la confianza en la métrica
- El POST con semántica de alternancia no es idempotente por diseño

## 10. Orden de implementación

1. `T039-01` Actualizar y validar el contrato OpenAPI del spec
2. `T039-02` Implementar: Regla de me gusta único por usuario
3. `T039-03` Implementar: ToggleLikeCommand y cálculo del conteo
4. `T039-04` Implementar: Índice único postId + userId y contador atómico
5. `T039-05` Implementar: Endpoint POST /api/v1/feed/posts/{id}/like
6. `T039-06` Modelar la unicidad del me gusta
7. `T039-07` Implementar el índice único y el contador atómico
8. `T039-08` Probar la alternancia y el 404
9. `T039-09` Implementar las pruebas de integración del flujo completo
10. `T039-10` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
11. `T039-11` Registrar evidencias del Constitution Check
