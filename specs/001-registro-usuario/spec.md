# Spec 001 — Registro de cuenta unificada

| Campo | Valor |
|---|---|
| ID | 001 |
| Slug | `registro-usuario` |
| Estado | Implementado (retro-spec) |
| Ola | 1 |
| Tipo | Retro-spec |
| Dueño | Por asignar |
| Dependencias | Ninguna |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que una persona cree una cuenta única con correo, nombre completo y contraseña, obteniendo una sesión JWT con rol Consumidor; el rol comercial se activa después (spec 005).

## 2. Alcance

**Incluye**

- Alta con correo único y nombre completo
- Política de contraseña y hash BCrypt
- Modelo de cuenta unificada con rol Consumidor
- Aceptación de términos con fecha
- Token JWT en la respuesta

**No incluye**

- Verificación de correo
- Recuperación de contraseña (spec 029)
- Login social
- MFA
- Selección de rol en el registro

## 3. Actores

- Visitante
- Usuario registrado

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-001-01 | Registrar cuenta con correo, nombre completo y contraseña | Must |
| FR-001-02 | Rechazar correo con formato inválido o ya registrado | Must |
| FR-001-03 | Exigir contraseña de 8+ caracteres con mayúscula, minúscula y número | Must |
| FR-001-04 | Almacenar la contraseña con BCrypt y salt, nunca en texto plano | Must |
| FR-001-05 | Asignar rol Consumidor en un modelo de cuenta unificada | Must |
| FR-001-06 | Exigir aceptación de términos y registrar la fecha | Must |
| FR-001-07 | Devolver sesión activa con token JWT de 60 minutos | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-001-01 | Latencia de registro | p95 < 800 ms |
| NFR-001-02 | Errores 5xx en registro | < 0.5 % |

## 6. Criterios de aceptación

### CA-001-01

- **Dado** datos válidos y correo no registrado
- **Cuando** POST /api/v1/auth/register
- **Entonces** 201 con token y perfil sin passwordHash

### CA-001-02

- **Dado** correo ya registrado
- **Cuando** se intenta registrar de nuevo
- **Entonces** 409 con mensaje de conflicto

### CA-001-03

- **Dado** contraseña débil
- **Cuando** se intenta registrar
- **Entonces** 400 con los criterios incumplidos

### CA-001-04

- **Dado** términos no aceptados
- **Cuando** se intenta registrar
- **Entonces** 400 y no se crea la cuenta

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/auth/register` | Pública |

## 8. Dependencias y supuestos

- Depende de: Ninguna
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- El secreto JWT versionado obliga a rotación
- El índice único no está probado contra MongoDB real

## 10. Preguntas abiertas

- ¿Debe verificarse el correo antes de permitir la primera sesión?

## 11. Trazabilidad

- Requisitos de este spec: `FR-001-01`, `FR-001-02`, `FR-001-03`, `FR-001-04`, `FR-001-05`, `FR-001-06`, `FR-001-07`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
