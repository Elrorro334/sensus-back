# Spec 028 — Webhooks entrantes (backlog)

| Campo | Valor |
|---|---|
| ID | 028 |
| Slug | `webhooks-entrantes` |
| Estado | Backlog |
| Ola | Backlog |
| Tipo | Backlog |
| Dueño | Por asignar |
| Dependencias | 009, 014 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Recibir eventos externos de forma segura, idempotente y sin bloquear al emisor. Capacidad fuera del catálogo vigente de 29 endpoints; se conserva como backlog.

## 2. Alcance

**Incluye**

- Recepción de eventos
- Verificación de firma
- Idempotencia por identificador
- Respuesta rápida

**No incluye**

- Webhooks salientes
- Reproceso manual desde panel

## 3. Actores

- Sistema externo
- Equipo de operación

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-028-01 | Exponer un endpoint de recepción de eventos | Must |
| FR-028-02 | Verificar la firma del emisor | Must |
| FR-028-03 | Garantizar idempotencia por eventId | Must |
| FR-028-04 | Responder 2xx en menos de 1 s y procesar de forma asíncrona | Must |
| FR-028-05 | Registrar eventos fallidos para reintento | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-028-01 | Latencia de respuesta | p95 < 500 ms |
| NFR-028-02 | Duplicados procesados | Cero efectos duplicados |

## 6. Criterios de aceptación

### CA-028-01

- **Dado** firma válida y evento nuevo
- **Cuando** se recibe el webhook
- **Entonces** 202 y evento encolado

### CA-028-02

- **Dado** firma inválida
- **Cuando** se recibe el webhook
- **Entonces** 401 sin procesar

### CA-028-03

- **Dado** eventId repetido
- **Cuando** se recibe de nuevo
- **Entonces** 200 sin reprocesar

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/webhooks/{provider}` | Firma HMAC |

## 8. Dependencias y supuestos

- Depende de: 009, 014
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- No hay proveedor identificado que justifique la capacidad
- Mantenerla activa consume presupuesto de especificación

## 10. Preguntas abiertas

- ¿Qué proveedor enviará webhooks y en qué fecha?

## 11. Trazabilidad

- Requisitos de este spec: `FR-028-01`, `FR-028-02`, `FR-028-03`, `FR-028-04`, `FR-028-05`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
