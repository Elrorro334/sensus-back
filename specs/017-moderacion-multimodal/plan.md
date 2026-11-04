# Plan 017 — Moderación multimodal de contenido

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Validar imagen y texto antes de publicar ofertas, publicaciones o comentarios, usando omni-moderation-latest con comportamiento fail-closed.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Domain | Contrato IModerationService y modelo de veredicto |
| Application | Caso de uso de moderación previa a la publicación |
| Infrastructure | Cliente de OpenAI con timeout, reintentos y mapeo de respuesta |
| Api | Integración en los flujos de oferta, publicación y comentario |

## 3. Modelo de datos e índices

- moderation_decisions con veredicto, puntajes, umbral y acción

## 4. Seguridad

- Clave del proveedor en Secrets Manager
- No registrar el contenido moderado
- Timeout obligatorio
- Sin endpoint público que permita saltarse el flujo

## 5. Observabilidad y rendimiento

- Métricas de latencia y de fallos del proveedor
- Log de decisión sin contenido

## 6. Costo AWS

API de moderación sin costo por token; validar límites de uso.

## 7. Estrategia de pruebas

- Dobles de fallo, timeout y respuesta malformada
- Integración con stub del proveedor

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-017-01` y 6 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- Dependencia externa sin SLA
- Coste oculto si el proveedor cambia de política

## 10. Orden de implementación

1. `T017-01` Implementar: Contrato IModerationService y modelo de veredicto
2. `T017-02` Implementar: Caso de uso de moderación previa a la publicación
3. `T017-03` Implementar: Cliente de OpenAI con timeout, reintentos y mapeo de respuesta
4. `T017-04` Implementar: Integración en los flujos de oferta, publicación y comentario
5. `T017-05` Definir IModerationService y el modelo de veredicto
6. `T017-06` Implementar el cliente con timeout y fail-closed
7. `T017-07` Registrar la decisión completa para auditoría
8. `T017-08` Encadenar la moderación a la creación de ofertas
9. `T017-09` Implementar las pruebas de integración del flujo completo
10. `T017-10` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
11. `T017-11` Registrar evidencias del Constitution Check
