# Plan 029 — Recuperación de contraseña

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que un usuario que olvidó su contraseña solicite un enlace de recuperación de un solo uso y establezca una contraseña nueva sin revelar si el correo existe.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Entidad PasswordResetToken con reglas de vigencia y uso único |
| Application | RequestPasswordResetCommand y ConfirmPasswordResetCommand |
| Infrastructure | Colección password_reset_tokens con índice TTL y cliente SES |
| Api | Endpoints de solicitud y confirmación |

## 3. Modelo de datos e índices

- password_reset_tokens con hash, userId, expiresAt, usedAt e índice TTL

## 4. Seguridad

- Token aleatorio de 32 bytes
- Solo se persiste el hash
- Respuesta neutra ante correo inexistente
- Revocación de sesiones
- Sin token en logs

## 5. Observabilidad y rendimiento

- Métricas de solicitudes, confirmaciones y fallos de envío
- Alerta si el volumen de solicitudes se dispara

## 6. Costo AWS

Amazon SES dentro del Free Tier (3.000 mensajes/mes desde Lambda).

## 7. Estrategia de pruebas

- Unitarias de vigencia y uso único
- Integración de solicitud, confirmación, reuso y expiración
- Prueba de respuesta neutra

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-029-01` y 6 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Un fallo de SES deja al usuario sin enlace
- El correo es un canal fuera del control del sistema

## 10. Orden de implementación

1. `T029-01` Actualizar y validar el contrato OpenAPI del spec
2. `T029-02` Implementar: Entidad PasswordResetToken con reglas de vigencia y uso único
3. `T029-03` Implementar: RequestPasswordResetCommand y ConfirmPasswordResetCommand
4. `T029-04` Implementar: Colección password_reset_tokens con índice TTL y cliente SES
5. `T029-05` Implementar: Endpoints de solicitud y confirmación
6. `T029-06` Modelar el token de recuperación y su vigencia
7. `T029-07` Implementar el envío con SES y la plantilla de correo
8. `T029-08` Coordinar la revocación de sesiones tras el cambio
9. `T029-09` Probar que la respuesta no permite enumerar cuentas
10. `T029-10` Implementar las pruebas de integración del flujo completo
11. `T029-11` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
12. `T029-12` Registrar evidencias del Constitution Check
