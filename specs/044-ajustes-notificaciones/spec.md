# Spec 044 — Ajustes de notificaciones

| Campo | Valor |
|---|---|
| ID | 044 |
| Slug | `ajustes-notificaciones` |
| Estado | Pendiente |
| Ola | 4 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 027, 003 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que el usuario configure qué notificaciones recibe, con lectura y actualización de sus preferencias y respeto estricto en el despacho.

## 2. Alcance

**Incluye**

- Lectura de preferencias
- Actualización de push activo, avisos de ofertas y avisos sociales
- Radio de interés
- Aplicación de las preferencias en el despacho

**No incluye**

- Programación de horarios silenciosos
- Plantillas de notificación
- Campañas

## 3. Actores

- Usuario autenticado
- Servicio de push

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-044-01 | Devolver las preferencias de notificación del usuario | Must |
| FR-044-02 | Actualizar pushEnabled, offerAlerts, socialAlerts y radiusKm | Must |
| FR-044-03 | Validar radiusKm entre 1 y 50 | Must |
| FR-044-04 | Respetar pushEnabled y las categorías al despachar | Must |
| FR-044-05 | Aplicar valores por defecto cuando el usuario no tiene preferencias | Must |
| FR-044-06 | Devolver 200 con el estado resultante tras la actualización | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-044-01 | Latencia de lectura | p95 < 250 ms |
| NFR-044-02 | Respeto de preferencias | 100 % de envíos filtrados según configuración |

## 6. Criterios de aceptación

### CA-044-01

- **Dado** usuario sin preferencias
- **Cuando** GET /api/v1/users/settings
- **Entonces** 200 con valores por defecto

### CA-044-02

- **Dado** pushEnabled=false
- **Cuando** se actualiza y se despacha una oferta
- **Entonces** no se envía notificación

### CA-044-03

- **Dado** radiusKm=0
- **Cuando** PUT /api/v1/users/settings
- **Entonces** 400

### CA-044-04

- **Dado** offerAlerts=false
- **Cuando** se despacha un aviso de oferta
- **Entonces** no se envía

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| GET | `/api/v1/users/settings` | JWT |
| PUT | `/api/v1/users/settings` | JWT |

## 8. Dependencias y supuestos

- Depende de: 027, 003
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Ignorar las preferencias en el despacho genera reportes de abuso y desinstalaciones

## 10. Preguntas abiertas

- ¿El catálogo necesita también un GET? Se incluye por obligación de lectura.

## 11. Trazabilidad

- Requisitos de este spec: `FR-044-01`, `FR-044-02`, `FR-044-03`, `FR-044-04`, `FR-044-05`, `FR-044-06`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
