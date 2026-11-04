# Plan 003 — Perfil del usuario autenticado

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Documenta estado actual con brechas |
| Versión | 1.0.0 |
| Constitution Check | FAIL |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que el usuario autenticado consulte y actualice su propio perfil —nombre y foto— sin poder alterar correo ni roles. Este plan documenta la implementación existente en la rama actual y las brechas detectadas frente a la constitución.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Application | UserDto, GetProfileQuery y UpdateProfileCommand |
| Infrastructure | Repositorio de usuarios y verificación de propiedad del avatar |
| Api | Endpoints GET y PUT /api/v1/users/profile con autorización |

## 3. Modelo de datos e índices

- users; actualización por identificador del token

## 4. Seguridad

- Autorización por token
- El identificador proviene del token, nunca del cuerpo
- Sin datos sensibles en la respuesta

## 5. Observabilidad y rendimiento

- Métricas de consultas, actualizaciones y errores 401/403

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Integración de 200 con token y 401 sin token
- Unitarias del comando de actualización
- Prueba de rechazo de campos inmutables

## 8. Constitution Check

Evaluación del estado actual. Las brechas marcadas como FAIL se convierten en tareas de remediación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-003-01` y 5 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | FAIL | no existen pruebas de integración |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PARCIAL | sin correlation-id en logs |

> **Brechas abiertas:** las decisiones `FAIL` no bloquean la documentación de un retro-spec, pero MUST resolverse antes de implementar cualquier funcionalidad que dependa de ella.

## 9. Riesgos de implementación

- El DTO podría divergir del documento si se expone la entidad directamente

## 10. Orden de implementación

1. `T003-01` Actualizar y validar el contrato OpenAPI del spec
2. `T003-02` Cubrir 200 y 401 en pruebas de integración
3. `T003-03` Probar que el DTO no contiene campos sensibles
4. `T003-04` Modelar la inmutabilidad de correo y roles
5. `T003-05` Implementar las pruebas de integración del flujo completo
6. `T003-06` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
7. `T003-07` Registrar evidencias del Constitution Check
