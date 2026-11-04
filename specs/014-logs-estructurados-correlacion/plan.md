# Plan 014 — Logs estructurados y correlación

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Emitir logs en JSON con identificador de correlación y sin datos personales.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Api | Middleware de correlación y logging |
| Infraestructura | Configuración de logging por entorno |

## 3. Modelo de datos e índices

- Sin cambios de datos

## 4. Seguridad

- Evita filtraciones por logs
- Cumple con minimización de datos

## 5. Observabilidad y rendimiento

- Base para métricas y alarmas de la spec 015

## 6. Costo AWS

CloudWatch Logs dentro del Free Tier; vigilar volumen de ingesta.

## 7. Estrategia de pruebas

- Verificación de campos y de ausencia de PII
- Integración de correlación

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-014-01` y 3 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | observabilidad medible |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- El volumen de logs puede superar el Free Tier

## 10. Orden de implementación

1. `T014-01` Implementar: Middleware de correlación y logging
2. `T014-02` Implementar: Configuración de logging por entorno
3. `T014-03` Implementar middleware de correlation-id
4. `T014-04` Aplicar redacción de datos sensibles
5. `T014-05` Implementar las pruebas de integración del flujo completo
6. `T014-06` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
7. `T014-07` Registrar evidencias del Constitution Check
