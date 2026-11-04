# Spec 016 — Health checks

| Campo | Valor |
|---|---|
| ID | 016 |
| Slug | `health-checks` |
| Estado | Parcial |
| Ola | 6 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 014 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Exponer estado de vida y de preparación sin revelar detalles internos.

## 2. Alcance

**Incluye**

- Endpoint de liveness
- Endpoint de readiness con dependencia de MongoDB

**No incluye**

- Paneles de estado públicos
- Sondas de terceros

## 3. Actores

- Orquestador
- Equipo de operación

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-016-01 | Exponer /health/live sin dependencias externas | Must |
| FR-016-02 | Exponer /health/ready verificando MongoDB | Must |
| FR-016-03 | No exponer cadenas de conexión ni versiones | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-016-01 | Latencia de readiness | < 500 ms |

## 6. Criterios de aceptación

### CA-016-01

- **Dado** proceso activo
- **Cuando** GET /health/live
- **Entonces** 200

### CA-016-02

- **Dado** MongoDB inaccesible
- **Cuando** GET /health/ready
- **Entonces** 503

### CA-016-03

- **Dado** petición pública
- **Cuando** se inspecciona la respuesta
- **Entonces** sin datos internos

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| GET | `/health/live` | Pública |
| GET | `/health/ready` | Pública |

## 8. Dependencias y supuestos

- Depende de: 014
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Un readiness agresivo puede provocar reinicios innecesarios

## 10. Preguntas abiertas

- Ninguna.

## 11. Trazabilidad

- Requisitos de este spec: `FR-016-01`, `FR-016-02`, `FR-016-03`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
