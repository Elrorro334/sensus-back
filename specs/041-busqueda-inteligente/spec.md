# Spec 041 — Búsqueda de ofertas

| Campo | Valor |
|---|---|
| ID | 041 |
| Slug | `busqueda-inteligente` |
| Estado | Pendiente |
| Ola | 5 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 007, 010, 011 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir buscar ofertas por texto combinando relevancia y cercanía, con un ranking determinista y medible dentro del backend.

## 2. Alcance

**Incluye**

- Búsqueda por texto en título, descripción, categoría y comercio
- Combinación con cercanía
- Ranking determinista con pesos configurables
- Paginación estándar
- Fallback a ofertas cercanas sin texto

**No incluye**

- Modelos de lenguaje externos
- Búsqueda semántica vectorial
- Corrección ortográfica
- Historial de búsquedas

## 3. Actores

- Usuario autenticado

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-041-01 | Buscar ofertas visibles por texto libre de 2 a 80 caracteres | Must |
| FR-041-02 | Calcular un puntaje determinista con pesos de texto, distancia y popularidad | Must |
| FR-041-03 | Excluir ofertas vencidas, pausadas o sin stock | Must |
| FR-041-04 | Aplicar paginación estándar | Must |
| FR-041-05 | Devolver ofertas cercanas cuando la consulta esté vacía | Should |
| FR-041-06 | Excluir del resultado las ofertas de comercios inactivos | Must |
| FR-041-07 | Versionar y configurar los pesos del ranking por entorno | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-041-01 | Latencia de búsqueda | p95 < 600 ms |
| NFR-041-02 | Relevancia | ≥ 90 % de consultas con coincidencia exacta de categoría en el top 10 |
| NFR-041-03 | Determinismo | Mismo conjunto de datos y consulta produce el mismo orden |

## 6. Criterios de aceptación

### CA-041-01

- **Dado** consulta "panadería" con ofertas cercanas
- **Cuando** GET /api/v1/ml/search?q=panadería
- **Entonces** 200 con resultados ordenados por puntaje

### CA-041-02

- **Dado** consulta vacía
- **Cuando** GET /api/v1/ml/search
- **Entonces** 200 con ofertas cercanas como fallback

### CA-041-03

- **Dado** oferta vencida relevante
- **Cuando** se busca
- **Entonces** no aparece

### CA-041-04

- **Dado** misma consulta repetida
- **Cuando** se ejecuta dos veces
- **Entonces** orden idéntico

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| GET | `/api/v1/ml/search` | JWT |

## 8. Dependencias y supuestos

- Depende de: 007, 010, 011
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Sin motor de búsqueda dedicado, el índice de texto de MongoDB tiene límites de idioma
- Los pesos mal calibrados degradan la relevancia

## 10. Preguntas abiertas

- ¿Se requiere análisis morfológico en español o basta con coincidencia de prefijos?

## 11. Trazabilidad

- Requisitos de este spec: `FR-041-01`, `FR-041-02`, `FR-041-03`, `FR-041-04`, `FR-041-05`, `FR-041-06`, `FR-041-07`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
