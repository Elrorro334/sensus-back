# Spec 035 — Edición de oferta

| Campo | Valor |
|---|---|
| ID | 035 |
| Slug | `edicion-oferta` |
| Estado | Pendiente |
| Ola | 2 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 034, 033 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que el comerciante corrija una oferta existente sin romper apartados activos ni saltarse la moderación cuando cambia el contenido visible.

## 2. Alcance

**Incluye**

- Edición de campos de la oferta
- Re-moderación al cambiar texto o imágenes
- Protección de apartados activos
- Control de concurrencia optimista

**No incluye**

- Cambio de comercio propietario
- Cambio de estado (spec 036)
- Historial de versiones visible al usuario

## 3. Actores

- Usuario comerciante
- Servicio de moderación

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-035-01 | Actualizar los campos editables de una oferta propia | Must |
| FR-035-02 | Rechazar la edición de ofertas de otro comercio con 403 | Must |
| FR-035-03 | Re-moderar cuando cambie el texto o las imágenes | Must |
| FR-035-04 | Impedir reducir el stock por debajo de los apartados activos | Must |
| FR-035-05 | Rechazar ediciones concurrentes con 409 usando version | Must |
| FR-035-06 | No cambiar el identificador ni los apartados existentes | Must |
| FR-035-07 | Mantener la oferta oculta mientras la re-moderación está pendiente | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-035-01 | Latencia de edición | p95 < 700 ms |
| NFR-035-02 | Pérdida de cambios | Cero sobrescrituras silenciosas |

## 6. Criterios de aceptación

### CA-035-01

- **Dado** edición de precio y stock válidos
- **Cuando** PUT /api/v1/offers/{id}
- **Entonces** 200 con la versión incrementada

### CA-035-02

- **Dado** stock menor a los apartados activos
- **Cuando** se intenta editar
- **Entonces** 409

### CA-035-03

- **Dado** cambio de descripción
- **Cuando** se edita
- **Entonces** oferta oculta hasta nuevo veredicto

### CA-035-04

- **Dado** dos ediciones con la misma versión
- **Cuando** se envían en paralelo
- **Entonces** una recibe 200 y otra 409

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| PUT | `/api/v1/offers/{id}` | JWT |

## 8. Dependencias y supuestos

- Depende de: 034, 033
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Una edición concurrente puede sobrescribir cambios legítimos
- Reducir stock puede invalidar apartados activos

## 10. Preguntas abiertas

- ¿Se permite editar una oferta con apartados activos o solo pausarla?

## 11. Trazabilidad

- Requisitos de este spec: `FR-035-01`, `FR-035-02`, `FR-035-03`, `FR-035-04`, `FR-035-05`, `FR-035-06`, `FR-035-07`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
