# Plan 016 — Health checks

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Exponer estado de vida y de preparación sin revelar detalles internos.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Api | Endpoints de health checks |
| Infraestructura | Comprobación de conectividad con MongoDB |

## 3. Modelo de datos e índices

- Sin cambios de datos

## 4. Seguridad

- Respuestas mínimas; sin trazas

## 5. Observabilidad y rendimiento

- Métricas de disponibilidad

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Integración de 200 y 503

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-016-01` y 2 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Un readiness agresivo puede provocar reinicios innecesarios

## 10. Orden de implementación

1. `T016-01` Actualizar y validar el contrato OpenAPI del spec
2. `T016-02` Implementar: Endpoints de health checks
3. `T016-03` Implementar: Comprobación de conectividad con MongoDB
4. `T016-04` Separar liveness y readiness
5. `T016-05` Añadir comprobación de MongoDB
6. `T016-06` Implementar las pruebas de integración del flujo completo
7. `T016-07` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
8. `T016-08` Registrar evidencias del Constitution Check
