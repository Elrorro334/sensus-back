# Descripción

<!-- Qué cambia y por qué. Una frase clara es suficiente si el título ya lo dice. -->

## Requisito y spec

| Campo | Valor |
|---|---|
| Spec | `specs/NNN-slug/` |
| Requisito(s) | `FR-NNN-nn`, `NFR-NNN-nn` |
| Tipo de cambio | Funcionalidad / Corrección / Refactor / Documentación / Infraestructura |

## Alcance

- [ ] El cambio está descrito en `spec.md` (sin alcance inventado).
- [ ] `tasks.md` y `checklist.md` reflejan el estado real.
- [ ] No se incluyen cambios ajenos al requisito.

## Verificación

- [ ] `dotnet build sensus-back.sln -warnaserror` en verde.
- [ ] `dotnet test sensus-back.sln` en verde.
- [ ] `dotnet format sensus-back.sln --verify-no-changes` sin cambios pendientes.
- [ ] Cobertura ≥ 80 % en `Domain` y `Application`.
- [ ] Test de arquitectura en verde.
- [ ] Pruebas de integración añadidas si cambió un endpoint.
- [ ] `cdk synth` y `cdk diff` revisados si cambió `infra/`.

## Seguridad

- [ ] Sin secretos, cadenas de conexión ni tokens en el diff.
- [ ] Sin PII ni contenido moderado en logs.
- [ ] Autorización verificada en el servidor para cada operación nueva.
- [ ] Sin nuevas dependencias vulnerables conocidas.
- [ ] Si hay migración o cambio de datos, está documentado y es reversible.

## Constitution Check

| Principio | Estado | Evidencia |
|---|---|---|
| I. Especificación y trazabilidad | PASS / FAIL | |
| II. Clean Architecture | PASS / FAIL | |
| III. CQRS y contratos | PASS / FAIL | |
| IV. Seguridad y privacidad | PASS / FAIL | |
| V. Verificación automatizada | PASS / FAIL | |
| VI. Costo cero e IaC | PASS / FAIL | |
| VII. Rendimiento y observabilidad | PASS / FAIL | |

Resultado global: **PASS / PASS CON OBSERVACIONES / FAIL**

Brechas abiertas y su tarea de remediación:

<!-- Si hay un FAIL, indica la tarea en tasks.md y la fecha de resolución. -->

## Capturas o evidencia

<!-- Opcional: salida de pruebas, métricas, capturas de ProblemDetails, etc. -->

## Notas para la revisión

<!-- Decisiones discutibles, alternativas descartadas, riesgos asumidos. -->
