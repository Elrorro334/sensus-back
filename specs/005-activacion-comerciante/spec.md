# Spec 005 — Activación del rol comercial

| Campo | Valor |
|---|---|
| ID | 005 |
| Slug | `activacion-comerciante` |
| Estado | Implementado (retro-spec) |
| Ola | 2 |
| Tipo | Retro-spec |
| Dueño | Por asignar |
| Dependencias | 001, 002 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que un usuario con cuenta unificada active el rol comercial creando el perfil de su local con categoría, dirección y coordenadas validadas.

## 2. Alcance

**Incluye**

- Alta del perfil comercial sobre una cuenta existente
- Categoría del catálogo
- Dirección y coordenadas aportadas por el cliente
- Cálculo de GeoJSON y geohash
- Un comercio activo por usuario
- Asignación del rol Merchant

**No incluye**

- Verificación documental del comercio
- Sucursales múltiples
- Edición posterior (spec 031)
- Geocodificación con proveedor externo

## 3. Actores

- Usuario autenticado
- Operador

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-005-01 | Activar el rol comercial sobre la cuenta autenticada | Must |
| FR-005-02 | Registrar nombre, categoría, dirección y coordenadas del local | Must |
| FR-005-03 | Permitir un solo comercio activo por usuario | Must |
| FR-005-04 | Asignar el rol Merchant tras la activación | Must |
| FR-005-05 | Validar que la categoría pertenezca al catálogo | Must |
| FR-005-06 | Validar rangos de latitud y longitud y calcular el punto GeoJSON | Must |
| FR-005-07 | Rechazar la activación de cuentas suspendidas | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-005-01 | Latencia de activación | p95 < 800 ms |
| NFR-005-02 | Precisión de coordenadas | 6 decimales, error < 1 m |

## 6. Criterios de aceptación

### CA-005-01

- **Dado** datos válidos y usuario sin comercio
- **Cuando** POST /api/v1/merchants/activate
- **Entonces** 201 con el comercio y el rol Merchant

### CA-005-02

- **Dado** usuario con comercio activo
- **Cuando** se intenta activar otro
- **Entonces** 409

### CA-005-03

- **Dado** coordenadas fuera de rango
- **Cuando** se intenta activar
- **Entonces** 400 sin crear el comercio

### CA-005-04

- **Dado** cuenta suspendida
- **Cuando** se intenta activar
- **Entonces** 403

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/merchants/activate` | JWT |

## 8. Dependencias y supuestos

- Depende de: 001, 002
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Coordenadas enviadas por el cliente pueden ser imprecisas
- El índice geoespacial no está probado contra MongoDB real

## 10. Preguntas abiertas

- ¿Se exigirá verificación documental del comercio en una fase posterior?

## 11. Trazabilidad

- Requisitos de este spec: `FR-005-01`, `FR-005-02`, `FR-005-03`, `FR-005-04`, `FR-005-05`, `FR-005-06`, `FR-005-07`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
