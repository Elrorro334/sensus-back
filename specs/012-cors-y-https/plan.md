# Plan 012 — CORS y HTTPS por entorno

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Documenta estado actual con brechas |
| Versión | 1.0.0 |
| Constitution Check | FAIL |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Restringir CORS a orígenes conocidos y forzar HTTPS fuera de desarrollo.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Api | Configuración de CORS y HTTPS por entorno |
| Infraestructura | Variables de entorno por despliegue |

## 3. Modelo de datos e índices

- Sin cambios de datos

## 4. Seguridad

- Cierra una exposición actual: AllowAnyOrigin incondicional

## 5. Observabilidad y rendimiento

- Métrica de peticiones bloqueadas por CORS

## 6. Costo AWS

Sin costo incremental: Lambda y API Gateway dentro del Free Tier.

## 7. Estrategia de pruebas

- Integración de origen permitido y denegado

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-012-01` y 3 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | FAIL | CORS abierto y RequireHttpsMetadata desactivado |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> **Brechas abiertas:** las decisiones `FAIL` no bloquean la documentación de un retro-spec, pero MUST resolverse antes de implementar cualquier funcionalidad que dependa de ella.

## 9. Riesgos de implementación

- Una lista mal configurada bloquea al cliente legítimo

## 10. Orden de implementación

1. `T012-01` Implementar: Configuración de CORS y HTTPS por entorno
2. `T012-02` Implementar: Variables de entorno por despliegue
3. `T012-03` Mover los orígenes permitidos a configuración
4. `T012-04` Condicionar HTTPS y HSTS al entorno
5. `T012-05` Implementar las pruebas de integración del flujo completo
6. `T012-06` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
7. `T012-07` Registrar evidencias del Constitution Check
