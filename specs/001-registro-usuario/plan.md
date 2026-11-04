# Plan 001 — Registro de cuenta unificada

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Documenta estado actual con brechas |
| Versión | 1.0.0 |
| Constitution Check | FAIL |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que una persona cree una cuenta única con correo, nombre completo y contraseña, obteniendo una sesión JWT con rol Consumidor; el rol comercial se activa después (spec 005). Este plan documenta la implementación existente en la rama actual y las brechas detectadas frente a la constitución.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Entidad User con invariantes de correo, contraseña y términos |
| Application | RegisterUserCommand, validador y handler |
| Infrastructure | MongoUserRepository, índice único de correo, BCrypt |
| Api | Endpoint POST /api/v1/auth/register |

## 3. Modelo de datos e índices

- users con índice único en email

## 4. Seguridad

- Hash BCrypt con salt
- Nunca devolver passwordHash
- Validar aceptación de términos
- Secreto JWT fuera del repositorio
- Límite de tasa estricto (spec 013)

## 5. Observabilidad y rendimiento

- Log de registro sin PII
- Métricas de altas y de errores 4xx

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de validador y handler
- Integración de alta, duplicado, contraseña débil y términos

## 8. Constitution Check

Evaluación del estado actual. Las brechas marcadas como FAIL se convierten en tareas de remediación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-001-01` y 6 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | FAIL | secreto JWT versionado en appsettings.json |
| V | FAIL | no existen pruebas de integración |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PARCIAL | sin correlation-id en logs |

> **Brechas abiertas:** las decisiones `FAIL` no bloquean la documentación de un retro-spec, pero MUST resolverse antes de implementar cualquier funcionalidad que dependa de ella.

## 9. Riesgos de implementación

- El secreto JWT versionado obliga a rotación
- El índice único no está probado contra MongoDB real

## 10. Orden de implementación

1. `T001-01` Actualizar y validar el contrato OpenAPI del spec
2. `T001-02` Documentar invariantes de User y sus pruebas unitarias
3. `T001-03` Cubrir RegisterUserCommandHandler con caso feliz y duplicado
4. `T001-04` Verificar el índice único de correo contra MongoDB real
5. `T001-05` Mover el secreto JWT a user-secrets y retirarlo de appsettings.json
6. `T001-06` Añadir prueba de integración del endpoint con WebApplicationFactory
7. `T001-07` Implementar las pruebas de integración del flujo completo
8. `T001-08` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
9. `T001-09` Registrar evidencias del Constitution Check
