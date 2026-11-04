# Spec 038 — Favoritos de ofertas

| Campo | Valor |
|---|---|
| ID | 038 |
| Slug | `favoritos` |
| Estado | Pendiente |
| Ola | 5 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 032, 042 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que un usuario marque o desmarque una oferta como favorita, alimentando la afinidad de categoría usada por las recomendaciones.

## 2. Alcance

**Incluye**

- Alternancia de favorito por usuario y oferta
- Conteo de favoritos por oferta
- Restricción a ofertas visibles
- Señal de afinidad para el ranking

**No incluye**

- Listado de favoritos (fuera del catálogo)
- Notificaciones por favorito
- Colecciones nombradas

## 3. Actores

- Usuario autenticado

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-038-01 | Alternar el favorito del usuario autenticado sobre una oferta visible | Must |
| FR-038-02 | Devolver el estado resultante y el conteo | Must |
| FR-038-03 | Rechazar ofertas no visibles con 404 | Must |
| FR-038-04 | Garantizar un solo favorito por usuario y oferta | Must |
| FR-038-05 | Exponer la afinidad de categoría al ranking de recomendaciones | Should |
| FR-038-06 | Limitar la operación a 120 alternancias por usuario y hora | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-038-01 | Latencia de la operación | p95 < 300 ms |
| NFR-038-02 | Consistencia | Cero conteos negativos |

## 6. Criterios de aceptación

### CA-038-01

- **Dado** oferta visible sin favorito
- **Cuando** POST /api/v1/users/favorites/{id}
- **Entonces** 200 con favorited=true

### CA-038-02

- **Dado** segunda llamada del mismo usuario
- **Cuando** POST /api/v1/users/favorites/{id}
- **Entonces** 200 con favorited=false

### CA-038-03

- **Dado** oferta archivada
- **Cuando** se intenta marcar
- **Entonces** 404

### CA-038-04

- **Dado** dos dispositivos en paralelo
- **Cuando** se envían ambas peticiones
- **Entonces** un solo registro y conteo correcto

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/users/favorites/{id}` | JWT |

## 8. Dependencias y supuestos

- Depende de: 032, 042
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Alimentar el ranking con datos escasos puede sesgar las recomendaciones

## 10. Preguntas abiertas

- ¿El cliente necesita leer sus favoritos? El catálogo solo expone la alternancia.

## 11. Trazabilidad

- Requisitos de este spec: `FR-038-01`, `FR-038-02`, `FR-038-03`, `FR-038-04`, `FR-038-05`, `FR-038-06`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
