# Spec 006 — Consulta de mi comercio

| Campo | Valor |
|---|---|
| ID | 006 |
| Slug | `consulta-mi-comercio` |
| Estado | Implementado (retro-spec) |
| Ola | 2 |
| Tipo | Retro-spec |
| Dueño | Por asignar |
| Dependencias | 005 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que un usuario comerciante consulte los datos de su propio comercio, incluyendo estado y ubicación, sin acceder a comercios ajenos.

## 2. Alcance

**Incluye**

- Consulta del comercio del usuario autenticado
- Respuesta 404 si no existe
- Datos de estado y ubicación

**No incluye**

- Consulta de comercios ajenos
- Listados públicos
- Estadísticas de ofertas (spec 037)

## 3. Actores

- Usuario comerciante

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-006-01 | Devolver el comercio asociado al usuario autenticado | Must |
| FR-006-02 | Responder 404 si el usuario no tiene comercio | Must |
| FR-006-03 | Impedir la consulta de comercios de otros usuarios | Must |
| FR-006-04 | Incluir estado del comercio y coordenadas normalizadas | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-006-01 | Latencia de consulta | p95 < 300 ms |

## 6. Criterios de aceptación

### CA-006-01

- **Dado** usuario con comercio
- **Cuando** GET /api/v1/merchants/me
- **Entonces** 200 con los datos del comercio

### CA-006-02

- **Dado** usuario sin comercio
- **Cuando** GET /api/v1/merchants/me
- **Entonces** 404

### CA-006-03

- **Dado** usuario sin rol comercial
- **Cuando** GET /api/v1/merchants/me
- **Entonces** 404 sin filtrar datos

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| GET | `/api/v1/merchants/me` | JWT |

## 8. Dependencias y supuestos

- Depende de: 005
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- El endpoint legacy duplica la funcionalidad y puede divergir

## 10. Preguntas abiertas

- Ninguna.

## 11. Trazabilidad

- Requisitos de este spec: `FR-006-01`, `FR-006-02`, `FR-006-03`, `FR-006-04`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
