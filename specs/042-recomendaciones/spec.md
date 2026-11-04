# Spec 042 — Recomendaciones personalizadas

| Campo | Valor |
|---|---|
| ID | 042 |
| Slug | `recomendaciones` |
| Estado | Pendiente |
| Ola | 5 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 007, 038, 039, 041 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Recomendar ofertas cercanas combinando proximidad, afinidad de categoría, vigencia y popularidad, con reglas deterministas y explicables.

## 2. Alcance

**Incluye**

- Ranking por proximidad, afinidad, vigencia y popularidad
- Uso de favoritos, me gusta y apartados como señales
- Exclusión de no visibles
- Caché de corta duración

**No incluye**

- Modelos entrenados
- Perfilado con datos sensibles
- Recomendación de usuarios
- Explicación al usuario final

## 3. Actores

- Usuario autenticado

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-042-01 | Devolver un máximo de 20 ofertas recomendadas para el usuario | Must |
| FR-042-02 | Calcular el puntaje con pesos configurables de proximidad, afinidad, vigencia y popularidad | Must |
| FR-042-03 | Derivar la afinidad de favoritos, me gusta y apartados del usuario | Should |
| FR-042-04 | Excluir ofertas vencidas, pausadas, sin stock o de comercios inactivos | Must |
| FR-042-05 | Aplicar un fallback a ofertas cercanas cuando no haya señales del usuario | Must |
| FR-042-06 | Devolver resultados estables para el mismo usuario e insumos | Must |
| FR-042-07 | No usar datos personales sensibles en el cálculo | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-042-01 | Latencia | p95 < 800 ms |
| NFR-042-02 | Cobertura | ≥ 90 % de usuarios con al menos 5 recomendaciones válidas |
| NFR-042-03 | Explicabilidad | 100 % de los pesos documentados y versionados |

## 6. Criterios de aceptación

### CA-042-01

- **Dado** usuario con favoritos en una categoría
- **Cuando** GET /api/v1/ml/recommendations
- **Entonces** 200 con ofertas de esa categoría en el top 5

### CA-042-02

- **Dado** usuario sin señales
- **Cuando** se consulta
- **Entonces** 200 con ofertas cercanas por proximidad

### CA-042-03

- **Dado** oferta vencida
- **Cuando** se consulta
- **Entonces** no aparece

### CA-042-04

- **Dado** misma entrada repetida
- **Cuando** se consulta dos veces
- **Entonces** orden idéntico

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| GET | `/api/v1/ml/recommendations` | JWT |

## 8. Dependencias y supuestos

- Depende de: 007, 038, 039, 041
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Pocas señales producen recomendaciones triviales
- El caché puede mostrar ofertas recién pausadas

## 10. Preguntas abiertas

- ¿Debe el usuario poder reiniciar sus preferencias?

## 11. Trazabilidad

- Requisitos de este spec: `FR-042-01`, `FR-042-02`, `FR-042-03`, `FR-042-04`, `FR-042-05`, `FR-042-06`, `FR-042-07`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
