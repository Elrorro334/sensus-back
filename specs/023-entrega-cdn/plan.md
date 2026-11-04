# Plan 023 — Entrega de contenido por CDN

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Documenta estado actual con brechas |
| Versión | 1.0.0 |
| Constitution Check | FAIL |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Servir el contenido aprobado a través de CloudFront con caché y control de acceso.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Infrastructure | Distribución CloudFront y política de caché en CDK |
| Application | Generación de URLs firmadas |

## 3. Modelo de datos e índices

- Sin cambios de datos

## 4. Seguridad

- Firma de URLs
- Bucket privado como origen

## 5. Observabilidad y rendimiento

- Métricas de aciertos de caché y errores 4xx

## 6. Costo AWS

CloudFront: 1 TB de salida gratuita al mes; vigilar.

## 7. Estrategia de pruebas

- Validación de IaC
- Prueba de URL sin firma

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-023-01` y 3 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | FAIL | no existe infraestructura como código |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> **Brechas abiertas:** las decisiones `FAIL` no bloquean la documentación de un retro-spec, pero MUST resolverse antes de implementar cualquier funcionalidad que dependa de ella.

## 9. Riesgos de implementación

- Costo por transferencia si el contenido se comparte masivamente

## 10. Orden de implementación

1. `T023-01` Implementar: Distribución CloudFront y política de caché en CDK
2. `T023-02` Implementar: Generación de URLs firmadas
3. `T023-03` Definir distribución y política de caché
4. `T023-04` Generar URLs firmadas para contenido privado
5. `T023-05` Implementar las pruebas de integración del flujo completo
6. `T023-06` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
7. `T023-07` Registrar evidencias del Constitution Check
