---
name: sdd-traceability
description: Garantiza trazabilidad spec ↔ requisitos FR/NFR ↔ tareas ↔ código ↔ tests
license: MIT
---

# SDD Traceability

## Objetivo
Mantener trazabilidad estricta según `constitution.md` §1 y `AGENTS.md` §3-4.

## Reglas obligatorias
- **ID estable**: `NNN` numérico de 3 dígitos, carpeta `specs/NNN-slug/`. No reutilizar ni renumerar.
- **4 artefactos**: `spec.md`, `plan.md`, `tasks.md`, `checklist.md`. Ausencia invalida la spec.
- **Requisitos**: `FR-<spec>-<nn>` para funcionales, `NFR-<spec>-<nn>` para no funcionales. Prefijo = ID de carpeta.
- **Secuencial**: Numeración secuencial por tipo (FR/NFR) dentro de la spec.
- **Trazabilidad**: Toda tarea referencia ≥1 requisito. Todo requisito con ≥1 prueba asociada registrada.
- **Constitution Check**: Debe aparecer en `plan.md` (PASS o brechas con remediación).
- **Versionado**: Los 4 artefactos deben citar `constitution.md` v1.2.0.

## Checklist de verificación
- [ ] Carpeta cumple patrón `^\d{3}-[a-z0-9]+(-[a-z0-9]+)*$`
- [ ] Los 4 archivos existen
- [ ] ID aparece en todos los artefactos
- [ ] Requisitos usan formato correcto y prefijo válido
- [ ] `requirements.md` tiene los requisitos de la spec
- [ ] `tasks.md` referencia requisitos
- [ ] `checklist.md` tiene DoR/DoD y evidencia
- [ ] Si hay endpoints → existe `contracts/openapi.yaml`
- [ ] Estado en `specs/README.md` coherente

## Comandos
```bash
node .github/scripts/check-specs.mjs specs
```

## Anti-patrones
- Usar `REQ-xxx` o formatos distintos
- Mezclar requisitos entre specs
- Tareas sin requisito
- Specs sin checklist
- Criterios no medibles
