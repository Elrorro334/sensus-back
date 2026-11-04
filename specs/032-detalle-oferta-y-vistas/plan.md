# Plan 032 — Detalle de oferta y contador de vistas

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir consultar una oferta completa y registrar una vista por usuario y día, para alimentar la popularidad usada por el ranking.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Application | GetOfferDetailQuery y RegisterOfferViewCommand |
| Infrastructure | Colección offer_views con índice único userId + offerId + dayOfYear |
| Api | Endpoints de detalle y vista |

## 3. Modelo de datos e índices

- offers; offer_views con índice único compuesto y TTL de 180 días

## 4. Seguridad

- Sin datos internos del comercio
- Deduplicación que impide inflar el contador

## 5. Observabilidad y rendimiento

- Métricas de vistas por oferta
- Alerta ante picos anómalos de vistas

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de deduplicación por día
- Integración de 200, 404 y 204 repetido

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-032-01` y 5 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- El contador de vistas puede inflarse con cuentas nuevas
- La escritura de vistas compite con la lectura del detalle

## 10. Orden de implementación

1. `T032-01` Actualizar y validar el contrato OpenAPI del spec
2. `T032-02` Implementar: GetOfferDetailQuery y RegisterOfferViewCommand
3. `T032-03` Implementar: Colección offer_views con índice único userId + offerId + dayOfYear
4. `T032-04` Implementar: Endpoints de detalle y vista
5. `T032-05` Modelar la vista y su clave de deduplicación
6. `T032-06` Crear el índice único de vistas y su TTL
7. `T032-07` Devolver 404 uniforme para ofertas no visibles
8. `T032-08` Implementar las pruebas de integración del flujo completo
9. `T032-09` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
10. `T032-10` Registrar evidencias del Constitution Check
