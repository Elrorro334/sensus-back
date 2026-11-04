# Spec 034 — Creación de oferta

| Campo | Valor |
|---|---|
| ID | 034 |
| Slug | `creacion-oferta` |
| Estado | Pendiente |
| Ola | 2 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 005, 017, 022 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que un comerciante publique una oferta con precio, descuento, stock, vigencia e imágenes, sujeta a moderación previa.

## 2. Alcance

**Incluye**

- Alta de oferta con validaciones de negocio
- Moderación de texto e imágenes antes de publicar
- Estado inicial Draft o Published
- Límite de ofertas activas por comercio
- Cálculo del precio con descuento

**No incluye**

- Edición (spec 035)
- Cambio de estado (spec 036)
- Duplicado de ofertas
- Pagos

## 3. Actores

- Usuario comerciante
- Servicio de moderación

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-034-01 | Crear una oferta asociada al comercio del token | Must |
| FR-034-02 | Validar precio, descuento entre 1 y 90 %, stock ≥ 1 y vigencia coherente | Must |
| FR-034-03 | Moderar texto e imágenes antes de publicar | Must |
| FR-034-04 | Permitir guardar como Draft sin moderación y sin visibilidad pública | Must |
| FR-034-05 | Limitar a 20 ofertas activas por comercio | Should |
| FR-034-06 | Calcular y persistir el precio final a partir del descuento | Must |
| FR-034-07 | Devolver 202 cuando la moderación quede pendiente | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-034-01 | Latencia de creación | p95 < 1 s sin moderación |
| NFR-034-02 | Publicaciones sin veredicto | Cero |

## 6. Criterios de aceptación

### CA-034-01

- **Dado** datos válidos, sin imágenes
- **Cuando** POST /api/v1/offers
- **Entonces** 201 en estado Published

### CA-034-02

- **Dado** imagen pendiente de moderación
- **Cuando** POST /api/v1/offers
- **Entonces** 202 y oferta oculta hasta el veredicto

### CA-034-03

- **Dado** descuento de 95 %
- **Cuando** se intenta crear
- **Entonces** 400

### CA-034-04

- **Dado** vigencia con fin anterior al inicio
- **Cuando** se intenta crear
- **Entonces** 400

### CA-034-05

- **Dado** comercio con 20 ofertas activas
- **Cuando** se intenta crear otra
- **Entonces** 409

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/offers` | JWT |

## 8. Dependencias y supuestos

- Depende de: 005, 017, 022
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Publicar antes del veredicto si se invierte el orden de las operaciones
- Un límite de ofertas mal calibrado bloquea comercios legítimos

## 10. Preguntas abiertas

- ¿El descuento se expresa como porcentaje o como precio final?

## 11. Trazabilidad

- Requisitos de este spec: `FR-034-01`, `FR-034-02`, `FR-034-03`, `FR-034-04`, `FR-034-05`, `FR-034-06`, `FR-034-07`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
