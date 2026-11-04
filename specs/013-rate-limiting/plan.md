# Plan 013 — Control de tasa de peticiones

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Proteger la disponibilidad de la API limitando la tasa de peticiones por cliente y por endpoint sensible.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Api | Middleware de rate limiting y políticas por endpoint |
| Infraestructura | Configuración de umbrales por entorno |

## 3. Modelo de datos e índices

- Sin persistencia propia; usar el limitador en memoria del proceso

## 4. Seguridad

- Protege login y registro contra fuerza bruta

## 5. Observabilidad y rendimiento

- Métrica de peticiones rechazadas por política

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Integración de 429 y de tráfico dentro del umbral

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-013-01` y 4 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | control de abuso implementado |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- El límite en memoria no es consistente entre instancias de Lambda

## 10. Orden de implementación

1. `T013-01` Implementar: Middleware de rate limiting y políticas por endpoint
2. `T013-02` Implementar: Configuración de umbrales por entorno
3. `T013-03` Implementar políticas de límite por endpoint
4. `T013-04` Externalizar umbrales a configuración
5. `T013-05` Implementar las pruebas de integración del flujo completo
6. `T013-06` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
7. `T013-07` Registrar evidencias del Constitution Check
