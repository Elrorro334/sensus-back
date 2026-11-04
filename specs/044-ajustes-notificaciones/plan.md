# Plan 044 — Ajustes de notificaciones

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir que el usuario configure qué notificaciones recibe, con lectura y actualización de sus preferencias y respeto estricto en el despacho.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Application | GetNotificationSettingsQuery y UpdateNotificationSettingsCommand |
| Infrastructure | Persistencia de preferencias y filtro en el despacho |
| Api | Endpoints GET y PUT /api/v1/users/settings |

## 3. Modelo de datos e índices

- notification_settings con índice único userId

## 4. Seguridad

- Preferencias del propio usuario
- Sin datos sensibles en la respuesta

## 5. Observabilidad y rendimiento

- Métricas de actualizaciones y de envíos filtrados

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de validación y valores por defecto
- Integración de filtrado en el despacho

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-044-01` y 5 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Ignorar las preferencias en el despacho genera reportes de abuso y desinstalaciones

## 10. Orden de implementación

1. `T044-01` Actualizar y validar el contrato OpenAPI del spec
2. `T044-02` Implementar: GetNotificationSettingsQuery y UpdateNotificationSettingsCommand
3. `T044-03` Implementar: Persistencia de preferencias y filtro en el despacho
4. `T044-04` Implementar: Endpoints GET y PUT /api/v1/users/settings
5. `T044-05` Implementar valores por defecto
6. `T044-06` Aplicar el filtro de preferencias en el despacho
7. `T044-07` Probar el límite de radio y la actualización
8. `T044-08` Implementar las pruebas de integración del flujo completo
9. `T044-09` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
10. `T044-10` Registrar evidencias del Constitution Check
