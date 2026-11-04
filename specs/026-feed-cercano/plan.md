# Plan 026 — Feed geolocalizado

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Entregar un feed paginado de publicaciones visibles cercanas a la ubicación del usuario, ordenado cronológicamente.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Application | GetNearbyFeedQuery con proyección |
| Infrastructure | Índice geoespacial y caché de corta duración |
| Api | Endpoint GET /api/v1/feed/nearby |

## 3. Modelo de datos e índices

- posts con location 2dsphere heredada del comercio o enviada por el autor; índice status + createdAt

## 4. Seguridad

- Solo contenido publicado
- Sin datos privados del autor
- Radio acotado

## 5. Observabilidad y rendimiento

- Métricas de aciertos de caché y latencia

## 6. Costo AWS

Caché en memoria del proceso; sin costo adicional.

## 7. Estrategia de pruebas

- Integración de paginación y filtrado
- Prueba de caché y de radio inválido

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-026-01` y 5 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- El caché en memoria no se comparte entre instancias de Lambda
- La ubicación heredada puede quedar desactualizada si el comercio se muda

## 10. Orden de implementación

1. `T026-01` Actualizar y validar el contrato OpenAPI del spec
2. `T026-02` Implementar: GetNearbyFeedQuery con proyección
3. `T026-03` Implementar: Índice geoespacial y caché de corta duración
4. `T026-04` Implementar: Endpoint GET /api/v1/feed/nearby
5. `T026-05` Crear índice geoespacial y de estado
6. `T026-06` Implementar caché con TTL e invalidación
7. `T026-07` Validar radio y paginación
8. `T026-08` Implementar las pruebas de integración del flujo completo
9. `T026-09` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
10. `T026-10` Registrar evidencias del Constitution Check
