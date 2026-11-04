# ADR 0008 — Rutas públicas versionadas y en inglés

| Campo | Valor |
|---|---|
| Estado | Aceptada |
| Fecha | 2026-10-29 |
| Principios afectados | III, VII |
| Specs relacionados | Todas las que exponen endpoints; 008 en particular |

## Contexto

El catálogo de endpoints usa rutas sin versión y en inglés (`/api/merchants`, `/api/offers`), mientras el
código actual expone rutas en español (`/api/v1/comercios`, `/api/v1/comercio/*`). El Principio III exige
contratos versionados y AGENTS §15 exige identificadores en inglés. No hay clientes en producción.

## Decisión

- Prefijo público obligatorio `/api/v1`.
- Recursos en inglés, en plural y sin verbos: `/auth`, `/users`, `/merchants`, `/offers`, `/feed/posts`,
  `/reports`, `/ml`, `/media`.
- Acciones no CRUD como subrecursos: `/offers/{id}/claim`, `/offers/{id}/status`,
  `/offers/claims/{code}/redeem`, `/feed/posts/{id}/like`, `/users/favorites/{id}`.
- `/health/live` y `/health/ready` quedan fuera del prefijo versionado por ser endpoints operativos.
- Las rutas en español se retiran sin periodo de convivencia; el retiro se especifica en la spec 008.

## Consecuencias

**Positivas**

- Un único contrato coherente con el catálogo y con la constitución.
- El versionado futuro no requiere renombrar recursos.
- Se elimina la duplicación de rutas.

**Negativas**

- Cualquier compilación móvil existente deja de funcionar.
- Hay que actualizar README, `.http`, OpenAPI y pruebas.

**Mitigación**

- Retiro controlado en la ola 6, después de que las rutas nuevas estén verdes.
- Prueba que verifica 404 en las rutas retiradas.

## Alternativas consideradas

| Alternativa | Motivo del descarte |
|---|---|
| Mantener `/api` sin versión | Viola P-III y encarece el primer cambio incompatible |
| Conservar nombres en español | Contradice AGENTS §15 y se aleja del catálogo |
| Mantener alias en español | Duplica contrato y multiplica la superficie de prueba |

## Referencias

- `constitution.md` P-III y §3.5
- `AGENTS.md` §15
- `specs/008-retiro-endpoints-legacy/`
