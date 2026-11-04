# Spec 031 — Edición del perfil de comercio

| Campo | Valor |
|---|---|
| ID | 031 |
| Slug | `edicion-perfil-comercio` |
| Estado | Pendiente |
| Ola | 2 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 005, 006 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que el comerciante actualice los datos de su local y recalcule sus coordenadas espaciales sin afectar ofertas ya publicadas.

## 2. Alcance

**Incluye**

- Edición de nombre, categoría, dirección, teléfono y horario
- Recálculo de GeoJSON y geohash
- Validación de categoría y coordenadas
- Auditoría del cambio

**No incluye**

- Cambio de titularidad
- Sucursales
- Verificación documental

## 3. Actores

- Usuario comerciante

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-031-01 | Actualizar los datos editables del comercio propio | Must |
| FR-031-02 | Recalcular location y geohash cuando cambien las coordenadas | Must |
| FR-031-03 | Validar categoría del catálogo y rangos de coordenadas | Must |
| FR-031-04 | Rechazar la edición sobre comercios ajenos con 403 | Must |
| FR-031-05 | Registrar quién cambió qué y cuándo | Should |
| FR-031-06 | Mantener las ofertas publicadas sin cambios de identificador | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-031-01 | Latencia de actualización | p95 < 600 ms |
| NFR-031-02 | Consistencia espacial | 100 % de comercios con location válida |

## 6. Criterios de aceptación

### CA-031-01

- **Dado** datos válidos
- **Cuando** PUT /api/v1/merchants/profile
- **Entonces** 200 con el comercio actualizado

### CA-031-02

- **Dado** coordenadas nuevas
- **Cuando** se actualiza
- **Entonces** location y geohash recalculados

### CA-031-03

- **Dado** comercio ajeno
- **Cuando** se intenta editar
- **Entonces** 403

### CA-031-04

- **Dado** categoría inexistente
- **Cuando** se intenta editar
- **Entonces** 400

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| PUT | `/api/v1/merchants/profile` | JWT |

## 8. Dependencias y supuestos

- Depende de: 005, 006
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Un cambio de ubicación altera la visibilidad de las ofertas sin avisar al comerciante

## 10. Preguntas abiertas

- ¿Se notifica a los usuarios con favoritos cuando el comercio cambia de ubicación?

## 11. Trazabilidad

- Requisitos de este spec: `FR-031-01`, `FR-031-02`, `FR-031-03`, `FR-031-04`, `FR-031-05`, `FR-031-06`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
