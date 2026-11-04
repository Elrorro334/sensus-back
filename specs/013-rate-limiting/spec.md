# Spec 013 — Control de tasa de peticiones

| Campo | Valor |
|---|---|
| ID | 013 |
| Slug | `rate-limiting` |
| Estado | Pendiente |
| Ola | 6 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 012 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Proteger la disponibilidad de la API limitando la tasa de peticiones por cliente y por endpoint sensible.

## 2. Alcance

**Incluye**

- Límite por IP
- Límite por usuario autenticado
- Respuesta 429 con Retry-After
- Umbrales configurables

**No incluye**

- Bloqueo permanente de IP
- WAF
- Cuotas por plan comercial

## 3. Actores

- Cliente de la API
- Operador

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-013-01 | Limitar peticiones por IP | Must |
| FR-013-02 | Limitar peticiones por usuario autenticado | Must |
| FR-013-03 | Responder 429 con Retry-After | Must |
| FR-013-04 | Configurar umbrales por entorno sin recompilar | Must |
| FR-013-05 | Aplicar límites más estrictos en autenticación | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-013-01 | Sobrecarga del limitador | < 10 ms por petición |
| NFR-013-02 | Disponibilidad ante picos | Sin degradar a 5xx |

## 6. Criterios de aceptación

### CA-013-01

- **Dado** límite excedido
- **Cuando** se superan las peticiones permitidas
- **Entonces** 429 con Retry-After

### CA-013-02

- **Dado** login repetido
- **Cuando** se supera el umbral de autenticación
- **Entonces** 429 y bloqueo temporal

### CA-013-03

- **Dado** tráfico normal
- **Cuando** se opera dentro del umbral
- **Entonces** sin cambios de comportamiento

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| — | — | — |

## 8. Dependencias y supuestos

- Depende de: 012
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- El límite en memoria no es consistente entre instancias de Lambda

## 10. Preguntas abiertas

- ¿Se requiere un limitador distribuido en una fase posterior?

## 11. Trazabilidad

- Requisitos de este spec: `FR-013-01`, `FR-013-02`, `FR-013-03`, `FR-013-04`, `FR-013-05`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
