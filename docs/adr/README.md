# Decisiones de arquitectura (ADR)

Registro de decisiones estructurales del backend Sensus. Cada ADR es inmutable una vez aceptada: para
cambiarla se crea una nueva que la sustituya y se marca la anterior como `Sustituida`.

| ID | Decisión | Estado | Fecha |
|---|---|---|---|
| [0001](0001-clean-architecture.md) | Clean Architecture en cuatro capas con dependencias unidireccionales | Aceptada | 2026-10-28 |
| [0002](0002-stack-dotnet-mongodb.md) | Stack fijado: .NET 10 LTS, C# 14 y MongoDB 8 | Aceptada | 2026-10-28 |
| [0003](0003-cqrs-y-contratos.md) | CQRS con DTOs explícitos y OpenAPI como contrato público | Aceptada | 2026-10-28 |
| [0004](0004-moderacion-multimodal.md) | Moderación multimodal fail-closed con `omni-moderation-latest` | Aceptada | 2026-10-28 |
| [0005](0005-serverless-aws-costo-cero.md) | Operación serverless en AWS con costo objetivo 0 USD e IaC | Aceptada | 2026-10-28 |
| [0006](0006-sdd-cuatro-artefactos.md) | SDD con cuatro artefactos por especificación | Aceptada | 2026-10-29 |
| [0007](0007-despacho-push-abstracto.md) | Despacho push mediante `IPushDispatcher` con adaptadores FCM o SNS | Aceptada | 2026-10-29 |
| [0008](0008-rutas-versionadas-ingles.md) | Rutas `/api/v1` con recursos en inglés y acciones como subrecurso | Aceptada | 2026-10-29 |
| [0009](0009-ranking-determinista.md) | Ranking determinista en el backend, sin servicio de ML externo | Aceptada | 2026-10-29 |

---

## Cuándo crear un ADR

- Se elige entre dos o más alternativas con impacto estructural.
- Se cambia una dependencia externa (base de datos, proveedor de moderación, almacenamiento, CI).
- Se decide algo que un futuro integrante podría revertir sin saber por qué se hizo.
- Se registra una excepción a la constitución (`constitution.md` §4.4 y Apéndice E).

No hace falta un ADR para decisiones locales y reversibles dentro de una capa.

---

## Plantilla

```markdown
# ADR NNNN — Título corto en imperativo

| Campo | Valor |
|---|---|
| Estado | Propuesta / Aceptada / Sustituida por ADR-NNNN / Rechazada |
| Fecha | AAAA-MM-DD |
| Principios afectados | I–VII |
| Specs relacionados | NNN, NNN |

## Contexto
Qué problema hay y qué restricciones aplican.

## Decisión
Qué se decide, en términos verificables.

## Consecuencias
Positivas, negativas y mitigaciones.

## Alternativas consideradas
Tabla de alternativa y motivo del descarte.

## Referencias
Constitución, specs, documentos.
```

---

## Convenciones

- Archivo: `NNNN-slug-en-minusculas.md`, numeración consecutiva sin huecos.
- Idioma: español; identificadores técnicos en inglés.
- Una ADR describe una decisión, no un plan de trabajo: las tareas viven en `specs/NNN-slug/tasks.md`.
- Toda ADR que afecte un principio MUST reflejarse también en `constitution.md` §4.6 si cambia la norma.
