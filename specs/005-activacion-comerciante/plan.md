# Plan 005 — Activación del rol comercial

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Documenta estado actual con brechas |
| Versión | 1.0.0 |
| Constitution Check | FAIL |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que un usuario con cuenta unificada active el rol comercial creando el perfil de su local con categoría, dirección y coordenadas validadas. Este plan documenta la implementación existente en la rama actual y las brechas detectadas frente a la constitución.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Entidad Merchant con invariantes de categoría, ubicación y estado |
| Application | ActivateMerchantCommand, validador y handler |
| Infrastructure | Repositorio Mongo, índices único y 2dsphere |
| Api | Endpoint POST /api/v1/merchants/activate |

## 3. Modelo de datos e índices

- merchants con índice único en userId e índice 2dsphere en location

## 4. Seguridad

- Autorización por token
- El userId proviene del token
- Sin datos de contacto internos en la respuesta

## 5. Observabilidad y rendimiento

- Métricas de activaciones y conflictos
- Log sin PII del local

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de handler y validador
- Integración de alta, duplicado y coordenadas inválidas

## 8. Constitution Check

Evaluación del estado actual. Las brechas marcadas como FAIL se convierten en tareas de remediación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-005-01` y 6 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | FAIL | no existen pruebas de integración |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PARCIAL | sin correlation-id |

> **Brechas abiertas:** las decisiones `FAIL` no bloquean la documentación de un retro-spec, pero MUST resolverse antes de implementar cualquier funcionalidad que dependa de ella.

## 9. Riesgos de implementación

- Coordenadas enviadas por el cliente pueden ser imprecisas
- El índice geoespacial no está probado contra MongoDB real

## 10. Orden de implementación

1. `T005-01` Actualizar y validar el contrato OpenAPI del spec
2. `T005-02` Documentar invariantes de Merchant y ubicación
3. `T005-03` Retirar el endpoint legacy /api/v1/comercio/registro
4. `T005-04` Probar el conflicto de comercio duplicado
5. `T005-05` Verificar el índice 2dsphere con explain
6. `T005-06` Implementar las pruebas de integración del flujo completo
7. `T005-07` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
8. `T005-08` Registrar evidencias del Constitution Check
