# Spec 037 — Inventario de ofertas del comerciante

| Campo | Valor |
|---|---|
| ID | 037 |
| Slug | `inventario-comerciante` |
| Estado | Pendiente |
| Ola | 2 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 034, 033, 010 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que el comerciante consulte el historial de sus ofertas con métricas de apartados, canjes y vistas, filtrable por estado y paginado.

## 2. Alcance

**Incluye**

- Listado de ofertas propias activas e inactivas
- Filtro por estado
- Métricas agregadas por oferta
- Paginación estándar

**No incluye**

- Exportación a CSV
- Series temporales
- Comparativa entre comercios

## 3. Actores

- Usuario comerciante

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-037-01 | Listar las ofertas del comercio autenticado, incluidas las inactivas | Must |
| FR-037-02 | Filtrar por estado de la oferta | Should |
| FR-037-03 | Incluir apartados activos, canjes y vistas por oferta | Must |
| FR-037-04 | Aplicar pageSize 20 por defecto y 100 máximo | Must |
| FR-037-05 | Ordenar por fecha de creación descendente | Must |
| FR-037-06 | Impedir ver ofertas de otros comercios | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-037-01 | Latencia del listado | p95 < 600 ms |
| NFR-037-02 | Costo del agregado | Sin recorrido completo por oferta |

## 6. Criterios de aceptación

### CA-037-01

- **Dado** comercio con ofertas
- **Cuando** GET /api/v1/merchants/my-offers
- **Entonces** 200 con ofertas y métricas

### CA-037-02

- **Dado** filtro por estado Archived
- **Cuando** se consulta
- **Entonces** solo ofertas archivadas

### CA-037-03

- **Dado** pageSize de 200
- **Cuando** se consulta
- **Entonces** 400

### CA-037-04

- **Dado** comercio sin ofertas
- **Cuando** se consulta
- **Entonces** 200 con lista vacía y total 0

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| GET | `/api/v1/merchants/my-offers` | JWT |

## 8. Dependencias y supuestos

- Depende de: 034, 033, 010
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- La agregación de métricas puede degradarse con muchas ofertas
- El total puede desviarse si se cuenta en dos consultas

## 10. Preguntas abiertas

- ¿Las métricas deben ser en tiempo real o pueden tener un desfase de minutos?

## 11. Trazabilidad

- Requisitos de este spec: `FR-037-01`, `FR-037-02`, `FR-037-03`, `FR-037-04`, `FR-037-05`, `FR-037-06`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
