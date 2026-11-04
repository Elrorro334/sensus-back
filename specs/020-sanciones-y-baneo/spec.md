# Spec 020 — Sanciones y baneo de usuario

| Campo | Valor |
|---|---|
| ID | 020 |
| Slug | `sanciones-y-baneo` |
| Estado | Pendiente |
| Ola | 4 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 018, 019 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Aplicar sanciones proporcionales y reversibles ante contenido de riesgo alto.

## 2. Alcance

**Incluye**

- Baneo automático por banda alta
- Reversibilidad
- Registro de la sanción

**No incluye**

- Apelaciones formales
- Sanciones económicas

## 3. Actores

- Usuario sancionado
- Moderador
- Administrador

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-020-01 | Aplicar baneo cuando la banda sea alta | Must |
| FR-020-02 | Permitir revertir una sanción | Must |
| FR-020-03 | Registrar motivo, evidencia y responsable | Must |
| FR-020-04 | Bloquear el acceso del usuario sancionado | Must |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-020-01 | Efectividad del bloqueo | < 1 s |
| NFR-020-02 | Reversibilidad | Sin pérdida de datos del usuario |

## 6. Criterios de aceptación

### CA-020-01

- **Dado** decisión de banda alta
- **Cuando** se aplica la sanción
- **Entonces** usuario bloqueado y sanción registrada

### CA-020-02

- **Dado** sanción revertida
- **Cuando** el usuario intenta acceder
- **Entonces** acceso restaurado

### CA-020-03

- **Dado** usuario sancionado
- **Cuando** intenta login
- **Entonces** 403 con mensaje claro

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| — | — | — |

## 8. Dependencias y supuestos

- Depende de: 018, 019
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- Un falso positivo de moderación sanciona a un usuario inocente

## 10. Preguntas abiertas

- ¿Cuánto dura una sanción por defecto?
- ¿Existe proceso de apelación?

## 11. Trazabilidad

- Requisitos de este spec: `FR-020-01`, `FR-020-02`, `FR-020-03`, `FR-020-04`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
