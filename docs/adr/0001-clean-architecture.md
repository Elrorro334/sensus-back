# ADR 0001 — Clean Architecture en cuatro capas con dependencias unidireccionales

| Campo | Valor |
|---|---|
| Estado | Aceptada |
| Fecha | 2026-10-28 |
| Principios afectados | II, III |
| Specs relacionados | Todos |
| Fuente | `arquitecturaNva.docx` |

## Contexto

El backend debe poder sustituir MongoDB, el proveedor de moderación, el correo o el almacenamiento sin
reescribir las reglas del negocio. La fuente autoritativa exige separación estricta entre dominio,
aplicación, infraestructura y presentación, y prohíbe que el modelo de persistencia se filtre a la API.

## Decisión

Organizar la solución en cuatro proyectos con dependencias que apuntan exclusivamente hacia el dominio:

```
Sensus.Domain          Entidades, invariantes, contratos. Sin dependencias externas.
Sensus.Application     Casos de uso CQRS, DTOs, validación de borde, mapeos.
Sensus.Infrastructure  MongoDB, BCrypt, JWT, S3, OpenAI, correo, push.
Sensus.Api             Endpoints, middleware y composición.
```

- `Application → Domain`, `Infrastructure → Application`, `Api → Application`.
- `Domain` y `Application` no referencian `Infrastructure` ni SDKs de nube.
- Toda dependencia externa se declara como interfaz en `Domain` o `Application`.
- La composición se realiza únicamente en `src/Sensus.Api/Program.cs`.

## Consecuencias

**Positivas**

- Cambiar de proveedor no toca reglas de negocio.
- El dominio se prueba sin base de datos ni contenedores.
- Las dependencias prohibidas se detectan automáticamente.

**Negativas**

- Más proyectos, interfaces y mapeos que una aplicación en capas simple.
- Requiere disciplina en la revisión: es fácil crear atajos desde `Api` hacia `Infrastructure`.

**Mitigación**

- Test de arquitectura con NetArchTest en CI (`tests/Sensus.ArchitectureTests`), bloqueante.
- Checklist de capas en la plantilla de PR.

## Alternativas consideradas

| Alternativa | Motivo del descarte |
|---|---|
| Monolito en capas tradicionales | Acopla el dominio a la persistencia; impide sustituir MongoDB |
| Arquitectura hexagonal pura con puertos y adaptadores por agregado | Válida en concepto, pero añade indirección sin beneficio en un equipo pequeño |
| Vertical slice architecture | Difumina la dirección de dependencias que la constitución exige verificar |

## Referencias

- `constitution.md` P-II y §3.6
- `AGENTS.md` §7
- `docs/architecture/arquitecturaNva.docx`
