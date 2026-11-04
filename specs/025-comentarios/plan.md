# Plan 025 — Comentarios del feed

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir comentar publicaciones visibles con moderación de texto y listado paginado.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Entidad Comment |
| Application | Comandos y consultas paginadas |
| Infrastructure | Persistencia con índice por postId y fecha |
| Api | Endpoints de comentarios |

## 3. Modelo de datos e índices

- comments con índice compuesto postId + createdAt

## 4. Seguridad

- Autor desde el token
- Moderación previa
- Sin comentarios en publicaciones ocultas

## 5. Observabilidad y rendimiento

- Métricas de comentarios y rechazos

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de moderación de texto
- Integración de paginación y 404

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-025-01` y 5 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Hilos largos degradan la consulta sin índice adecuado

## 10. Orden de implementación

1. `T025-01` Actualizar y validar el contrato OpenAPI del spec
2. `T025-02` Implementar: Entidad Comment
3. `T025-03` Implementar: Comandos y consultas paginadas
4. `T025-04` Implementar: Persistencia con índice por postId y fecha
5. `T025-05` Implementar: Endpoints de comentarios
6. `T025-06` Modelar Comment
7. `T025-07` Crear índice compuesto postId + createdAt
8. `T025-08` Probar el 404 en publicaciones no visibles
9. `T025-09` Implementar las pruebas de integración del flujo completo
10. `T025-10` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
11. `T025-11` Registrar evidencias del Constitution Check
