# Spec 036 — Ciclo de vida de la oferta

| Campo | Valor |
|---|---|
| ID | 036 |
| Slug | `ciclo-vida-oferta` |
| Estado | Pendiente |
| Ola | 2 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 034, 035 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que el comerciante publique, pause o archive sus ofertas mediante transiciones explícitas y auditables.

## 2. Alcance

**Incluye**

- Cambio de estado Published, Paused y Archived
- Matriz de transiciones válidas
- Efecto inmediato en la visibilidad
- Auditoría del cambio

**No incluye**

- Eliminación física de ofertas
- Programación de publicación por fecha
- Aprobación administrativa

## 3. Actores

- Usuario comerciante
- Sistema

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-036-01 | Cambiar el estado de una oferta propia entre Published, Paused y Archived | Must |
| FR-036-02 | Rechazar transiciones no permitidas con 409 | Must |
| FR-036-03 | Ocultar de inmediato las ofertas pausadas o archivadas | Must |
| FR-036-04 | Impedir archivar una oferta con apartados activos | Must |
| FR-036-05 | Publicar solo ofertas con moderación aprobada y stock disponible | Must |
| FR-036-06 | Registrar estado anterior, nuevo estado, actor y fecha | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-036-01 | Efectividad del cambio | < 2 s en la consulta de cercanía |
| NFR-036-02 | Auditoría | 100 % de transiciones registradas |

## 6. Criterios de aceptación

### CA-036-01

- **Dado** oferta Published sin apartados
- **Cuando** PUT /api/v1/offers/{id}/status a Archived
- **Entonces** 200 y desaparece de cercanía

### CA-036-02

- **Dado** oferta con apartado activo
- **Cuando** se intenta archivar
- **Entonces** 409

### CA-036-03

- **Dado** oferta pausada
- **Cuando** se vuelve a publicar
- **Entonces** 200 y reaparece si sigue vigente

### CA-036-04

- **Dado** transición Archived a Published
- **Cuando** se intenta
- **Entonces** 409 y la oferta sigue archivada

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| PUT | `/api/v1/offers/{id}/status` | JWT |

## 8. Dependencias y supuestos

- Depende de: 034, 035
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Una transición mal definida deja ofertas invisibles para siempre
- El archivado con apartados activos puede dejar usuarios sin canje

## 10. Preguntas abiertas

- ¿Se permite reprogramar una oferta archivada?

## 11. Trazabilidad

- Requisitos de este spec: `FR-036-01`, `FR-036-02`, `FR-036-03`, `FR-036-04`, `FR-036-05`, `FR-036-06`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
