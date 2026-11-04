# Plan 010 — Paginación estándar

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Definir un contrato único de paginación para todos los endpoints que devuelven colecciones.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Application | Modelo PagedResult y validadores de paginación |
| Api | Binding de parámetros y respuesta uniforme |

## 3. Modelo de datos e índices

- Toda consulta paginada debe apoyarse en un índice declarado

## 4. Seguridad

- Evitar que pageSize ilimitado permita extracción masiva

## 5. Observabilidad y rendimiento

- Métricas de pageSize solicitado y tiempo de conteo

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de cálculo de páginas
- Integración de límites y página vacía

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-010-01` y 3 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | implementa la regla de paginación obligatoria |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Contar el total puede ser costoso en colecciones grandes

## 10. Orden de implementación

1. `T010-01` Implementar: Modelo PagedResult y validadores de paginación
2. `T010-02` Implementar: Binding de parámetros y respuesta uniforme
3. `T010-03` Implementar y probar PagedResult
4. `T010-04` Aplicar el contrato en los endpoints existentes de colección
5. `T010-05` Implementar las pruebas de integración del flujo completo
6. `T010-06` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
7. `T010-07` Registrar evidencias del Constitution Check
