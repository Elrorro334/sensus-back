# Checklist 001 — Registro de cuenta unificada

Alineado a `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0.

Usar antes, durante y al cierre de la implementación. Ninguna tarea se cierra con casillas pendientes.

## A. Antes de empezar (Definition of Ready)

- [ ] La spec 001 está aprobada y sin preguntas bloqueantes
- [ ] Los requisitos tienen identificador y criterio medible
- [ ] Las dependencias están resueltas: Ninguna
- [ ] El alcance y las exclusiones están declarados
- [ ] El impacto en seguridad, costo y rendimiento fue evaluado

## B. Específicos de este spec

- [ ] El contrato de registro está en OpenAPI
- [ ] El índice único de correo se crea al iniciar
- [ ] Ninguna respuesta incluye passwordHash
- [ ] No se registra PII en logs

## C. Antes de entregar (Definition of Done)

- [ ] Código en la capa correcta y test de arquitectura en verde
- [ ] Pruebas unitarias y de integración añadidas y en verde
- [ ] Cobertura ≥ 80 % en Domain y Application
- [ ] Contrato OpenAPI actualizado
- [ ] IaC actualizada y `cdk diff` revisado si aplica
- [ ] Logs con correlation-id y sin PII
- [ ] Revisión de seguridad: sin secretos, autorización verificada
- [ ] Constitution Check en PASS o brecha registrada

## D. Constitution Check

- [ ] **I. Especificación y trazabilidad** — PASS
- [ ] **II. Clean Architecture** — PASS
- [ ] **III. CQRS y contratos** — PASS
- [ ] **IV. Seguridad y privacidad** — FAIL
- [ ] **V. Verificación automatizada** — FAIL
- [ ] **VI. Costo cero e IaC** — PASS
- [ ] **VII. Rendimiento y observabilidad** — PARCIAL

## E. Evidencia

| Artefacto | Ubicación | Estado |
|---|---|---|
| Spec | [`spec.md`](spec.md) | ☐ |
| Plan | [`plan.md`](plan.md) | ☐ |
| Tareas | [`tasks.md`](tasks.md) | ☐ |
| Pruebas unitarias | `tests/Sensus.Application.UnitTests/` | ☐ |
| Pruebas de integración | `tests/Sensus.IntegrationTests/` | ☐ |
| Contrato OpenAPI | `contracts/openapi.yaml` | ☐ |

## F. Registro

| Fecha | Responsable | Resultado | Observaciones |
|---|---|---|---|
| — | — | — | — |
