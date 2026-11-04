# Spec 040 — Reportes de contenido

| Campo | Valor |
|---|---|
| ID | 040 |
| Slug | `reportes-contenido` |
| Estado | Pendiente |
| Ola | 3 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 024, 025, 017, 018 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que un usuario reporte ofertas, publicaciones o comentarios, activando la revisión del contenido sin persistir material de explotación infantil.

## 2. Alcance

**Incluye**

- Alta de reporte con motivo tipificado
- Deduplicación por usuario y objetivo
- Marcado del contenido para revisión
- Exclusión legal de material de explotación infantil

**No incluye**

- Apelaciones
- Comunicación directa con el reportante
- Eliminación automática por un solo reporte

## 3. Actores

- Usuario autenticado
- Moderador

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-040-01 | Crear un reporte sobre una oferta, publicación o comentario existente | Must |
| FR-040-02 | Aceptar solo motivos tipificados: spam, ofensivo, fraude u otro | Must |
| FR-040-03 | Limitar los detalles a 500 caracteres | Must |
| FR-040-04 | Permitir un solo reporte por usuario y objetivo cada 24 horas | Must |
| FR-040-05 | Marcar el contenido para revisión al alcanzar 3 reportes en 24 horas | Must |
| FR-040-06 | Rechazar y no persistir reportes de material de explotación infantil | Must |
| FR-040-07 | Impedir que el autor reporte su propio contenido | Should |
| FR-040-08 | No revelar al reportado la identidad del reportante | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-040-01 | Latencia de creación | p95 < 400 ms |
| NFR-040-02 | Privacidad | Cero exposición de la identidad del reportante |

## 6. Criterios de aceptación

### CA-040-01

- **Dado** reporte válido sobre publicación visible
- **Cuando** POST /api/v1/reports
- **Entonces** 201 con estado registrado

### CA-040-02

- **Dado** segundo reporte del mismo usuario en 24 h
- **Cuando** POST /api/v1/reports
- **Entonces** 409

### CA-040-03

- **Dado** reporte con material de explotación infantil
- **Cuando** se envía
- **Entonces** 400, sin persistencia y con registro legal mínimo

### CA-040-04

- **Dado** tercer reporte en 24 h
- **Cuando** se procesa
- **Entonces** el contenido pasa a revisión

### CA-040-05

- **Dado** autor reporta su propio contenido
- **Cuando** se envía
- **Entonces** 400

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/reports` | JWT |

## 8. Dependencias y supuestos

- Depende de: 024, 025, 017, 018
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Reportes coordinados pueden ocultar contenido legítimo
- Un reporte legal mal clasificado podría persistirse por error

## 10. Preguntas abiertas

- ¿Quién revisa el contenido marcado y en qué plazo?

## 11. Trazabilidad

- Requisitos de este spec: `FR-040-01`, `FR-040-02`, `FR-040-03`, `FR-040-04`, `FR-040-05`, `FR-040-06`, `FR-040-07`, `FR-040-08`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
