# Spec 015 — Métricas y alarmas

| Campo | Valor |
|---|---|
| ID | 015 |
| Slug | `metricas-y-alarmas` |
| Estado | Pendiente |
| Ola | 6 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 014 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Medir latencia, errores y saturación, con alarmas definidas como código.

## 2. Alcance

**Incluye**

- Métricas de latencia y errores
- Alarmas en CloudWatch
- Panel mínimo

**No incluye**

- Apm de terceros
- Trazas distribuidas

## 3. Actores

- Equipo de operación

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-015-01 | Publicar métricas de latencia, errores y throttling | Must |
| FR-015-02 | Definir alarmas en IaC con umbrales explícitos | Must |
| FR-015-03 | Notificar alarmas a un canal definido | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-015-01 | Coste de alarmas | Dentro del Free Tier (10 alarmas) |
| NFR-015-02 | Latencia de detección | < 5 min |

## 6. Criterios de aceptación

### CA-015-01

- **Dado** tasa de error superior al umbral
- **Cuando** se ejecuta la alarma
- **Entonces** notificación emitida

### CA-015-02

- **Dado** despliegue de infraestructura
- **Cuando** se aplica IaC
- **Entonces** las alarmas quedan creadas

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| — | — | — |

## 8. Dependencias y supuestos

- Depende de: 014
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Sin IaC no se pueden versionar las alarmas

## 10. Preguntas abiertas

- ¿Dónde se notifican las alarmas: correo, Slack o SNS?

## 11. Trazabilidad

- Requisitos de este spec: `FR-015-01`, `FR-015-02`, `FR-015-03`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
