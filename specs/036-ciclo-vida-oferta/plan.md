# Plan 036 — Ciclo de vida de la oferta

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que el comerciante publique, pause o archive sus ofertas mediante transiciones explícitas y auditables.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Máquina de estados de Offer y transiciones válidas |
| Application | ChangeOfferStatusCommand y validador |
| Infrastructure | Actualización atómica y registro de auditoría |
| Api | Endpoint PUT /api/v1/offers/{id}/status |

## 3. Modelo de datos e índices

- offers.status con transiciones auditadas; offer_status_history append-only

## 4. Seguridad

- Solo el propietario cambia el estado
- Sin saltarse la moderación al publicar

## 5. Observabilidad y rendimiento

- Métricas de transiciones por estado
- Alerta si una oferta archivada reaparece

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de la matriz de transiciones
- Integración de archivado con apartados activos

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-036-01` y 5 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Una transición mal definida deja ofertas invisibles para siempre
- El archivado con apartados activos puede dejar usuarios sin canje

## 10. Orden de implementación

1. `T036-01` Actualizar y validar el contrato OpenAPI del spec
2. `T036-02` Implementar: Máquina de estados de Offer y transiciones válidas
3. `T036-03` Implementar: ChangeOfferStatusCommand y validador
4. `T036-04` Implementar: Actualización atómica y registro de auditoría
5. `T036-05` Implementar: Endpoint PUT /api/v1/offers/{id}/status
6. `T036-06` Implementar la máquina de estados
7. `T036-07` Validar apartados activos antes de archivar
8. `T036-08` Persistir el historial de estados
9. `T036-09` Implementar las pruebas de integración del flujo completo
10. `T036-10` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
11. `T036-11` Registrar evidencias del Constitution Check
