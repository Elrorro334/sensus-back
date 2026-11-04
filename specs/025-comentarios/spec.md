# Spec 025 — Comentarios del feed

| Campo | Valor |
|---|---|
| ID | 025 |
| Slug | `comentarios` |
| Estado | Pendiente |
| Ola | 3 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 017, 024, 010 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir comentar publicaciones visibles con moderación de texto y listado paginado.

## 2. Alcance

**Incluye**

- Alta de comentario
- Moderación del texto
- Listado paginado por publicación

**No incluye**

- Respuestas anidadas
- Edición de comentarios
- Reacciones a comentarios

## 3. Actores

- Usuario autenticado

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-025-01 | Crear comentario en una publicación existente y visible | Must |
| FR-025-02 | Moderar el texto antes de publicarlo | Must |
| FR-025-03 | Listar comentarios paginados por publicación | Must |
| FR-025-04 | Rechazar comentarios en publicaciones no visibles con 404 | Must |
| FR-025-05 | Limitar el comentario a 300 caracteres | Must |
| FR-025-06 | Ordenar los comentarios por fecha ascendente | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-025-01 | Latencia de listado | p95 < 400 ms |
| NFR-025-02 | Paginación | 20 por defecto, 100 máximo |

## 6. Criterios de aceptación

### CA-025-01

- **Dado** texto apto
- **Cuando** POST /api/v1/feed/posts/{id}/comments
- **Entonces** 201 y comentario visible

### CA-025-02

- **Dado** texto ofensivo
- **Cuando** se comenta
- **Entonces** rechazo registrado y sin publicación

### CA-025-03

- **Dado** publicación inexistente
- **Cuando** se comenta
- **Entonces** 404

### CA-025-04

- **Dado** comentario de 301 caracteres
- **Cuando** se comenta
- **Entonces** 400

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/feed/posts/{id}/comments` | JWT |
| GET | `/api/v1/feed/posts/{id}/comments` | JWT |

## 8. Dependencias y supuestos

- Depende de: 017, 024, 010
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Hilos largos degradan la consulta sin índice adecuado

## 10. Preguntas abiertas

- ¿Se permiten respuestas a comentarios en esta fase?

## 11. Trazabilidad

- Requisitos de este spec: `FR-025-01`, `FR-025-02`, `FR-025-03`, `FR-025-04`, `FR-025-05`, `FR-025-06`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
