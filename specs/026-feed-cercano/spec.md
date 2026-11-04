# Spec 026 — Feed geolocalizado

| Campo | Valor |
|---|---|
| ID | 026 |
| Slug | `feed-cercano` |
| Estado | Pendiente |
| Ola | 3 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 024, 007, 010 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Entregar un feed paginado de publicaciones visibles cercanas a la ubicación del usuario, ordenado cronológicamente.

## 2. Alcance

**Incluye**

- Listado de publicaciones visibles dentro de un radio
- Orden cronológico inverso
- Paginación estándar
- Caché de lecturas recurrentes

**No incluye**

- Feed personalizado por intereses (spec 042)
- Tendencias
- Publicaciones sin ubicación

## 3. Actores

- Usuario autenticado

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-026-01 | Listar publicaciones publicadas dentro del radio, en orden cronológico inverso | Must |
| FR-026-02 | Excluir contenido en cuarentena, rechazado o eliminado | Must |
| FR-026-03 | Aplicar paginación estándar | Must |
| FR-026-04 | Limitar el radio a 50 km y rechazar valores fuera de rango | Must |
| FR-026-05 | Excluir publicaciones sin ubicación del feed cercano | Must |
| FR-026-06 | Usar caché de corta duración para páginas recurrentes | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-026-01 | Latencia del feed | p95 < 300 ms con caché |
| NFR-026-02 | Índice | Compuesto por status + location + createdAt |

## 6. Criterios de aceptación

### CA-026-01

- **Dado** feed solicitado
- **Cuando** GET /api/v1/feed/nearby?lat=-12.05&lng=-77.04&radiusKm=5
- **Entonces** 200 con publicaciones paginadas

### CA-026-02

- **Dado** contenido en cuarentena
- **Cuando** se consulta el feed
- **Entonces** no aparece

### CA-026-03

- **Dado** publicación sin ubicación
- **Cuando** se consulta el feed
- **Entonces** no aparece

### CA-026-04

- **Dado** segunda consulta idéntica
- **Cuando** se repite la petición
- **Entonces** respuesta desde caché

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| GET | `/api/v1/feed/nearby` | JWT |

## 8. Dependencias y supuestos

- Depende de: 024, 007, 010
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- El caché en memoria no se comparte entre instancias de Lambda
- La ubicación heredada puede quedar desactualizada si el comercio se muda

## 10. Preguntas abiertas

- ¿Las publicaciones de consumidores deben heredar la última ubicación conocida?

## 11. Trazabilidad

- Requisitos de este spec: `FR-026-01`, `FR-026-02`, `FR-026-03`, `FR-026-04`, `FR-026-05`, `FR-026-06`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
