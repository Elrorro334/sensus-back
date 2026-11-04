# Plan 015 — Métricas y alarmas

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Documenta estado actual con brechas |
| Versión | 1.0.0 |
| Constitution Check | FAIL |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Medir latencia, errores y saturación, con alarmas definidas como código.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Infraestructura | Definición de alarmas y panel en CDK |
| Api | Emisión de métricas de aplicación |

## 3. Modelo de datos e índices

- Sin cambios de datos

## 4. Seguridad

- Las alarmas no contienen datos personales

## 5. Observabilidad y rendimiento

- Esta spec es la propia observabilidad

## 6. Costo AWS

CloudWatch: 10 alarmas gratuitas; más allá, costo por alarma.

## 7. Estrategia de pruebas

- Validación de sintaxis de IaC
- Prueba de umbral con métrica simulada

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-015-01` y 2 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | FAIL | no existe infraestructura como código todavía |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> **Brechas abiertas:** las decisiones `FAIL` no bloquean la documentación de un retro-spec, pero MUST resolverse antes de implementar cualquier funcionalidad que dependa de ella.

## 9. Riesgos de implementación

- Sin IaC no se pueden versionar las alarmas

## 10. Orden de implementación

1. `T015-01` Implementar: Definición de alarmas y panel en CDK
2. `T015-02` Implementar: Emisión de métricas de aplicación
3. `T015-03` Definir alarmas en CDK
4. `T015-04` Emitir métricas de aplicación
5. `T015-05` Implementar las pruebas de integración del flujo completo
6. `T015-06` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
7. `T015-07` Registrar evidencias del Constitution Check
