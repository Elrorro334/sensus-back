# ADR 0003 — CQRS con DTOs explícitos y OpenAPI como contrato público

| Campo | Valor |
|---|---|
| Estado | Aceptada |
| Fecha | 2026-10-28 |
| Principios afectados | III, VII |
| Specs relacionados | 009, 010, 011 |
| Fuente | `arquitecturaNva.docx` |

## Contexto

Los flujos de lectura y escritura tienen requisitos distintos: la escritura prioriza invariantes y
consistencia; la lectura, proyecciones e índices. Exponer entidades de dominio o documentos de MongoDB
convierte el esquema de persistencia en contrato público de facto y bloquea cualquier refactor interno.
Además, sin un contrato versionado no es posible detectar cambios incompatibles.

## Decisión

- Modelar comandos y consultas por separado, con un handler cada uno, sin mezclar lectura y escritura.
- Exponer únicamente DTOs (`record`) desde la API; nunca entidades de dominio ni documentos de MongoDB.
- Mapear de forma explícita entre dominio, aplicación y contrato, con pruebas del mapeo.
- Validar en dos niveles: borde (formato, autenticación, autorización) y dominio (invariantes).
- Publicar un único documento OpenAPI 3.1, versionado, con esquema `bearerAuth` y todos los códigos de
  respuesta, validado en CI.
- Responder errores con `ProblemDetails` uniforme e identificador de correlación, sin trazas ni nombres de
  colecciones.

## Consecuencias

**Positivas**

- El contrato público evoluciona de forma controlada.
- Las consultas pueden optimizarse sin tocar el dominio.
- Los cambios incompatibles se detectan antes de fusionar.

**Negativas**

- Duplicación aparente entre entidad y DTO que exige mantenimiento.
- La validación en dos niveles puede parecer redundante.

**Mitigación**

- Pruebas de mapeo que fallan si un campo nuevo queda sin mapear.
- Un solo generador de OpenAPI (eliminar la duplicación Swashbuckle / `Microsoft.AspNetCore.OpenApi`, spec 011).
- Diff de OpenAPI en CI.

## Alternativas consideradas

| Alternativa | Motivo del descarte |
|---|---|
| Devolver entidades de dominio | Viola P-III; acopla la API al modelo interno |
| Un único modelo para lectura y escritura | Impide proyectar y optimizar consultas |
| Contrato generado solo en tiempo de ejecución | No se puede validar en CI ni versionar |

## Referencias

- `constitution.md` P-III y §2.2
- `specs/009-errores-problemdetails/`, `specs/010-paginacion-estandar/`, `specs/011-contrato-openapi/`
