# Spec 004 — Cierre de sesión y revocación de token

| Campo | Valor |
|---|---|
| ID | 004 |
| Slug | `cierre-sesion-y-revocacion` |
| Estado | Implementado |
| Ola | 6 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 002 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir cerrar sesión de forma efectiva, invalidando el token del usuario antes de su expiración natural.

## 2. Alcance

**Incluye**

- Endpoint de logout autenticado
- Lista de revocación por jti con TTL
- Rechazo de tokens revocados
- Logout idempotente

**No incluye**

- Cierre de sesión en todos los dispositivos
- Revocación administrativa masiva

## 3. Actores

- Usuario autenticado

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-004-01 | Exponer POST /api/v1/auth/logout autenticado | Must |
| FR-004-02 | Registrar el jti revocado con TTL igual a la expiración del token | Must |
| FR-004-03 | Rechazar cualquier petición con token revocado | Must |
| FR-004-04 | Responder 204 aunque el token ya estuviera revocado | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-004-01 | Efectividad de la revocación | < 1 s desde el logout |
| NFR-004-02 | Sobrecarga por petición | < 50 ms |

## 6. Criterios de aceptación

### CA-004-01

- **Dado** token válido
- **Cuando** POST /api/v1/auth/logout
- **Entonces** 204 y el token deja de servir

### CA-004-02

- **Dado** token ya revocado
- **Cuando** se usa en otro endpoint
- **Entonces** 401

### CA-004-03

- **Dado** token ya revocado
- **Cuando** se repite el logout
- **Entonces** 204 sin error

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/auth/logout` | JWT |

## 8. Dependencias y supuestos

- Depende de: 002
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Consultar la denylist en cada petición añade latencia; mitigar con caché

## 10. Preguntas abiertas

- ¿Se revocan también los refresh tokens cuando existan?

## 11. Trazabilidad

- Requisitos de este spec: `FR-004-01`, `FR-004-02`, `FR-004-03`, `FR-004-04`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
