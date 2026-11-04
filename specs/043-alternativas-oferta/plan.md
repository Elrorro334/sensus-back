# Plan 043 — Alternativas a una oferta

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Sugerir ofertas similares a una dada —misma categoría, cercanas y con precio comparable— para evitar que el usuario abandone la búsqueda.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Application | GetAlternativesQuery y proyección |
| Infrastructure | Consulta por categoría + 2dsphere + rango de precio |
| Api | Endpoint GET /api/v1/ml/alternatives |

## 3. Modelo de datos e índices

- offers con índices category + status + price y location 2dsphere

## 4. Seguridad

- Solo ofertas visibles
- Sin exponer datos internos del comercio

## 5. Observabilidad y rendimiento

- Métricas de alternativas devueltas y consultas vacías

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Integración de filtros, límites y 404
- Prueba de exclusión de la oferta original

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-043-01` y 6 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Un radio corto deja la respuesta vacía en zonas con poca oferta

## 10. Orden de implementación

1. `T043-01` Actualizar y validar el contrato OpenAPI del spec
2. `T043-02` Implementar: GetAlternativesQuery y proyección
3. `T043-03` Implementar: Consulta por categoría + 2dsphere + rango de precio
4. `T043-04` Implementar: Endpoint GET /api/v1/ml/alternatives
5. `T043-05` Implementar el filtro por categoría y precio
6. `T043-06` Crear el índice category + status + price
7. `T043-07` Validar el límite y el 404
8. `T043-08` Implementar las pruebas de integración del flujo completo
9. `T043-09` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
10. `T043-10` Registrar evidencias del Constitution Check
