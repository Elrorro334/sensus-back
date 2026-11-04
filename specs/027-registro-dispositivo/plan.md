# Plan 027 — Registro de dispositivo para push

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Registrar y actualizar el token de dispositivo del usuario para habilitar notificaciones push, con despacho abstracto y limpieza de tokens inválidos.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Entidad Device e IPushDispatcher |
| Application | RegisterDeviceCommand y DispatchNotificationCommand |
| Infrastructure | Adaptadores FCM y SNS, repositorio de dispositivos |
| Api | Endpoint PUT /api/v1/users/device-token |

## 3. Modelo de datos e índices

- devices con índice único en token y en userId + deviceId

## 4. Seguridad

- El token pertenece al usuario autenticado
- Sin datos sensibles en la carga
- Credenciales del proveedor en Secrets Manager

## 5. Observabilidad y rendimiento

- Métricas de registros, envíos, fallos y tokens depurados

## 6. Costo AWS

FCM sin costo; SNS dentro del Free Tier si se elige ese adaptador.

## 7. Estrategia de pruebas

- Unitarias de registro y reasignación
- Integración con proveedor simulado
- Test de arquitectura: Infrastructure no filtra el SDK

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-027-01` y 6 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Tokens duplicados entre usuarios
- Un adaptador mal configurado deja las notificaciones sin enviar

## 10. Orden de implementación

1. `T027-01` Actualizar y validar el contrato OpenAPI del spec
2. `T027-02` Implementar: Entidad Device e IPushDispatcher
3. `T027-03` Implementar: RegisterDeviceCommand y DispatchNotificationCommand
4. `T027-04` Implementar: Adaptadores FCM y SNS, repositorio de dispositivos
5. `T027-05` Implementar: Endpoint PUT /api/v1/users/device-token
6. `T027-06` Definir IPushDispatcher y la entidad Device
7. `T027-07` Implementar los adaptadores FCM y SNS
8. `T027-08` Resolver la reasignación de token entre usuarios
9. `T027-09` Implementar las pruebas de integración del flujo completo
10. `T027-10` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
11. `T027-11` Registrar evidencias del Constitution Check
