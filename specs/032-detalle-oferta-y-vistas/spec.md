# Spec 032 — Detalle de oferta y contador de vistas

| Campo | Valor |
|---|---|
| ID | 032 |
| Slug | `detalle-oferta-y-vistas` |
| Estado | Pendiente |
| Ola | 2 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 007 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir consultar una oferta completa y registrar una vista por usuario y día, para alimentar la popularidad usada por el ranking.

## 2. Alcance

**Incluye**

- Detalle completo de la oferta
- Datos públicos del comercio
- Indicador de apartado disponible
- Registro de vista deduplicado por usuario y día

**No incluye**

- Edición de la oferta (spec 035)
- Reseñas
- Estadísticas del comerciante (spec 037)

## 3. Actores

- Usuario autenticado
- Comerciante

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-032-01 | Devolver el detalle de una oferta visible con comercio, vigencia, stock y precio | Must |
| FR-032-02 | Responder 404 si la oferta no existe o no es visible | Must |
| FR-032-03 | Registrar una vista por usuario, oferta y día | Must |
| FR-032-04 | Responder 204 a la vista sin revelar datos internos | Must |
| FR-032-05 | Indicar si el usuario ya tiene un apartado activo sobre la oferta | Should |
| FR-032-06 | No permitir vistas sobre ofertas archivadas | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-032-01 | Latencia del detalle | p95 < 350 ms |
| NFR-032-02 | Sobrecarga de la vista | < 100 ms y sin bloquear la respuesta |

## 6. Criterios de aceptación

### CA-032-01

- **Dado** oferta publicada
- **Cuando** GET /api/v1/offers/{id}
- **Entonces** 200 con detalle completo

### CA-032-02

- **Dado** oferta archivada
- **Cuando** GET /api/v1/offers/{id}
- **Entonces** 404

### CA-032-03

- **Dado** primera vista del día
- **Cuando** POST /api/v1/offers/{id}/view
- **Entonces** 204 y contador incrementado en 1

### CA-032-04

- **Dado** segunda vista del mismo día
- **Cuando** POST /api/v1/offers/{id}/view
- **Entonces** 204 y contador sin cambios

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| GET | `/api/v1/offers/{id}` | JWT |
| POST | `/api/v1/offers/{id}/view` | JWT |

## 8. Dependencias y supuestos

- Depende de: 007
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- El contador de vistas puede inflarse con cuentas nuevas
- La escritura de vistas compite con la lectura del detalle

## 10. Preguntas abiertas

- ¿La vista se registra de forma síncrona o encolada?

## 11. Trazabilidad

- Requisitos de este spec: `FR-032-01`, `FR-032-02`, `FR-032-03`, `FR-032-04`, `FR-032-05`, `FR-032-06`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
