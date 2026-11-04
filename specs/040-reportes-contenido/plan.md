# Plan 040 — Reportes de contenido

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que un usuario reporte ofertas, publicaciones o comentarios, activando la revisión del contenido sin persistir material de explotación infantil.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Entidad Report con motivos y reglas de deduplicación |
| Application | CreateReportCommand y umbral de revisión |
| Infrastructure | Persistencia e índice único reporterId + targetId + día |
| Api | Endpoint POST /api/v1/reports |

## 3. Modelo de datos e índices

- reports con índice único reporterId + targetId + dayKey; contenido marcado para revisión

## 4. Seguridad

- Identidad del reportante protegida
- Sin persistir material ilegal
- Límite de tasa por usuario

## 5. Observabilidad y rendimiento

- Métricas de reportes por motivo y de contenido marcado

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de deduplicación y umbral
- Integración de 201, 409 y rechazo legal

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-040-01` y 7 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Reportes coordinados pueden ocultar contenido legítimo
- Un reporte legal mal clasificado podría persistirse por error

## 10. Orden de implementación

1. `T040-01` Actualizar y validar el contrato OpenAPI del spec
2. `T040-02` Implementar: Entidad Report con motivos y reglas de deduplicación
3. `T040-03` Implementar: CreateReportCommand y umbral de revisión
4. `T040-04` Implementar: Persistencia e índice único reporterId + targetId + día
5. `T040-05` Implementar: Endpoint POST /api/v1/reports
6. `T040-06` Modelar motivos y deduplicación
7. `T040-07` Implementar el umbral de revisión
8. `T040-08` Bloquear el persistido de material de explotación infantil
9. `T040-09` Implementar las pruebas de integración del flujo completo
10. `T040-10` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
11. `T040-11` Registrar evidencias del Constitution Check
