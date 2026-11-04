# Spec 018 — Umbrales y bandas de decisión

| Campo | Valor |
|---|---|
| ID | 018 |
| Slug | `umbrales-y-bandas` |
| Estado | Pendiente |
| Ola | 4 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 017 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Decidir automáticamente entre publicar, revisar o rechazar según puntajes configurables.

## 2. Alcance

**Incluye**

- Tres bandas de decisión
- Umbrales por categoría y por entorno
- Registro del umbral aplicado

**No incluye**

- Aprendizaje automático de umbrales
- Panel de ajuste en tiempo real

## 3. Actores

- Moderador
- Operador

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-018-01 | Definir banda baja, intermedia y alta por categoría | Must |
| FR-018-02 | Externalizar los umbrales en configuración | Must |
| FR-018-03 | Registrar qué umbral se aplicó en cada decisión | Must |
| FR-018-04 | Permitir ajustar umbrales sin recompilar | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-018-01 | Trazabilidad | 100 % de decisiones con umbral registrado |

## 6. Criterios de aceptación

### CA-018-01

- **Dado** puntaje bajo
- **Cuando** se evalúa
- **Entonces** publicación automática

### CA-018-02

- **Dado** puntaje intermedio
- **Cuando** se evalúa
- **Entonces** revisión manual

### CA-018-03

- **Dado** puntaje alto
- **Cuando** se evalúa
- **Entonces** rechazo y posible sanción

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| — | — | — |

## 8. Dependencias y supuestos

- Depende de: 017
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Umbrales mal calibrados generan falsos positivos masivos

## 10. Preguntas abiertas

- ¿Quién aprueba un cambio de umbral?

## 11. Trazabilidad

- Requisitos de este spec: `FR-018-01`, `FR-018-02`, `FR-018-03`, `FR-018-04`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
