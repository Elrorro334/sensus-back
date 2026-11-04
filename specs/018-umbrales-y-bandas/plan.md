# Plan 018 — Umbrales y bandas de decisión

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Decidir automáticamente entre publicar, revisar o rechazar según puntajes configurables.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Política de bandas como regla de negocio |
| Application | Evaluación y decisión |
| Infraestructura | Configuración de umbrales por entorno |

## 3. Modelo de datos e índices

- moderation_policies con versión y umbrales

## 4. Seguridad

- Cambios de umbral auditados

## 5. Observabilidad y rendimiento

- Distribución de decisiones por banda

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de cada banda y de los límites exactos

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-018-01` y 3 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Umbrales mal calibrados generan falsos positivos masivos

## 10. Orden de implementación

1. `T018-01` Implementar: Política de bandas como regla de negocio
2. `T018-02` Implementar: Evaluación y decisión
3. `T018-03` Implementar: Configuración de umbrales por entorno
4. `T018-04` Modelar la política de bandas
5. `T018-05` Externalizar umbrales por entorno
6. `T018-06` Implementar las pruebas de integración del flujo completo
7. `T018-07` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
8. `T018-08` Registrar evidencias del Constitution Check
