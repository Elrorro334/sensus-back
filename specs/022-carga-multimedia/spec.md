# Spec 022 — Carga de multimedia

| Campo | Valor |
|---|---|
| ID | 022 |
| Slug | `carga-multimedia` |
| Estado | Pendiente |
| Ola | 4 |
| Tipo | Nuevo |
| Dueño | Por asignar |
| Dependencias | 017, 013 |
| Versión | 1.1.0 |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

---

## 1. Contexto y problema

Permitir la carga de imágenes a través de la API con validación estricta de tipo y tamaño, almacenamiento privado y moderación antes de exponerlas.

## 2. Alcance

**Incluye**

- Endpoint multipart POST /api/v1/media/upload
- Validación de MIME real y tamaño ≤ 4,5 MB
- Almacenamiento privado en S3
- Asociación con el usuario y el contenido
- Moderación de la imagen

**No incluye**

- Vídeo
- Edición de imágenes
- Miniaturas automáticas
- Carga directa con URL prefirmada

## 3. Actores

- Usuario autenticado
- Cliente móvil

## 4. Requisitos funcionales

| ID | Requisito | Prioridad |
|---|---|---|
| FR-022-01 | Aceptar una imagen multipart y devolver su identificador | Must |
| FR-022-02 | Validar tipo MIME real (JPEG, PNG, WebP) por firma de bytes | Must |
| FR-022-03 | Rechazar archivos mayores a 4,5 MB con 413 | Must |
| FR-022-04 | Almacenar el archivo de forma privada hasta su aprobación | Must |
| FR-022-05 | Registrar propietario, estado y clave de S3 | Must |
| FR-022-06 | Moderar la imagen antes de permitir su uso público | Must |
| FR-022-07 | Limitar la carga a 20 archivos por usuario y día | Should |

## 5. Requisitos no funcionales

| ID | Requisito | Métrica objetivo |
|---|---|---|
| NFR-022-01 | Tamaño máximo por archivo | 4,5 MB |
| NFR-022-02 | Latencia de carga | p95 < 2 s para 1 MB |
| NFR-022-03 | Coste de almacenamiento | Dentro del Free Tier de S3 (5 GB) |

## 6. Criterios de aceptación

### CA-022-01

- **Dado** JPEG de 1 MB
- **Cuando** POST /api/v1/media/upload
- **Entonces** 201 con mediaId y estado

### CA-022-02

- **Dado** archivo de 6 MB
- **Cuando** POST /api/v1/media/upload
- **Entonces** 413 sin almacenar

### CA-022-03

- **Dado** archivo .exe renombrado a .jpg
- **Cuando** POST /api/v1/media/upload
- **Entonces** 400 por firma inválida

### CA-022-04

- **Dado** imagen sin moderar
- **Cuando** se solicita su URL pública
- **Entonces** 403

### CA-022-05

- **Dado** 21ª carga del día
- **Cuando** POST /api/v1/media/upload
- **Entonces** 429

## 7. Contratos expuestos

| Método | Ruta | Autorización |
|---|---|---|
| POST | `/api/v1/media/upload` | JWT |

## 8. Dependencias y supuestos

- Depende de: 017, 013
- El comportamiento descrito aplica a la versión v1 de la API.
- Los secretos y cadenas de conexión se resuelven por configuración de entorno, nunca en código.

## 9. Riesgos

- El límite de 6 MB de Lambda hace inviable subir archivos mayores
- Archivos huérfanos si el usuario no los usa

## 10. Preguntas abiertas

- ¿Se requiere una migración posterior a URL prefirmada para archivos grandes?

## 11. Trazabilidad

- Requisitos de este spec: `FR-022-01`, `FR-022-02`, `FR-022-03`, `FR-022-04`, `FR-022-05`, `FR-022-06`, `FR-022-07`
- Plan: [`plan.md`](plan.md)
- Tareas: [`tasks.md`](tasks.md)
- Checklist: [`checklist.md`](checklist.md)
