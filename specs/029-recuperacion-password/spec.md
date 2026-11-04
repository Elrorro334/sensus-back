# Spec 029 — Recuperación de contraseña

| Campo | Valor |
|---|---|
| ID | 029 |
| Slug | `recuperacion-password` |
| Estado | Pendiente |
| Ola | 1 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 001, 004, 013 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que un usuario que olvidó su contraseña solicite un enlace de recuperación de un solo uso y establezca una contraseña nueva sin revelar si el correo existe.

## 2. Alcance

**Incluye**

- Solicitud de recuperación sin enumeración de cuentas
- Token de un solo uso con hash y TTL
- Confirmación de nueva contraseña
- Revocación de sesiones activas tras el cambio
- Envío por correo con Amazon SES

**No incluye**

- Recuperación por SMS
- Preguntas de seguridad
- Soporte telefónico

## 3. Actores

- Visitante
- Usuario registrado
- Servicio de correo

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-029-01 | Exponer POST /api/v1/auth/recover y responder 202 siempre | Must |
| FR-029-02 | Generar token de 32 bytes, almacenar solo su hash y caducarlo en 30 minutos | Must |
| FR-029-03 | Invalidar tokens anteriores del mismo usuario al emitir uno nuevo | Must |
| FR-029-04 | Enviar el enlace por correo mediante Amazon SES | Must |
| FR-029-05 | Exponer POST /api/v1/auth/recover/confirm para fijar la nueva contraseña | Must |
| FR-029-06 | Rechazar tokens expirados, ya usados o inexistentes con 400 | Must |
| FR-029-07 | Revocar todas las sesiones activas tras el cambio de contraseña | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-029-01 | Latencia de solicitud | p95 < 700 ms |
| NFR-029-02 | Enumeración de cuentas | Cero diferencia de respuesta entre correo existente e inexistente |
| NFR-029-03 | Límite de solicitudes | 3 por correo y hora |

## 6. Criterios de aceptación

### CA-029-01

- **Dado** correo registrado
- **Cuando** POST /api/v1/auth/recover
- **Entonces** 202 y correo con enlace

### CA-029-02

- **Dado** correo inexistente
- **Cuando** POST /api/v1/auth/recover
- **Entonces** 202 idéntico al caso anterior

### CA-029-03

- **Dado** token válido y contraseña nueva válida
- **Cuando** POST /api/v1/auth/recover/confirm
- **Entonces** 204 y sesiones previas revocadas

### CA-029-04

- **Dado** token usado dos veces
- **Cuando** se reintenta la confirmación
- **Entonces** 400

### CA-029-05

- **Dado** token expirado
- **Cuando** se confirma
- **Entonces** 400 sin cambiar la contraseña

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/auth/recover` | Pública |
| POST | `/api/v1/auth/recover/confirm` | Pública |

## 8. Dependencias y supuestos

- Depende de: 001, 004, 013
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Un fallo de SES deja al usuario sin enlace
- El correo es un canal fuera del control del sistema

## 10. Preguntas abiertas

- ¿Se exige reautenticación adicional para cambios desde un dispositivo nuevo?

## 11. Trazabilidad

- Requisitos de este spec: `FR-029-01`, `FR-029-02`, `FR-029-03`, `FR-029-04`, `FR-029-05`, `FR-029-06`, `FR-029-07`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
