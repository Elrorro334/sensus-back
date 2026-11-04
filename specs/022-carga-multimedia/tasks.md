# Tareas 022 — Carga de multimedia

| Campo | Valor |
|---|---|
| Spec | [`spec.md`](spec.md) |
| Plan | [`plan.md`](plan.md) |
| Checklist | [`checklist.md`](checklist.md) |
| Estado | Pendiente |
| Alineado a | `constitution.md` v1.2.0 · `AGENTS.md` v1.2.0 |

## Tareas

| ID | Tarea | Capa | Requisito | Depende | Estado |
|---|---|---|---|---|---|
| T022-01 | Actualizar y validar el contrato OpenAPI del spec | Api | `FR-022-01` | — | Pendiente |
| T022-02 | Implementar: UploadMediaCommand y validador de tipo y tamaño | Application | `FR-022-01` | T022-01 | Pendiente |
| T022-03 | Implementar: Cliente S3 y verificación de firma de bytes | Infrastructure | `FR-022-01` | T022-02 | Pendiente |
| T022-04 | Implementar: Endpoint multipart POST /api/v1/media/upload | Api | `FR-022-01` | T022-03 | Pendiente |
| T022-05 | Validar MIME real por firma de bytes | Application | `FR-022-02` | T022-04 | Pendiente |
| T022-06 | Implementar el almacenamiento privado en S3 | Infrastructure | `FR-022-04` | T022-05 | Pendiente |
| T022-07 | Probar el límite de 4,5 MB y el 413 | Api | `FR-022-03` | T022-06 | Pendiente |
| T022-08 | Añadir la limpieza de archivos huérfanos | Infrastructure | `FR-022-05` | T022-07 | Pendiente |
| T022-09 | Implementar las pruebas de integración del flujo completo | Pruebas | `FR-022-01` | T022-08 | Pendiente |
| T022-10 | Actualizar `specs/README.md`, `requirements.md` y el checklist del spec | Docs | `FR-022-01` | T022-09 | Pendiente |
| T022-11 | Registrar evidencias del Constitution Check | Docs | `FR-022-01` | T022-10 | Pendiente |

## Notas de ejecución

- Cada tarea se cierra actualizando `checklist.md` y `plan.md` si cambia el diseño.
- Ninguna tarea se considera terminada sin pruebas verdes y cobertura dentro del umbral (P-V).
- Los identificadores de requisito remiten a `spec.md` §4 y §5.
