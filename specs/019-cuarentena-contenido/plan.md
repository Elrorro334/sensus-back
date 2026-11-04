# Plan 019 — Cuarentena de contenido

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Retener el contenido no aprobado sin publicarlo y con una política de retención explícita.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Estado del contenido y transiciones permitidas |
| Application | Caso de uso de cuarentena y liberación |
| Infrastructure | Almacenamiento privado y purga programada |

## 3. Modelo de datos e índices

- contenido con estado y fecha de expiración de cuarentena

## 4. Seguridad

- Bucket privado; URLs firmadas solo tras aprobación

## 5. Observabilidad y rendimiento

- Métricas de contenido en cuarentena y purgado

## 6. Costo AWS

S3 dentro del Free Tier; vigilar almacenamiento acumulado.

## 7. Estrategia de pruebas

- Unitarias de transiciones de estado
- Integración de acceso denegado

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-019-01` y 3 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Acumulación de contenido sin purga efectiva

## 10. Orden de implementación

1. `T019-01` Implementar: Estado del contenido y transiciones permitidas
2. `T019-02` Implementar: Caso de uso de cuarentena y liberación
3. `T019-03` Implementar: Almacenamiento privado y purga programada
4. `T019-04` Modelar estados y transiciones
5. `T019-05` Implementar purga por expiración
6. `T019-06` Implementar las pruebas de integración del flujo completo
7. `T019-07` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
8. `T019-08` Registrar evidencias del Constitution Check
