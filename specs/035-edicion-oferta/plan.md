# Plan 035 — Edición de oferta

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que el comerciante corrija una oferta existente sin romper apartados activos ni saltarse la moderación cuando cambia el contenido visible.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Reglas de edición y versión de la oferta |
| Application | UpdateOfferCommand y validador |
| Infrastructure | Actualización con filtro de versión y re-moderación |
| Api | Endpoint PUT /api/v1/offers/{id} |

## 3. Modelo de datos e índices

- offers con campo version; offer_claims consultados para el stock reservado

## 4. Seguridad

- Autorización por propietario
- Re-moderación obligatoria del contenido visible

## 5. Observabilidad y rendimiento

- Métricas de ediciones, conflictos y re-moderaciones

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de reglas de stock
- Integración de concurrencia y re-moderación

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-035-01` y 6 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Una edición concurrente puede sobrescribir cambios legítimos
- Reducir stock puede invalidar apartados activos

## 10. Orden de implementación

1. `T035-01` Actualizar y validar el contrato OpenAPI del spec
2. `T035-02` Implementar: Reglas de edición y versión de la oferta
3. `T035-03` Implementar: UpdateOfferCommand y validador
4. `T035-04` Implementar: Actualización con filtro de versión y re-moderación
5. `T035-05` Implementar: Endpoint PUT /api/v1/offers/{id}
6. `T035-06` Modelar la versión y las reglas de edición
7. `T035-07` Calcular el stock reservado antes de validar
8. `T035-08` Aplicar el filtro de versión en la actualización
9. `T035-09` Implementar las pruebas de integración del flujo completo
10. `T035-10` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
11. `T035-11` Registrar evidencias del Constitution Check
