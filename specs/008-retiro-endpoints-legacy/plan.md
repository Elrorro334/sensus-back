# Plan 008 — Retiro de endpoints legacy

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Eliminar las rutas duplicadas en español (/comercio/* y /comercios) para dejar un único contrato público versionado en inglés.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Api | Eliminar MapGroup y endpoints legacy |
| Documentación | Actualizar README, .http y OpenAPI |

## 3. Modelo de datos e índices

- Sin cambios de datos

## 4. Seguridad

- Reducir superficie de ataque eliminando rutas no mantenidas

## 5. Observabilidad y rendimiento

- Alerta de 404 en rutas legacy durante la transición

## 6. Costo AWS

Sin costo.

## 7. Estrategia de pruebas

- Prueba de que las rutas legacy devuelven 404

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-008-01` y 4 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Clientes móviles antiguos aún pueden consumir las rutas retiradas

## 10. Orden de implementación

1. `T008-01` Implementar: Eliminar MapGroup y endpoints legacy
2. `T008-02` Implementar: Actualizar README, .http y OpenAPI
3. `T008-03` Eliminar el grupo legacy y sus handlers
4. `T008-04` Actualizar README y Sensus.Api.http
5. `T008-05` Implementar las pruebas de integración del flujo completo
6. `T008-06` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
7. `T008-07` Registrar evidencias del Constitution Check
