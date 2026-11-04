# Plan 021 — Auditoría de moderación

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Conservar evidencia inmutable de cada decisión de moderación para revisión y defensa legal.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Modelo de registro de auditoría |
| Application | Consulta paginada |
| Infrastructure | Colección append-only |
| Api | Endpoint administrativo |

## 3. Modelo de datos e índices

- moderation_audit append-only con índice por fecha y usuario

## 4. Seguridad

- Acceso restringido a administradores
- Sin datos personales innecesarios

## 5. Observabilidad y rendimiento

- Métricas de consultas de auditoría

## 6. Costo AWS

Almacenamiento en MongoDB; vigilar crecimiento.

## 7. Estrategia de pruebas

- Unitarias de consulta
- Integración de permisos y paginación

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-021-01` y 3 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Crecimiento indefinido de la colección

## 10. Orden de implementación

1. `T021-01` Actualizar y validar el contrato OpenAPI del spec
2. `T021-02` Implementar: Modelo de registro de auditoría
3. `T021-03` Implementar: Consulta paginada
4. `T021-04` Implementar: Colección append-only
5. `T021-05` Implementar: Endpoint administrativo
6. `T021-06` Crear colección append-only con índices
7. `T021-07` Restringir el endpoint al rol Admin
8. `T021-08` Implementar las pruebas de integración del flujo completo
9. `T021-09` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
10. `T021-10` Registrar evidencias del Constitution Check
