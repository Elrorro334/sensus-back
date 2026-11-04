# Plan 020 — Sanciones y baneo de usuario

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Aplicar sanciones proporcionales y reversibles ante contenido de riesgo alto.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Entidad Sancion con reglas de vigencia |
| Application | Casos de uso de aplicar y revertir |
| Infrastructure | Persistencia y consulta en autenticación |
| Api | Endpoints administrativos de sanción |

## 3. Modelo de datos e índices

- sanciones con índice por usuarioId y vigencia

## 4. Seguridad

- Solo administradores pueden revertir
- Auditoría obligatoria

## 5. Observabilidad y rendimiento

- Métricas de sanciones aplicadas y revertidas

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de vigencia
- Integración de bloqueo y reversión

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-020-01` y 3 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Un falso positivo de moderación sanciona a un usuario inocente

## 10. Orden de implementación

1. `T020-01` Implementar: Entidad Sancion con reglas de vigencia
2. `T020-02` Implementar: Casos de uso de aplicar y revertir
3. `T020-03` Implementar: Persistencia y consulta en autenticación
4. `T020-04` Implementar: Endpoints administrativos de sanción
5. `T020-05` Modelar sanción y vigencia
6. `T020-06` Implementar reversión con auditoría
7. `T020-07` Implementar las pruebas de integración del flujo completo
8. `T020-08` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
9. `T020-09` Registrar evidencias del Constitution Check
