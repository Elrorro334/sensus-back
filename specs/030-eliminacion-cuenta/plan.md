# Plan 030 — Eliminación de cuenta y datos

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que el titular elimine su cuenta con confirmación explícita, aplicando una retención declarada y purgando sus datos personales en cascada.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Estado Deleted y reglas de retención |
| Application | DeleteAccountCommand y caso de uso de purga |
| Infrastructure | Purga programada y anonimización en MongoDB |
| Api | Endpoint DELETE /api/v1/users/account |

## 3. Modelo de datos e índices

- users con deletedAt; offers y posts anonimizados; moderation_audit sin PII

## 4. Seguridad

- Confirmación con contraseña
- Revocación inmediata
- Sin PII en logs
- Purga verificable

## 5. Observabilidad y rendimiento

- Métricas de eliminaciones y de purgas completadas
- Alerta si la purga no se ejecuta

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de transición a Deleted
- Integración de bloqueo y revocación
- Prueba de purga con reloj simulado

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-030-01` y 6 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Una purga incompleta deja datos personales retenidos
- Eliminar una cuenta comercial oculta ofertas con apartados activos

## 10. Orden de implementación

1. `T030-01` Actualizar y validar el contrato OpenAPI del spec
2. `T030-02` Implementar: Estado Deleted y reglas de retención
3. `T030-03` Implementar: DeleteAccountCommand y caso de uso de purga
4. `T030-04` Implementar: Purga programada y anonimización en MongoDB
5. `T030-05` Implementar: Endpoint DELETE /api/v1/users/account
6. `T030-06` Modelar el estado Deleted y la retención
7. `T030-07` Implementar la purga programada con evidencia
8. `T030-08` Anonimizar contenido público en lugar de borrarlo
9. `T030-09` Resolver apartados activos antes de desactivar ofertas
10. `T030-10` Implementar las pruebas de integración del flujo completo
11. `T030-11` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
12. `T030-12` Registrar evidencias del Constitution Check
