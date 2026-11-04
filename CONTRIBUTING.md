# Contribuir — Sensus Backend

Gracias por contribuir. Este repositorio sigue **Spec-Driven Development (SDD)** y está gobernado por
[`constitution.md`](constitution.md). Si algo aquí contradice la constitución, prevalece la constitución.

---

## 1. Antes de escribir código

1. Lee [`constitution.md`](constitution.md) y [`AGENTS.md`](AGENTS.md).
2. Localiza la capacidad en [`specs/README.md`](specs/README.md).
3. Comprueba que la spec tiene sus **cuatro artefactos**: `spec.md`, `plan.md`, `tasks.md` y
   `checklist.md`. Si falta alguno, complétalo antes de implementar.
4. Comprueba que el requisito que vas a implementar existe en `spec.md` con identificador
   `FR-<spec>-<nn>` o `NFR-<spec>-<nn>`.

Si la capacidad no está especificada, **detente** y abre una propuesta de spec.

---

## 2. Flujo de trabajo

```
specify → clarify → plan → tasks → checklist → implement
```

| Fase | Artefacto | Criterio de salida |
|---|---|---|
| `specify` | `spec.md` | Alcance, exclusiones y requisitos con ID |
| `clarify` | `spec.md` | Sin preguntas abiertas ni criterios no medibles |
| `plan` | `plan.md` | Constitution Check evaluado |
| `tasks` | `tasks.md` | Tareas atómicas trazables a requisitos |
| `checklist` | `checklist.md` | DoR, criterios específicos y DoD listos |
| `implement` | código, pruebas, IaC | Definition of Done cumplida |

No se adelantan fases. No se implementa sin `plan.md` y `checklist.md`.

---

## 3. Convenciones

- **Idioma**: documentación en español; código, identificadores, ramas y commits en inglés.
- **Commits**: Conventional Commits con requisito — `feat(FR-012-03): ...`, `fix(FR-030-01): ...`.
- **Ramas**: una por requisito o spec, p. ej. `feat/012-cors-y-https`.
- **PRs**: pequeños, con propósito único, con el ID del requisito en el título.
- **Formato**: `dotnet format sensus-back.sln --verify-no-changes` antes de entregar.

### Código C#

- `nullable` habilitado; sin `.Result` ni `.Wait()`.
- `record` para DTOs; `CancellationToken` propagado en toda operación de E/S.
- Reglas de negocio en `Sensus.Domain`; orquestación en `Sensus.Application`.
- Prohibido devolver entidades de dominio o documentos de MongoDB desde la API.

---

## 4. Pruebas obligatorias

| Tipo de cambio | Prueba |
|---|---|
| Invariante o regla de dominio | Unitaria |
| Endpoint nuevo o modificado | Integración: éxito, entrada inválida, sin autenticación, sin permisos |
| Persistencia | Integración con Testcontainers contra MongoDB real |
| Integración externa | Doble que simule fallo, timeout y respuesta malformada |
| Corrección de defecto | Regresión que falla antes del arreglo |

```bash
dotnet test sensus-back.sln --collect:"XPlat Code Coverage"
```

Cobertura mínima: **80 %** en `Domain` y `Application`. No se bajan umbrales ni se omiten pruebas.

---

## 5. Definition of Done

Una tarea está terminada solo si:

- [ ] El requisito está referenciado y los criterios de aceptación están cubiertos.
- [ ] El código está en la capa correcta y el test de arquitectura pasa.
- [ ] Las pruebas unitarias y de integración están en verde.
- [ ] La cobertura cumple el umbral.
- [ ] El contrato OpenAPI está actualizado si cambió la API.
- [ ] La IaC está actualizada y `cdk diff` revisado si aplica.
- [ ] Los logs incluyen `correlation-id` y no contienen PII.
- [ ] `tasks.md` y `checklist.md` reflejan el estado real, sin casillas pendientes.
- [ ] `memory.md` está actualizado si cambió el estado del proyecto.

---

## 6. Prohibiciones

- Añadir bases de datos relacionales, ORM relacional o migraciones SQL.
- Integrar nubes distintas de AWS o servicios fuera del Free Tier sin aprobación.
- Crear recursos AWS fuera de `infra/`.
- Publicar contenido sin moderación previa o implementar moderación *fail-open*.
- Versionar secretos, cadenas de conexión o tokens.
- Registrar PII, tokens o contenido moderado en logs.
- Exponer colecciones sin paginación o consultas sin índice declarado.
- Fijar umbrales de moderación, paginación o rate limiting en código.

---

## 7. Seguridad

- No incluyas datos reales de personas en ejemplos, pruebas ni documentación.
- Reporta vulnerabilidades según [`SECURITY.md`](SECURITY.md), nunca en un issue público.
- Si detectas un secreto versionado, detente, notifica y solicita su rotación.

---

**Alineado a `constitution.md` v1.2.0 y `AGENTS.md` v1.2.0.**
