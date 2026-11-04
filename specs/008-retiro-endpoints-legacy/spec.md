# Spec 008 — Retiro de endpoints legacy

| Campo | Valor |
|---|---|
| ID | 008 |
| Slug | `retiro-endpoints-legacy` |
| Estado | Pendiente |
| Ola | 6 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 001, 005, 006 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Eliminar las rutas duplicadas en español (/comercio/* y /comercios) para dejar un único contrato público versionado en inglés.

## 2. Alcance

**Incluye**

- Eliminación de rutas legacy
- Eliminación de alias en español
- Actualización de documentación y archivo .http
- Verificación de ausencia de referencias

**No incluye**

- Redirecciones temporales
- Versionado adicional
- Clientes en producción

## 3. Actores

- Equipo de desarrollo
- Cliente móvil

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-008-01 | Eliminar el grupo de rutas /api/v1/comercio | Must |
| FR-008-02 | Eliminar el alias /api/v1/comercios | Must |
| FR-008-03 | Devolver 404 en las rutas retiradas | Must |
| FR-008-04 | Actualizar README, Sensus.Api.http y OpenAPI | Must |
| FR-008-05 | No dejar referencias a rutas en español en el código | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-008-01 | Rutas duplicadas | Cero en el contrato OpenAPI |
| NFR-008-02 | Cobertura del contrato | 100 % de rutas bajo /api/v1 en inglés |

## 6. Criterios de aceptación

### CA-008-01

- **Dado** ruta legacy retirada
- **Cuando** se invoca /api/v1/comercio/registro
- **Entonces** 404

### CA-008-02

- **Dado** alias retirado
- **Cuando** se invoca /api/v1/comercios
- **Entonces** 404

### CA-008-03

- **Dado** búsqueda en el código
- **Cuando** se revisan referencias
- **Entonces** sin coincidencias

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| — | — | — |

## 8. Dependencias y supuestos

- Depende de: 001, 005, 006
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Clientes móviles antiguos aún pueden consumir las rutas retiradas

## 10. Preguntas abiertas

- ¿Existen compilaciones móviles en uso con las rutas en español?

## 11. Trazabilidad

- Requisitos de este spec: `FR-008-01`, `FR-008-02`, `FR-008-03`, `FR-008-04`, `FR-008-05`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
