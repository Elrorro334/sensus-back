# Plan 024 — Publicaciones del feed

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir crear y eliminar publicaciones con texto e imagen opcional, sujetas a moderación previa y con borrado por el autor.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Entidad Post con estados y regla de autoría |
| Application | CreatePostCommand, DeletePostCommand y orquestación de moderación |
| Infrastructure | Persistencia y referencia al archivo |
| Api | Endpoints de creación y eliminación |

## 3. Modelo de datos e índices

- posts con índice por authorId + createdAt y por status + createdAt

## 4. Seguridad

- Autor tomado del token
- Moderación obligatoria antes de exponer
- Borrado autorizado por rol o autoría

## 5. Observabilidad y rendimiento

- Métricas de publicaciones creadas, rechazadas y eliminadas

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Unitarias de estados
- Integración de flujo con moderación simulada y borrado

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-024-01` y 6 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Publicación visible antes del veredicto si se invierte el orden
- El borrado lógico puede dejar imágenes accesibles por URL

## 10. Orden de implementación

1. `T024-01` Actualizar y validar el contrato OpenAPI del spec
2. `T024-02` Implementar: Entidad Post con estados y regla de autoría
3. `T024-03` Implementar: CreatePostCommand, DeletePostCommand y orquestación de moderación
4. `T024-04` Implementar: Persistencia y referencia al archivo
5. `T024-05` Implementar: Endpoints de creación y eliminación
6. `T024-06` Modelar estados de publicación
7. `T024-07` Encadenar moderación y publicación
8. `T024-08` Autorizar el borrado por autoría o rol
9. `T024-09` Implementar las pruebas de integración del flujo completo
10. `T024-10` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
11. `T024-11` Registrar evidencias del Constitution Check
