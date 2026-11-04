# Spec 023 — Entrega de contenido por CDN

| Campo | Valor |
|---|---|
| ID | 023 |
| Slug | `entrega-cdn` |
| Estado | Pendiente |
| Ola | 4 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 022 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Servir el contenido aprobado a través de CloudFront con caché y control de acceso.

## 2. Alcance

**Incluye**

- Distribución CloudFront
- URLs firmadas para contenido privado
- Cabeceras de caché

**No incluye**

- Optimización de imágenes
- Streaming de vídeo

## 3. Actores

- Cliente móvil
- Cliente web

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-023-01 | Servir multimedia mediante CloudFront | Must |
| FR-023-02 | Firmar URLs cuando el contenido sea privado | Must |
| FR-023-03 | Aplicar cabeceras de caché por tipo de contenido | Should |
| FR-023-04 | Invalidar caché al retirar contenido | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-023-01 | Latencia de entrega | p95 < 200 ms en caché |
| NFR-023-02 | Coste | Dentro del Free Tier de CloudFront |

## 6. Criterios de aceptación

### CA-023-01

- **Dado** contenido público aprobado
- **Cuando** se solicita la URL
- **Entonces** 200 desde CloudFront

### CA-023-02

- **Dado** URL sin firma para contenido privado
- **Cuando** se solicita
- **Entonces** 403

### CA-023-03

- **Dado** contenido retirado
- **Cuando** se solicita la URL
- **Entonces** 404 tras invalidación

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| — | — | — |

## 8. Dependencias y supuestos

- Depende de: 022
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Costo por transferencia si el contenido se comparte masivamente

## 10. Preguntas abiertas

- ¿Todo el contenido requiere URL firmada?

## 11. Trazabilidad

- Requisitos de este spec: `FR-023-01`, `FR-023-02`, `FR-023-03`, `FR-023-04`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
