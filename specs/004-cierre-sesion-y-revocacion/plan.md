# Plan 004 — Cierre de sesión y revocación de token

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir cerrar sesión de forma efectiva, invalidando el token del usuario antes de su expiración natural.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Contrato ITokenRevocationStore |
| Application | LogoutCommand y handler |
| Infrastructure | Colección token_revocations con índice TTL |
| Api | Endpoint y validación de jti revocado en el pipeline JWT |

## 3. Modelo de datos e índices

- token_revocations con índice TTL en expiresAt e índice único en jti

## 4. Seguridad

- La denylist se consulta en cada autenticación
- No registrar tokens en logs
- Caché con TTL alineado a la expiración

## 5. Observabilidad y rendimiento

- Métricas de logouts y de tokens rechazados
- Alerta si la denylist crece de forma anómala

## 6. Costo AWS

Sin costo incremental: colección pequeña con TTL en MongoDB.

## 7. Estrategia de pruebas

- Unitarias del handler
- Integración: logout, uso posterior del token y doble logout

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-004-01` y 3 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Consultar la denylist en cada petición añade latencia; mitigar con caché

## 10. Orden de implementación

1. `T004-01` Actualizar y validar el contrato OpenAPI del spec
2. `T004-02` Implementar: Contrato ITokenRevocationStore
3. `T004-03` Implementar: LogoutCommand y handler
4. `T004-04` Implementar: Colección token_revocations con índice TTL
5. `T004-05` Implementar: Endpoint y validación de jti revocado en el pipeline JWT
6. `T004-06` Crear colección e índice TTL de revocaciones
7. `T004-07` Validar jti revocado en el pipeline de autenticación
8. `T004-08` Implementar las pruebas de integración del flujo completo
9. `T004-09` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
10. `T004-10` Registrar evidencias del Constitution Check
