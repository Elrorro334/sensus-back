# Spec 002 — Inicio de sesión

| Campo | Valor |
|---|---|
| ID | 002 |
| Slug | `inicio-sesion` |
| Estado | Implementado (retro-spec) |
| Ola | 1 |
| Tipo | Retro-spec |
| Dueño | Por asignar |
| Dependencias | 001 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que un usuario registrado obtenga un token JWT a partir de sus credenciales, sin revelar si el correo existe y respetando el estado de la cuenta.

## 2. Alcance

**Incluye**

- Validación de credenciales con BCrypt
- Emisión de JWT con sub, email, name, roles, jti y exp
- Mensaje de error genérico
- Rechazo de cuentas suspendidas
- Actualización de último acceso

**No incluye**

- Refresh token
- MFA
- Login social
- Bloqueo por intentos fallidos (spec 013)

## 3. Actores

- Usuario registrado

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-002-01 | Autenticar con correo y contraseña | Must |
| FR-002-02 | Emitir JWT con sub, email, name, roles, jti y exp de 60 minutos | Must |
| FR-002-03 | Responder 401 con mensaje genérico ante credenciales inválidas | Must |
| FR-002-04 | Actualizar la fecha de último acceso | Should |
| FR-002-05 | Rechazar cuentas suspendidas o eliminadas | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-002-01 | Latencia de login | p95 < 600 ms |
| NFR-002-02 | Vigencia del token | 60 minutos, configurable por entorno |

## 6. Criterios de aceptación

### CA-002-01

- **Dado** credenciales correctas y cuenta activa
- **Cuando** POST /api/v1/auth/login
- **Entonces** 200 con token y datos de sesión

### CA-002-02

- **Dado** contraseña incorrecta
- **Cuando** se intenta login
- **Entonces** 401 con mensaje genérico

### CA-002-03

- **Dado** correo inexistente
- **Cuando** se intenta login
- **Entonces** 401 idéntico al caso anterior

### CA-002-04

- **Dado** cuenta suspendida
- **Cuando** se intenta login
- **Entonces** 403 sin revelar el estado detallado

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/auth/login` | Pública |

## 8. Dependencias y supuestos

- Depende de: 001
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Sin rate limiting, el endpoint es vulnerable a fuerza bruta (ver spec 013)

## 10. Preguntas abiertas

- ¿Se requiere refresh token o reautenticación al expirar?

## 11. Trazabilidad

- Requisitos de este spec: `FR-002-01`, `FR-002-02`, `FR-002-03`, `FR-002-04`, `FR-002-05`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
