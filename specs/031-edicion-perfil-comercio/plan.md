# Plan 031 — Edición del perfil de comercio

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que el comerciante actualice los datos de su local y recalcule sus coordenadas espaciales sin afectar ofertas ya publicadas.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Invariantes de Merchant y recálculo de ubicación |
| Application | UpdateMerchantProfileCommand y validador |
| Infrastructure | Actualización atómica y recálculo de geohash |
| Api | Endpoint PUT /api/v1/merchants/profile |

## 3. Modelo de datos e índices

- merchants con location 2dsphere y geohash; audit de cambios

## 4. Seguridad

- Autorización por propietario
- El userId proviene del token
- Sin exposición de datos de contacto internos

## 5. Observabilidad y rendimiento

- Métricas de ediciones y conflictos
- Log de cambio sin PII

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de recálculo
- Integración de edición, 403 y validación de categoría

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-031-01` y 5 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Un cambio de ubicación altera la visibilidad de las ofertas sin avisar al comerciante

## 10. Orden de implementación

1. `T031-01` Actualizar y validar el contrato OpenAPI del spec
2. `T031-02` Implementar: Invariantes de Merchant y recálculo de ubicación
3. `T031-03` Implementar: UpdateMerchantProfileCommand y validador
4. `T031-04` Implementar: Actualización atómica y recálculo de geohash
5. `T031-05` Implementar: Endpoint PUT /api/v1/merchants/profile
6. `T031-06` Implementar el recálculo de location y geohash
7. `T031-07` Validar categoría y coordenadas
8. `T031-08` Probar el rechazo de edición sobre comercios ajenos
9. `T031-09` Implementar las pruebas de integración del flujo completo
10. `T031-10` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
11. `T031-11` Registrar evidencias del Constitution Check
