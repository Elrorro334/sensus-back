# Spec 039 — Me gusta en publicaciones

| Campo | Valor |
|---|---|
| ID | 039 |
| Slug | `me-gusta-publicaciones` |
| Estado | Pendiente |
| Ola | 3 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 024, 013 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que un usuario marque o desmarque su me gusta en una publicación visible, con semántica de alternancia y conteo consistente.

## 2. Alcance

**Incluye**

- Alternancia de me gusta por usuario y publicación
- Conteo agregado
- Restricción a publicaciones visibles
- Idempotencia del resultado

**No incluye**

- Reacciones múltiples
- Listado de usuarios que dieron me gusta
- Notificaciones por me gusta (spec 044)

## 3. Actores

- Usuario autenticado

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-039-01 | Alternar el me gusta del usuario autenticado sobre una publicación | Must |
| FR-039-02 | Devolver el estado resultante y el conteo actualizado | Must |
| FR-039-03 | Rechazar publicaciones no visibles con 404 | Must |
| FR-039-04 | Garantizar un solo registro por usuario y publicación | Must |
| FR-039-05 | Mantener el conteo consistente ante peticiones repetidas | Must |
| FR-039-06 | Limitar la operación a 60 alternancias por usuario y hora | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-039-01 | Latencia de la operación | p95 < 300 ms |
| NFR-039-02 | Consistencia del conteo | Cero conteos negativos |

## 6. Criterios de aceptación

### CA-039-01

- **Dado** publicación visible sin me gusta
- **Cuando** POST /api/v1/feed/posts/{id}/like
- **Entonces** 200 con liked=true y conteo incrementado

### CA-039-02

- **Dado** segunda llamada del mismo usuario
- **Cuando** POST /api/v1/feed/posts/{id}/like
- **Entonces** 200 con liked=false y conteo decrementado

### CA-039-03

- **Dado** publicación en cuarentena
- **Cuando** se intenta reaccionar
- **Entonces** 404

### CA-039-04

- **Dado** dos dispositivos del mismo usuario en paralelo
- **Cuando** se envían ambas peticiones
- **Entonces** un solo registro y conteo correcto

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/feed/posts/{id}/like` | JWT |

## 8. Dependencias y supuestos

- Depende de: 024, 013
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Un contador desincronizado rompe la confianza en la métrica
- El POST con semántica de alternancia no es idempotente por diseño

## 10. Preguntas abiertas

- ¿Se acepta POST con alternancia o se prefiere DELETE explícito fuera del catálogo?

## 11. Trazabilidad

- Requisitos de este spec: `FR-039-01`, `FR-039-02`, `FR-039-03`, `FR-039-04`, `FR-039-05`, `FR-039-06`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
