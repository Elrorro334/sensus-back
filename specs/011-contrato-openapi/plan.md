# Plan 011 — Contrato OpenAPI

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS CON OBSERVACIONES |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Publicar y validar un contrato OpenAPI que refleje exactamente los endpoints y modelos expuestos.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Api | Configuración de OpenAPI y anotaciones |
| CI | Paso de validación del documento |

## 3. Modelo de datos e índices

- Sin cambios de datos

## 4. Seguridad

- Documentar el esquema Bearer y los endpoints protegidos

## 5. Observabilidad y rendimiento

- Publicar la versión del contrato en el pipeline

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Validación del documento en CI
- Verificación de endpoints sin documentar

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-011-01` y 3 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PARCIAL | falta validación automatizada del contrato |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Duplicar Swashbuckle y Microsoft.AspNetCore.OpenApi puede generar dos documentos distintos

## 10. Orden de implementación

1. `T011-01` Implementar: Configuración de OpenAPI y anotaciones
2. `T011-02` Implementar: Paso de validación del documento
3. `T011-03` Unificar la generación de OpenAPI en una sola fuente
4. `T011-04` Añadir validación del contrato al pipeline
5. `T011-05` Implementar las pruebas de integración del flujo completo
6. `T011-06` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
7. `T011-07` Registrar evidencias del Constitution Check
