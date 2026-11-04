# Spec 033 — Apartado de oferta con código QR

| Campo | Valor |
|---|---|
| ID | 033 |
| Slug | `apartado-con-qr` |
| Estado | Pendiente |
| Ola | 2 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 007, 032, 037 |
| Versión | 1.0.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir que un usuario aparte una unidad de una oferta y reciba un código QR de un solo uso que el comerciante canjea en el local.

## 2. Alcance

**Incluye**

- Apartado con reserva atómica de stock
- Un apartado activo por usuario y oferta
- Generación de código firmado y QR
- Canje por el comerciante
- Expiración a las 24 horas y liberación de stock

**No incluye**

- Pagos
- Envío a domicilio
- Reserva de múltiples unidades
- Cancelación manual por el usuario

## 3. Actores

- Usuario autenticado
- Comerciante
- Proceso de expiración

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-033-01 | Crear un apartado descontando stock de forma atómica | Must |
| FR-033-02 | Rechazar el apartado si no hay stock o la oferta no es visible | Must |
| FR-033-03 | Permitir un solo apartado activo por usuario y oferta | Must |
| FR-033-04 | Generar un código firmado con claimId, offerId y expiración, y su imagen QR | Must |
| FR-033-05 | Caducar el apartado a las 24 horas y devolver el stock | Must |
| FR-033-06 | Exponer POST /api/v1/offers/claims/{code}/redeem para el comerciante propietario | Must |
| FR-033-07 | Marcar el canje como usado y rechazar un segundo canje del mismo código | Must |
| FR-033-08 | Impedir que un comerciante canjee códigos de otro comercio | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-033-01 | Latencia del apartado | p95 < 700 ms |
| NFR-033-02 | Consistencia de stock | Cero unidades negativas bajo concurrencia |
| NFR-033-03 | Liberación de stock | ≤ 5 min tras la expiración |

## 6. Criterios de aceptación

### CA-033-01

- **Dado** stock disponible y sin apartado previo
- **Cuando** POST /api/v1/offers/{id}/claim
- **Entonces** 201 con código, QR y expiresAt

### CA-033-02

- **Dado** dos usuarios compiten por la última unidad
- **Cuando** se procesan en paralelo
- **Entonces** solo uno recibe 201 y el otro 409

### CA-033-03

- **Dado** código válido del propio comercio
- **Cuando** POST /api/v1/offers/claims/{code}/redeem
- **Entonces** 200 con el apartado canjeado

### CA-033-04

- **Dado** código ya canjeado
- **Cuando** se reintenta el canje
- **Entonces** 409 sin efectos

### CA-033-05

- **Dado** apartado vencido
- **Cuando** se ejecuta la liberación
- **Entonces** stock devuelto y código inválido

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/offers/{id}/claim` | JWT |
| POST | `/api/v1/offers/claims/{code}/redeem` | JWT |

## 8. Dependencias y supuestos

- Depende de: 007, 032, 037
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Una condición de carrera puede vender stock inexistente
- Un QR filtrado permite el canje si no se valida la propiedad

## 10. Preguntas abiertas

- ¿El QR se devuelve como imagen PNG o solo como payload para que el cliente lo renderice?

## 11. Trazabilidad

- Requisitos de este spec: `FR-033-01`, `FR-033-02`, `FR-033-03`, `FR-033-04`, `FR-033-05`, `FR-033-06`, `FR-033-07`, `FR-033-08`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
