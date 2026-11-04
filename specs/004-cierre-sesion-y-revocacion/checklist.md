# Checklist 004 — Cierre de sesión y revocación de token

Alineado a `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0.

Usar antes, durante y al cierre de la implementación. Ninguna tarea se cierra con casillas pendientes.

## A. Antes de empezar (Definition of Ready)

- [x] La spec 004 está aprobada y sin preguntas bloqueantes
- [x] Los requisitos tienen identificador y criterio medible
- [x] Las dependencias están resueltas: 002
- [x] El alcance y las exclusiones están declarados
- [x] El impacto en seguridad, costo y rendimiento fue evaluado

## B. Específicos de este spec

- [x] El token revocado devuelve 401 de inmediato en endpoints protegidos
- [x] La invalidación vía rotación de SecurityStamp está verificada en el pipeline JWT
- [x] El logout es idempotente (responde 204 ante llamadas repetidas con el mismo token)

## C. Antes de entregar (Definition of Done)

- [x] Código en la capa correcta y test de arquitectura en verde (4/4)
- [x] Pruebas unitarias y de integración añadidas y en verde (100 unitarias, 34 integración)
- [x] Cobertura evaluada y verificada
- [x] Contrato OpenAPI actualizado
- [x] IaC verificada (sin impacto incremental)
- [x] Logs con correlation-id y sin PII ni tokens
- [x] Revisión de seguridad: sin secretos, autorización verificada
- [x] Constitution Check en PASS

## D. Constitution Check

- [x] **I. Especificación y trazabilidad** — PASS
- [x] **II. Clean Architecture** — PASS
- [x] **III. CQRS y contratos** — PASS
- [x] **IV. Seguridad y privacidad** — PASS
- [x] **V. Verificación automatizada** — PASS
- [x] **VI. Costo cero e IaC** — PASS
- [x] **VII. Rendimiento y observabilidad** — PASS

## E. Evidencia

| Artefacto | Ubicación | Estado |
|---|---|---|
| Spec | [`spec.md`](spec.md) | Completado |
| Plan | [`plan.md`](plan.md) | Completado |
| Tareas | [`tasks.md`](tasks.md) | Completado |
| Pruebas unitarias | `tests/Sensus.Application.UnitTests/Users/Commands/LogoutCommandHandlerTests.cs` | 3 pruebas en verde |
| Pruebas de integración | `tests/Sensus.IntegrationTests/Security/SecurityPipelineTests.cs` | 3 pruebas de logout en verde |
| Contrato OpenAPI | `contracts/openapi.yaml` | Actualizado y validado |

## F. Registro

| Fecha | Responsable | Resultado | Observaciones |
|---|---|---|---|
| 2026-11-04 | Rodrigo / Antigravity | PASS | Implementación completa de POST /api/v1/auth/logout con rotación de SecurityStamp, 100 pruebas unitarias y 34 de integración en verde. |
