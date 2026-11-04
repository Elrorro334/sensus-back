# Spec 024 — Publicaciones del feed

| Campo | Valor |
|---|---|
| ID | 024 |
| Slug | `publicaciones-feed` |
| Estado | Pendiente |
| Ola | 3 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 017, 022 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir crear y eliminar publicaciones con texto e imagen opcional, sujetas a moderación previa y con borrado por el autor.

## 2. Alcance

**Incluye**

- Alta de publicación
- Moderación previa obligatoria
- Borrado por el autor o un administrador
- Estado de la publicación

**No incluye**

- Edición de publicaciones
- Menciones y etiquetas
- Publicaciones programadas

## 3. Actores

- Usuario autenticado
- Administrador

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-024-01 | Crear publicación con texto de 1 a 500 caracteres e imagen opcional | Must |
| FR-024-02 | No publicar hasta obtener veredicto favorable | Must |
| FR-024-03 | Registrar autor, fechas y estado | Must |
| FR-024-04 | Devolver 202 mientras la moderación está pendiente | Should |
| FR-024-05 | Permitir al autor eliminar su propia publicación | Must |
| FR-024-06 | Permitir a un administrador eliminar cualquier publicación | Should |
| FR-024-07 | Excluir del feed las publicaciones eliminadas de inmediato | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-024-01 | Latencia percibida | p95 < 1 s hasta aceptar la solicitud |
| NFR-024-02 | Consistencia | Sin publicaciones sin veredicto |

## 6. Criterios de aceptación

### CA-024-01

- **Dado** contenido apto sin imagen
- **Cuando** POST /api/v1/feed/posts
- **Entonces** 201 y publicación visible

### CA-024-02

- **Dado** contenido rechazado
- **Cuando** se crea la publicación
- **Entonces** la publicación no se hace visible

### CA-024-03

- **Dado** sin autenticación
- **Cuando** se intenta publicar
- **Entonces** 401

### CA-024-04

- **Dado** autor elimina su publicación
- **Cuando** DELETE /api/v1/feed/posts/{id}
- **Entonces** 204 y desaparece del feed

### CA-024-05

- **Dado** otro usuario intenta eliminar
- **Cuando** DELETE /api/v1/feed/posts/{id}
- **Entonces** 403

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/feed/posts` | JWT |
| DELETE | `/api/v1/feed/posts/{id}` | JWT |

## 8. Dependencias y supuestos

- Depende de: 017, 022
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Publicación visible antes del veredicto si se invierte el orden
- El borrado lógico puede dejar imágenes accesibles por URL

## 10. Preguntas abiertas

- ¿El borrado es lógico con purga posterior o inmediato?

## 11. Trazabilidad

- Requisitos de este spec: `FR-024-01`, `FR-024-02`, `FR-024-03`, `FR-024-04`, `FR-024-05`, `FR-024-06`, `FR-024-07`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
