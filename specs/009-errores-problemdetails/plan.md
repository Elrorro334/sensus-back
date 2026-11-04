# Plan 009 — Errores uniformes con ProblemDetails

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS CON OBSERVACIONES |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Garantizar que toda respuesta de error use ProblemDetails con un identificador de correlación y sin filtrar detalles internos.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Api | GlobalExceptionHandler y middleware de correlación |
| Application | Excepciones de dominio mapeadas a códigos |
| Domain | Sin dependencias de presentación |

## 3. Modelo de datos e índices

- Sin cambios de datos

## 4. Seguridad

- No filtrar rutas internas, versiones ni detalles de infraestructura

## 5. Observabilidad y rendimiento

- Cada error registra correlationId y código interno

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias del mapeo de excepciones
- Integración de 400, 401, 404, 409 y 500

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-009-01` y 3 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PARCIAL | falta correlation-id en todas las respuestas |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Cambiar el formato rompe clientes que parsean el cuerpo actual

## 10. Orden de implementación

1. `T009-01` Implementar: GlobalExceptionHandler y middleware de correlación
2. `T009-02` Implementar: Excepciones de dominio mapeadas a códigos
3. `T009-03` Implementar: Sin dependencias de presentación
4. `T009-04` Unificar el mapeo de Result y excepciones a ProblemDetails
5. `T009-05` Añadir correlationId a todas las respuestas de error
6. `T009-06` Implementar las pruebas de integración del flujo completo
7. `T009-07` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
8. `T009-08` Registrar evidencias del Constitution Check
