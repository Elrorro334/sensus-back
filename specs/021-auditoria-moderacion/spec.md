# Spec 021 — Auditoría de moderación

| Campo | Valor |
|---|---|
| ID | 021 |
| Slug | `auditoria-moderacion` |
| Estado | Pendiente |
| Ola | 4 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 017, 020 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Conservar evidencia inmutable de cada decisión de moderación para revisión y defensa legal.

## 2. Alcance

**Incluye**

- Registro inmutable de decisiones
- Consulta paginada para administradores
- Retención definida

**No incluye**

- Interfaz de revisión
- Exportación masiva

## 3. Actores

- Administrador
- Auditor

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-021-01 | Registrar cada decisión con veredicto, puntajes y umbral | Must |
| FR-021-02 | Impedir la modificación de registros de auditoría | Must |
| FR-021-03 | Permitir consulta paginada por administrador | Must |
| FR-021-04 | Definir retención conforme a la política de datos | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-021-01 | Integridad | Registros sin actualización ni borrado manual |
| NFR-021-02 | Latencia de consulta | p95 < 800 ms |

## 6. Criterios de aceptación

### CA-021-01

- **Dado** decisión adoptada
- **Cuando** se consulta la auditoría
- **Entonces** el registro existe con todos los campos

### CA-021-02

- **Dado** intento de modificar un registro
- **Cuando** se ejecuta la operación
- **Entonces** la operación es rechazada

### CA-021-03

- **Dado** consulta sin permisos
- **Cuando** se invoca el endpoint
- **Entonces** 403

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| GET | `/api/v1/admin/moderation/audit` | JWT + rol Admin |

## 8. Dependencias y supuestos

- Depende de: 017, 020
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Crecimiento indefinido de la colección

## 10. Preguntas abiertas

- ¿Cuánto tiempo debe conservarse la evidencia?

## 11. Trazabilidad

- Requisitos de este spec: `FR-021-01`, `FR-021-02`, `FR-021-03`, `FR-021-04`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
