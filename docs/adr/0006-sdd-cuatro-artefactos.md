# ADR 0006 — Spec-Driven Development con cuatro artefactos por especificación

| Campo | Valor |
|---|---|
| Estado | Aceptada |
| Fecha | 2026-10-29 |
| Principios afectados | I, V |
| Specs relacionados | Todos |
| Fuente | `arquitecturaNva.docx` (metodología SDD) |

## Contexto

El proyecto necesita que cada cambio sea trazable desde el requisito hasta la prueba, y que el alcance no
crezca sin control. Un único documento de especificación por funcionalidad tiende a mezclar alcance,
diseño y tareas, y deja sin registrar el criterio de cierre. Además, los identificadores globales
`FR-###` colisionan cuando varias funcionalidades se especifican en paralelo.

## Decisión

- Una capacidad por especificación, con alcance implementable en un máximo aproximado de una semana.
- Cada especificación vive en `specs/NNN-slug/` con un ID numérico estable de tres dígitos.
- **Cuatro artefactos obligatorios** por spec:

| Artefacto | Contenido | Criterio de salida |
|---|---|---|
| `spec.md` | Contexto, alcance, exclusiones, `FR`/`NFR`, criterios de aceptación, contratos | Requisitos con ID y criterios medibles |
| `plan.md` | Diseño por capa, datos, seguridad, costo, pruebas, Constitution Check | Los siete principios evaluados |
| `tasks.md` | Tareas atómicas con capa, requisito y dependencia | Toda tarea trazable |
| `checklist.md` | DoR, criterios específicos, DoD y evidencia | Sin casillas pendientes al cerrar |

- Los requisitos usan `FR-<spec>-<nn>` y `NFR-<spec>-<nn>`, con el prefijo igual al ID de la carpeta.
- El contrato del spec, si expone endpoints, vive en `contracts/openapi.yaml`.
- El índice y el registro viven en `specs/README.md` y `specs/requirements.md`.
- La verificación es automática: `node .github/scripts/check-specs.mjs specs` falla si falta un artefacto,
  si el ID no aparece o si un requisito usa un prefijo ajeno.

## Consecuencias

**Positivas**

- Alcance acotado y revisable por spec.
- Trazabilidad requisito → tarea → prueba → evidencia.
- Los identificadores no colisionan entre funcionalidades.
- El estado del trabajo es visible sin preguntar.

**Negativas**

- Cuatro archivos por capacidad: más escritura y más mantenimiento.
- Riesgo de duplicar información entre `spec.md`, `plan.md` y `tasks.md`.
- El retro-spec de código existente puede declarar brechas en lugar de PASS.

**Mitigación**

- Plantillas en `specs/_templates/` y validación automática en CI.
- Regla de responsabilidad única por artefacto: el alcance en `spec`, el diseño en `plan`, el trabajo en
  `tasks`, la evidencia en `checklist`.
- Los `FAIL` de un retro-spec se registran como deuda técnica con tarea de remediación y fecha.

## Alternativas consideradas

| Alternativa | Motivo del descarte |
|---|---|
| Un solo documento por funcionalidad | Mezcla alcance, diseño y tareas; sin criterio de cierre |
| Documento único global de requisitos | Sin alcance acotado; crece sin control |
| Identificadores globales `FR-###` | Colisionan al especificar en paralelo |
| Gestión solo con issues de GitHub | Sin versionado del diseño ni trazabilidad verificable en CI |

## Referencias

- `constitution.md` P-I y §3.6
- `AGENTS.md` §4
- `specs/README.md`, `specs/requirements.md`
- `.github/scripts/check-specs.mjs`
