# Spec 010 — Paginación estándar

| Campo | Valor |
|---|---|
| ID | 010 |
| Slug | `paginacion-estandar` |
| Estado | Parcial |
| Ola | 0 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 009 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Definir un contrato único de paginación para todos los endpoints que devuelven colecciones.

## 2. Alcance

**Incluye**

- Modelo PagedResult
- Parámetros page y pageSize
- Validación de límites
- Metadatos de respuesta

**No incluye**

- Paginación por cursor
- Ordenamiento configurable por campo

## 3. Actores

- Cliente de la API

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-010-01 | Exponer PagedResult con items, page, pageSize, total y totalPages | Must |
| FR-010-02 | Aplicar 20 elementos por defecto y 100 como máximo | Must |
| FR-010-03 | Rechazar pageSize fuera de rango con 400 | Must |
| FR-010-04 | Aplicar el contrato a todo endpoint de colección | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-010-01 | Sobrecarga de contar total | p95 < 100 ms adicionales |

## 6. Criterios de aceptación

### CA-010-01

- **Dado** sin parámetros
- **Cuando** se consulta una colección
- **Entonces** pageSize 20 y metadatos completos

### CA-010-02

- **Dado** pageSize=500
- **Cuando** se consulta
- **Entonces** 400 con mensaje de límite

### CA-010-03

- **Dado** página sin resultados
- **Cuando** se consulta
- **Entonces** 200 con lista vacía y total correcto

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| — | — | — |

## 8. Dependencias y supuestos

- Depende de: 009
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Contar el total puede ser costoso en colecciones grandes

## 10. Preguntas abiertas

- ¿Se acepta un total aproximado en colecciones grandes?

## 11. Trazabilidad

- Requisitos de este spec: `FR-010-01`, `FR-010-02`, `FR-010-03`, `FR-010-04`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
