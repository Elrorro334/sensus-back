# Spec 009 — Errores uniformes con ProblemDetails

| Campo | Valor |
|---|---|
| ID | 009 |
| Slug | `errores-problemdetails` |
| Estado | Parcial |
| Ola | 0 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | Ninguna |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Garantizar que toda respuesta de error use ProblemDetails con un identificador de correlación y sin filtrar detalles internos.

## 2. Alcance

**Incluye**

- Formato único de errores
- Correlation-id en la respuesta
- Catálogo de códigos de error

**No incluye**

- Internacionalización de mensajes
- Documentación interactiva de errores

## 3. Actores

- Cliente de la API
- Equipo de soporte

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-009-01 | Toda respuesta de error usa ProblemDetails | Must |
| FR-009-02 | Incluir traceId o correlationId en cada error | Must |
| FR-009-03 | No exponer stack traces ni nombres de colecciones | Must |
| FR-009-04 | Mantener un catálogo de códigos de error documentado | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-009-01 | Estabilidad del formato | Sin cambios incompatibles dentro de v1 |

## 6. Criterios de aceptación

### CA-009-01

- **Dado** error de validación
- **Cuando** se envía una petición inválida
- **Entonces** 400 con ProblemDetails y errores por campo

### CA-009-02

- **Dado** excepción no controlada
- **Cuando** se produce un fallo interno
- **Entonces** 500 genérico con correlationId y sin stack trace

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| — | — | — |

## 8. Dependencias y supuestos

- Depende de: Ninguna
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Cambiar el formato rompe clientes que parsean el cuerpo actual

## 10. Preguntas abiertas

- ¿Se mantiene compatibilidad con el formato actual durante una versión?

## 11. Trazabilidad

- Requisitos de este spec: `FR-009-01`, `FR-009-02`, `FR-009-03`, `FR-009-04`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
