# Spec 011 — Contrato OpenAPI

| Campo | Valor |
|---|---|
| ID | 011 |
| Slug | `contrato-openapi` |
| Estado | Parcial |
| Ola | 0 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 009, 010 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Publicar y validar un contrato OpenAPI que refleje exactamente los endpoints y modelos expuestos.

## 2. Alcance

**Incluye**

- Generación del documento OpenAPI 3.1
- Documentación de esquemas y seguridad
- Validación en CI

**No incluye**

- Portal de desarrolladores
- SDKs generados

## 3. Actores

- Equipo de desarrollo
- Cliente móvil

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-011-01 | Exponer el documento OpenAPI en desarrollo | Must |
| FR-011-02 | Documentar todos los endpoints con esquemas y códigos de respuesta | Must |
| FR-011-03 | Validar el documento en CI y fallar ante cambios incompatibles | Must |
| FR-011-04 | Versionar el contrato junto con la API | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-011-01 | Cobertura del contrato | 100 % de endpoints publicados |

## 6. Criterios de aceptación

### CA-011-01

- **Dado** endpoint nuevo
- **Cuando** se genera el documento
- **Entonces** aparece con esquema y seguridad

### CA-011-02

- **Dado** cambio incompatible
- **Cuando** se ejecuta CI
- **Entonces** el pipeline falla y solicita nueva versión

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| — | — | — |

## 8. Dependencias y supuestos

- Depende de: 009, 010
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Duplicar Swashbuckle y Microsoft.AspNetCore.OpenApi puede generar dos documentos distintos

## 10. Preguntas abiertas

- ¿Se publica el contrato fuera del entorno de desarrollo?

## 11. Trazabilidad

- Requisitos de este spec: `FR-011-01`, `FR-011-02`, `FR-011-03`, `FR-011-04`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
