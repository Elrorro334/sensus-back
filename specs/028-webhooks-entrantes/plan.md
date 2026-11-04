# Plan 028 — Webhooks entrantes (backlog)

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Recibir eventos externos de forma segura, idempotente y sin bloquear al emisor. Capacidad fuera del catálogo vigente de 29 endpoints; se conserva como backlog.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Contrato de evento y estado |
| Application | Caso de uso idempotente |
| Infrastructure | Persistencia de eventos y verificación de firma |
| Api | Endpoint de recepción |

## 3. Modelo de datos e índices

- webhook_events con índice único en eventId y estado de procesamiento

## 4. Seguridad

- Firma HMAC obligatoria
- Rechazo de eventos sin firma válida
- Sin confiar en el cuerpo sin verificar

## 5. Observabilidad y rendimiento

- Métricas de eventos recibidos, duplicados y fallidos

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de idempotencia
- Integración de firma inválida y duplicado

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-028-01` y 4 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- No hay proveedor identificado que justifique la capacidad
- Mantenerla activa consume presupuesto de especificación

## 10. Orden de implementación

1. `T028-01` Confirmar si la capacidad entra en el alcance del curso
2. `T028-02` Implementar idempotencia por eventId
3. `T028-03` Reevaluar el backlog cuando exista un proveedor o una decisión de producto
