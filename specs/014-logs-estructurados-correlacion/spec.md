# Spec 014 — Logs estructurados y correlación

| Campo | Valor |
|---|---|
| ID | 014 |
| Slug | `logs-estructurados-correlacion` |
| Estado | Pendiente |
| Ola | 6 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 009 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Emitir logs en JSON con identificador de correlación y sin datos personales.

## 2. Alcance

**Incluye**

- Formato JSON
- Propagación de correlation-id
- Redacción de datos sensibles
- Niveles por entorno

**No incluye**

- Trazas distribuidas completas
- Exportación a terceros

## 3. Actores

- Equipo de operación

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-014-01 | Emitir logs en JSON estructurado | Must |
| FR-014-02 | Aceptar o generar correlation-id por petición | Must |
| FR-014-03 | Excluir PII, tokens y contenido moderado | Must |
| FR-014-04 | Registrar método, ruta, estado y duración | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-014-01 | Campos obligatorios | timestamp, level, correlationId, message |
| NFR-014-02 | Sobrecarga | < 5 ms |

## 6. Criterios de aceptación

### CA-014-01

- **Dado** petición con cabecera de correlación
- **Cuando** se procesa
- **Entonces** el log reutiliza ese identificador

### CA-014-02

- **Dado** petición sin cabecera
- **Cuando** se procesa
- **Entonces** se genera un correlation-id y se devuelve

### CA-014-03

- **Dado** petición con datos personales
- **Cuando** se registra
- **Entonces** los datos aparecen enmascarados o ausentes

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| — | — | — |

## 8. Dependencias y supuestos

- Depende de: 009
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- El volumen de logs puede superar el Free Tier

## 10. Preguntas abiertas

- ¿Se define una retención máxima de logs?

## 11. Trazabilidad

- Requisitos de este spec: `FR-014-01`, `FR-014-02`, `FR-014-03`, `FR-014-04`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
