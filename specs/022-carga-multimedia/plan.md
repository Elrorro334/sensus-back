# Plan 022 — Carga de multimedia

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Estado del plan | Aprobado |
| Versión | 1.0.0 |
| Constitution Check | PASS |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## 1. Resumen técnico

Permitir la carga de imágenes a través de la API con validación estricta de tipo y tamaño, almacenamiento privado y moderación antes de exponerlas.

## 2. Cambios por capa

| Capa | Cambio |
|---|---|
| Application | UploadMediaCommand y validador de tipo y tamaño |
| Infrastructure | Cliente S3 y verificación de firma de bytes |
| Api | Endpoint multipart POST /api/v1/media/upload |

## 3. Modelo de datos e índices

- media_assets con ownerId, status, s3Key, contentType y createdAt

## 4. Seguridad

- Bucket privado
- Validación de MIME real en servidor
- Sin ejecución de contenido subido
- Límite de tasa por usuario

## 5. Observabilidad y rendimiento

- Métricas de cargas, rechazos y tamaño medio

## 6. Costo AWS

S3 dentro del Free Tier (5 GB); vigilar transferencia y número de peticiones.

## 7. Estrategia de pruebas

- Unitarias de validación de firma y tamaño
- Integración de 201, 400, 413 y 429

## 8. Constitution Check

Evaluación previa a la implementación.

| Principio | Decisión | Evidencia |
|---|---|---|
| I | PASS | `FR-022-01` y 6 requisitos más, con criterios de aceptación medibles |
| II | PASS | Cambios acotados a las capas declaradas en §2 |
| III | PASS | DTOs, comandos y consultas separados; sin exponer entidades de dominio |
| IV | PASS | Reglas de seguridad aplicadas en §4 |
| V | PASS | Estrategia de pruebas definida en §7 |
| VI | PASS | Costo estimado declarado en §6 |
| VII | PASS | Paginación, índices y observabilidad en §5 |

> Sin excepciones registradas (Apéndice E de la constitución).

## 9. Riesgos de implementación

- El límite de 6 MB de Lambda hace inviable subir archivos mayores
- Archivos huérfanos si el usuario no los usa

## 10. Orden de implementación

1. `T022-01` Actualizar y validar el contrato OpenAPI del spec
2. `T022-02` Implementar: UploadMediaCommand y validador de tipo y tamaño
3. `T022-03` Implementar: Cliente S3 y verificación de firma de bytes
4. `T022-04` Implementar: Endpoint multipart POST /api/v1/media/upload
5. `T022-05` Validar MIME real por firma de bytes
6. `T022-06` Implementar el almacenamiento privado en S3
7. `T022-07` Probar el límite de 4,5 MB y el 413
8. `T022-08` Añadir la limpieza de archivos huérfanos
9. `T022-09` Implementar las pruebas de integración del flujo completo
10. `T022-10` Actualizar `specs/README.md`, `requirements.md` y el checklist del spec
11. `T022-11` Registrar evidencias del Constitution Check
