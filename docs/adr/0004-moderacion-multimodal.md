# ADR 0004 — Moderación multimodal fail-closed con `omni-moderation-latest`

| Campo | Valor |
|---|---|
| Estado | Aceptada |
| Fecha | 2026-10-28 |
| Principios afectados | IV |
| Specs relacionados | 017, 018, 019, 020, 021 |
| Fuente | `arquitecturaNva.docx` §Contrato mínimo de moderación |

## Contexto

Sensus publica contenido generado por usuarios. El contenido sexual, la violencia y las autolesiones
deben evaluarse **antes** de publicar, con imagen y texto asociados en la misma solicitud. Un modelo local
está prohibido y un proveedor externo puede fallar, expirar o responder de forma inválida.

## Decisión

- Usar la API de moderación de OpenAI con el modelo `omni-moderation-latest`.
- Enviar imagen (Base64 o URL) **y** texto asociado en la misma solicitud.
- Interpretar `flagged`, `categories` y `category_scores` de las seis subcategorías: sexual, erótico,
  desnudez, violencia gráfica, violencia general y autolesiones (suicidio, intención, instrucciones).
- Aplicar tres bandas con umbrales externalizados por entorno:
  - baja → publicación automática,
  - intermedia → revisión manual,
  - alta → rechazo automático y baneo.
- **Fail-closed**: ante error, timeout, cuota agotada o respuesta inválida, el contenido no se publica y
  queda en cuarentena.
- Registrar cada decisión con veredicto, categorías, puntajes, umbral aplicado, acción y marca temporal.
- Gestionar la clave del proveedor en AWS Secrets Manager, nunca en el repositorio.
- Acotar el tiempo de espera y contar con reintentos acotados; el agotamiento se trata como fallo.

## Consecuencias

**Positivas**

- El contenido no conforme no llega a publicarse.
- Las decisiones son auditables y las sanciones, reversibles.
- Cambiar de proveedor solo afecta a `Sensus.Infrastructure`.

**Negativas**

- Dependencia de un servicio externo sin SLA contractual.
- Falsos positivos que sancionan a personas inocentes.
- Latencia añadida al flujo de publicación.

**Mitigación**

- Cuarentena en lugar de borrado; sanción reversible con auditoría (spec 020).
- Umbrales ajustables sin recompilar (spec 018).
- Métricas de latencia y de fallos del proveedor (spec 015).

## Alternativas consideradas

| Alternativa | Motivo del descarte |
|---|---|
| Modelo local de visión artificial | Prohibido por §2.2 |
| Moderación posterior a la publicación | Viola P-IV: el contenido ya sería visible |
| *Fail-open* con revisión diferida | Prohibido explícitamente; expone contenido de riesgo |
| Solo moderación de texto | Ignora la imagen, que es el vector principal de riesgo |

## Referencias

- `constitution.md` P-IV y §2.4
- `specs/017-moderacion-multimodal/` … `specs/021-auditoria-moderacion/`
- `SECURITY.md`
