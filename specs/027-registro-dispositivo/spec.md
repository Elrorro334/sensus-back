# Spec 027 — Registro de dispositivo para push

| Campo | Valor |
|---|---|
| ID | 027 |
| Slug | `registro-dispositivo` |
| Estado | Pendiente |
| Ola | 4 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 003, 013 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Registrar y actualizar el token de dispositivo del usuario para habilitar notificaciones push, con despacho abstracto y limpieza de tokens inválidos.

## 2. Alcance

**Incluye**

- Alta y actualización de token por dispositivo
- Reasignación segura entre usuarios
- Despacho mediante IPushDispatcher
- Limpieza de tokens rechazados

**No incluye**

- Campañas masivas
- Programación de envíos
- Preferencias por tipo (spec 044)

## 3. Actores

- Usuario autenticado
- Proveedor de push

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-027-01 | Registrar token de dispositivo con plataforma y deviceId | Must |
| FR-027-02 | Actualizar el token existente sin duplicar el dispositivo | Must |
| FR-027-03 | Despachar notificaciones mediante IPushDispatcher | Must |
| FR-027-04 | Seleccionar el adaptador FCM o SNS por configuración | Must |
| FR-027-05 | Eliminar tokens rechazados por el proveedor | Must |
| FR-027-06 | Reasignar un token que cambia de usuario sin dejar acceso residual | Must |
| FR-027-07 | No fallar si el usuario no tiene dispositivos activos | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-027-01 | Latencia de registro | p95 < 300 ms |
| NFR-027-02 | Latencia de despacho | p95 < 2 s |
| NFR-027-03 | Acoplamiento al proveedor | Cero referencias al SDK fuera de Infrastructure |

## 6. Criterios de aceptación

### CA-027-01

- **Dado** token nuevo
- **Cuando** PUT /api/v1/users/device-token
- **Entonces** 200 y queda asociado al usuario

### CA-027-02

- **Dado** mismo deviceId con token renovado
- **Cuando** PUT /api/v1/users/device-token
- **Entonces** 200 sin duplicar el dispositivo

### CA-027-03

- **Dado** token inválido en el envío
- **Cuando** se despacha
- **Entonces** el token se elimina y se registra

### CA-027-04

- **Dado** usuario sin dispositivos
- **Cuando** se intenta notificar
- **Entonces** sin error

### CA-027-05

- **Dado** token de otro usuario
- **Cuando** se registra
- **Entonces** se reasigna y el anterior deja de recibir

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| PUT | `/api/v1/users/device-token` | JWT |

## 8. Dependencias y supuestos

- Depende de: 003, 013
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Tokens duplicados entre usuarios
- Un adaptador mal configurado deja las notificaciones sin enviar

## 10. Preguntas abiertas

- ¿Qué adaptador será el predeterminado en producción: FCM o SNS?

## 11. Trazabilidad

- Requisitos de este spec: `FR-027-01`, `FR-027-02`, `FR-027-03`, `FR-027-04`, `FR-027-05`, `FR-027-06`, `FR-027-07`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
