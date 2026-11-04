# Plan 006 — Consulta de mi comercio

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Documenta estado actual con brechas |
| Versión | 1.0.0 |
| Constitution Check | FAIL |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que un usuario comerciante consulte los datos de su propio comercio, incluyendo estado y ubicación, sin acceder a comercios ajenos. Este plan documenta la implementación existente en la rama actual y las brechas detectadas frente a la constitución.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Application | GetMyMerchantQuery y handler |
| Infrastructure | Consulta por userId usando el índice único |
| Api | Endpoint GET /api/v1/merchants/me |

## 3. Modelo de datos e índices

- merchants; consulta por userId

## 4. Seguridad

- El identificador se toma del token, nunca del cliente

## 5. Observabilidad y rendimiento

- Métricas de consultas y 404

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Integración de 200 con comercio y 404 sin comercio

## 8. Constitution Check

Evaluación del estado actual. Las brechas marcadas como FAIL se convierten en tareas de remediación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-006-01` y 3 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | FAIL | no existen pruebas de integración |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> **Brechas abiertas:** las decisiones `FAIL` no bloquean la documentación de un retro-spec, pero MUST resolverse antes de implementar cualquier funcionalidad que dependa de ella.

## 9. Riesgos de implementación

- El endpoint legacy duplica la funcionalidad y puede divergir

## 10. Orden de implementación

1. `T006-01` Actualizar y validar el contrato OpenAPI del spec
2. `T006-02` Retirar el endpoint legacy /api/v1/comercio/mi-comercio
3. `T006-03` Probar el caso 404
4. `T006-04` Implementar las pruebas de integración del flujo completo
5. `T006-05` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
6. `T006-06` Registrar evidencias del Constitution Check
