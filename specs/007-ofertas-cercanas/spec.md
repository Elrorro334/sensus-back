# Spec 007 — Ofertas cercanas

| Campo | Valor |
|---|---|
| ID | 007 |
| Slug | `ofertas-cercanas` |
| Estado | Pendiente |
| Ola | 2 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 005, 010 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que un usuario descubra ofertas activas cerca de su ubicación, con filtros por radio y categoría, resultados paginados y ordenados por distancia.

## 2. Alcance

**Incluye**

- Consulta geoespacial de ofertas publicadas
- Filtro por radio y categoría
- Exclusión de ofertas vencidas o sin stock
- Paginación estándar
- Distancia calculada en la respuesta

**No incluye**

- Búsqueda por texto (spec 041)
- Ranking personalizado (spec 042)
- Ofertas de comercios inactivos

## 3. Actores

- Usuario autenticado

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-007-01 | Listar ofertas publicadas dentro de un radio de la ubicación enviada | Must |
| FR-007-02 | Excluir ofertas vencidas, pausadas, archivadas o sin stock | Must |
| FR-007-03 | Filtrar por categoría del catálogo | Should |
| FR-007-04 | Aplicar pageSize 20 por defecto y 100 máximo | Must |
| FR-007-05 | Ordenar por distancia ascendente y desempatar por fecha de publicación | Must |
| FR-007-06 | Limitar el radio a 50 km y rechazar valores fuera de rango | Must |
| FR-007-07 | Devolver la distancia en metros junto a cada oferta | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-007-01 | Latencia del listado | p95 < 500 ms |
| NFR-007-02 | Consulta geoespacial | Sin recorrido completo de colección (índice 2dsphere) |

## 6. Criterios de aceptación

### CA-007-01

- **Dado** parámetros válidos
- **Cuando** GET /api/v1/offers/nearby?lat=-12.05&lng=-77.04&radiusKm=5
- **Entonces** 200 con metadatos de paginación y distancia

### CA-007-02

- **Dado** pageSize mayor a 100
- **Cuando** se envía la petición
- **Entonces** 400 con el límite documentado

### CA-007-03

- **Dado** oferta vencida dentro del radio
- **Cuando** se consulta
- **Entonces** no aparece en el resultado

### CA-007-04

- **Dado** radio de 80 km
- **Cuando** se consulta
- **Entonces** 400 por límite excedido

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| GET | `/api/v1/offers/nearby` | JWT |

## 8. Dependencias y supuestos

- Depende de: 005, 010
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Consultas geoespaciales sin índice degradan la latencia
- Un radio grande multiplica resultados y costo de lectura

## 10. Preguntas abiertas

- ¿El listado requiere ubicación obligatoria o admite una ubicación por defecto?

## 11. Trazabilidad

- Requisitos de este spec: `FR-007-01`, `FR-007-02`, `FR-007-03`, `FR-007-04`, `FR-007-05`, `FR-007-06`, `FR-007-07`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
