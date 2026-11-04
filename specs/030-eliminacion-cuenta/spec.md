# Spec 030 — Eliminación de cuenta y datos

| Campo | Valor |
|---|---|
| ID | 030 |
| Slug | `eliminacion-cuenta` |
| Estado | Pendiente |
| Ola | 1 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 003, 004, 020 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que el titular elimine su cuenta con confirmación explícita, aplicando una retención declarada y purgando sus datos personales en cascada.

## 2. Alcance

**Incluye**

- Solicitud de eliminación autenticada con confirmación
- Borrado lógico inmediato y bloqueo de acceso
- Purga de PII a los 30 días
- Anonimización de contenido público
- Desactivación de ofertas y comercio
- Revocación de sesiones

**No incluye**

- Exportación de datos previa
- Recuperación de la cuenta tras la purga
- Eliminación de registros de auditoría antes de 90 días

## 3. Actores

- Usuario autenticado
- Operador
- Proceso de purga

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-030-01 | Exponer DELETE /api/v1/users/account con confirmación explícita y contraseña | Must |
| FR-030-02 | Marcar la cuenta como deleted y revocar todas sus sesiones de inmediato | Must |
| FR-030-03 | Impedir el login y cualquier operación autenticada tras la eliminación | Must |
| FR-030-04 | Purgar los datos personales a los 30 días mediante proceso programado | Must |
| FR-030-05 | Anonimizar publicaciones y comentarios del usuario en lugar de conservarlos asociados | Must |
| FR-030-06 | Desactivar el comercio y sus ofertas activas al eliminar una cuenta comercial | Must |
| FR-030-07 | Conservar la auditoría de moderación anonimizada durante 90 días | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-030-01 | Efectividad del bloqueo | < 1 s desde la eliminación |
| NFR-030-02 | Purga | 100 % de cuentas marcadas, en ≤ 31 días |
| NFR-030-03 | Trazabilidad | Toda eliminación registrada sin PII |

## 6. Criterios de aceptación

### CA-030-01

- **Dado** contraseña correcta y confirmación explícita
- **Cuando** DELETE /api/v1/users/account
- **Entonces** 202 y cuenta bloqueada

### CA-030-02

- **Dado** contraseña incorrecta
- **Cuando** DELETE /api/v1/users/account
- **Entonces** 401 sin eliminar

### CA-030-03

- **Dado** cuenta eliminada
- **Cuando** se intenta login
- **Entonces** 401 genérico

### CA-030-04

- **Dado** purga ejecutada
- **Cuando** se inspeccionan los datos
- **Entonces** sin PII del titular y contenido anonimizado

### CA-030-05

- **Dado** cuenta comercial eliminada
- **Cuando** se consultan sus ofertas
- **Entonces** ofertas desactivadas y no visibles

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| DELETE | `/api/v1/users/account` | JWT |

## 8. Dependencias y supuestos

- Depende de: 003, 004, 020
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Una purga incompleta deja datos personales retenidos
- Eliminar una cuenta comercial oculta ofertas con apartados activos

## 10. Preguntas abiertas

- ¿Se notifica por correo la eliminación completada?

## 11. Trazabilidad

- Requisitos de este spec: `FR-030-01`, `FR-030-02`, `FR-030-03`, `FR-030-04`, `FR-030-05`, `FR-030-06`, `FR-030-07`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
