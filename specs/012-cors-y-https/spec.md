# Spec 012 — CORS y HTTPS por entorno

| Campo | Valor |
|---|---|
| ID | 012 |
| Slug | `cors-y-https` |
| Estado | Pendiente |
| Ola | 6 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | Ninguna |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Restringir CORS a orígenes conocidos y forzar HTTPS fuera de desarrollo.

## 2. Alcance

**Incluye**

- Política CORS por entorno
- Redirección HTTPS
- HSTS

**No incluye**

- Certificados propios
- WAF

## 3. Actores

- Cliente web
- Cliente móvil

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-012-01 | Definir orígenes permitidos por configuración y entorno | Must |
| FR-012-02 | Prohibir AllowAnyOrigin fuera de desarrollo | Must |
| FR-012-03 | Redirigir a HTTPS fuera de desarrollo | Must |
| FR-012-04 | Habilitar HSTS en producción | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-012-01 | Orígenes permitidos | Lista explícita, cero comodines en producción |

## 6. Criterios de aceptación

### CA-012-01

- **Dado** origen no permitido
- **Cuando** se envía una petición CORS
- **Entonces** sin cabecera Access-Control-Allow-Origin

### CA-012-02

- **Dado** petición HTTP en producción
- **Cuando** se invoca la API
- **Entonces** redirección 308 a HTTPS

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| — | — | — |

## 8. Dependencias y supuestos

- Depende de: Ninguna
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Una lista mal configurada bloquea al cliente legítimo

## 10. Preguntas abiertas

- ¿Qué dominios web consumirán la API?

## 11. Trazabilidad

- Requisitos de este spec: `FR-012-01`, `FR-012-02`, `FR-012-03`, `FR-012-04`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
