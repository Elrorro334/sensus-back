# Spec 019 — Cuarentena de contenido

| Campo | Valor |
|---|---|
| ID | 019 |
| Slug | `cuarentena-contenido` |
| Estado | Pendiente |
| Ola | 4 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 017, 018 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Retener el contenido no aprobado sin publicarlo y con una política de retención explícita.

## 2. Alcance

**Incluye**

- Estado de cuarentena
- Inaccesibilidad pública
- Política de retención y purga

**No incluye**

- Revisión manual en interfaz
- Notificación al usuario

## 3. Actores

- Moderador
- Usuario que publica

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-019-01 | Marcar el contenido no aprobado como PendienteRevision | Must |
| FR-019-02 | Impedir el acceso público al contenido en cuarentena | Must |
| FR-019-03 | Definir y aplicar retención máxima | Must |
| FR-019-04 | Permitir la liberación del contenido tras aprobación | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-019-01 | Retención | Máximo definido en la spec (propuesta: 30 días) |
| NFR-019-02 | Coste de almacenamiento | Dentro del Free Tier de S3 |

## 6. Criterios de aceptación

### CA-019-01

- **Dado** contenido en cuarentena
- **Cuando** se solicita por URL pública
- **Entonces** 403 o 404

### CA-019-02

- **Dado** retención vencida
- **Cuando** se ejecuta la purga
- **Entonces** el contenido se elimina y se registra

### CA-019-03

- **Dado** contenido aprobado
- **Cuando** se libera
- **Entonces** pasa a publicado

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| — | — | — |

## 8. Dependencias y supuestos

- Depende de: 017, 018
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Acumulación de contenido sin purga efectiva

## 10. Preguntas abiertas

- ¿Se notifica al usuario cuando su contenido entra en cuarentena?

## 11. Trazabilidad

- Requisitos de este spec: `FR-019-01`, `FR-019-02`, `FR-019-03`, `FR-019-04`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
