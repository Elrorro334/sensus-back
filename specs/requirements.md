# Registro de requisitos

Todos los requisitos del proyecto, trazables a su spec. Formato: `FR-<spec>-<nn>` y `NFR-<spec>-<nn>`.

| ID | Spec | Tipo | Requisito | Prioridad / Métrica | Estado |
|---|---|---|---|---|---|
| `FR-001-01` | 001 | FR | Registrar cuenta con correo, nombre completo y contraseña | Must | Implementado |
| `FR-001-02` | 001 | FR | Rechazar correo con formato inválido o ya registrado | Must | Implementado |
| `FR-001-03` | 001 | FR | Exigir contraseña de 8+ caracteres con mayúscula, minúscula y número | Must | Implementado |
| `FR-001-04` | 001 | FR | Almacenar la contraseña con BCrypt y salt, nunca en texto plano | Must | Implementado |
| `FR-001-05` | 001 | FR | Asignar rol Consumidor en un modelo de cuenta unificada | Must | Implementado |
| `FR-001-06` | 001 | FR | Exigir aceptación de términos y registrar la fecha | Must | Implementado |
| `FR-001-07` | 001 | FR | Devolver sesión activa con token JWT de 60 minutos | Must | Implementado |
| `NFR-001-01` | 001 | NFR | Latencia de registro | p95 < 800 ms | Implementado |
| `NFR-001-02` | 001 | NFR | Errores 5xx en registro | < 0.5 % | Implementado |
| `FR-002-01` | 002 | FR | Autenticar con correo y contraseña | Must | Implementado |
| `FR-002-02` | 002 | FR | Emitir JWT con sub, email, name, roles, jti y exp de 60 minutos | Must | Implementado |
| `FR-002-03` | 002 | FR | Responder 401 con mensaje genérico ante credenciales inválidas | Must | Implementado |
| `FR-002-04` | 002 | FR | Actualizar la fecha de último acceso | Should | Implementado |
| `FR-002-05` | 002 | FR | Rechazar cuentas suspendidas o eliminadas | Must | Implementado |
| `NFR-002-01` | 002 | NFR | Latencia de login | p95 < 600 ms | Implementado |
| `NFR-002-02` | 002 | NFR | Vigencia del token | 60 minutos, configurable por entorno | Implementado |
| `FR-003-01` | 003 | FR | Devolver el perfil del usuario autenticado | Must | Implementado |
| `FR-003-02` | 003 | FR | Excluir passwordHash y datos sensibles de la respuesta | Must | Implementado |
| `FR-003-03` | 003 | FR | Responder 401 sin token o con token inválido | Must | Implementado |
| `FR-003-04` | 003 | FR | Actualizar nombre completo y avatar del propio usuario | Must | Implementado |
| `FR-003-05` | 003 | FR | Rechazar cambios de correo o roles desde este endpoint | Must | Implementado |
| `FR-003-06` | 003 | FR | Aceptar solo avatares subidos por el propio usuario y aprobados | Must | Implementado |
| `NFR-003-01` | 003 | NFR | Latencia de consulta | p95 < 300 ms | Implementado |
| `NFR-003-02` | 003 | NFR | Latencia de actualización | p95 < 500 ms | Implementado |
| `FR-004-01` | 004 | FR | Exponer POST /api/v1/auth/logout autenticado | Must | Pendiente |
| `FR-004-02` | 004 | FR | Registrar el jti revocado con TTL igual a la expiración del token | Must | Pendiente |
| `FR-004-03` | 004 | FR | Rechazar cualquier petición con token revocado | Must | Pendiente |
| `FR-004-04` | 004 | FR | Responder 204 aunque el token ya estuviera revocado | Should | Pendiente |
| `NFR-004-01` | 004 | NFR | Efectividad de la revocación | < 1 s desde el logout | Pendiente |
| `NFR-004-02` | 004 | NFR | Sobrecarga por petición | < 50 ms | Pendiente |
| `FR-005-01` | 005 | FR | Activar el rol comercial sobre la cuenta autenticada | Must | Implementado |
| `FR-005-02` | 005 | FR | Registrar nombre, categoría, dirección y coordenadas del local | Must | Implementado |
| `FR-005-03` | 005 | FR | Permitir un solo comercio activo por usuario | Must | Implementado |
| `FR-005-04` | 005 | FR | Asignar el rol Merchant tras la activación | Must | Implementado |
| `FR-005-05` | 005 | FR | Validar que la categoría pertenezca al catálogo | Must | Implementado |
| `FR-005-06` | 005 | FR | Validar rangos de latitud y longitud y calcular el punto GeoJSON | Must | Implementado |
| `FR-005-07` | 005 | FR | Rechazar la activación de cuentas suspendidas | Must | Implementado |
| `NFR-005-01` | 005 | NFR | Latencia de activación | p95 < 800 ms | Implementado |
| `NFR-005-02` | 005 | NFR | Precisión de coordenadas | 6 decimales, error < 1 m | Implementado |
| `FR-006-01` | 006 | FR | Devolver el comercio asociado al usuario autenticado | Must | Implementado |
| `FR-006-02` | 006 | FR | Responder 404 si el usuario no tiene comercio | Must | Implementado |
| `FR-006-03` | 006 | FR | Impedir la consulta de comercios de otros usuarios | Must | Implementado |
| `FR-006-04` | 006 | FR | Incluir estado del comercio y coordenadas normalizadas | Should | Implementado |
| `NFR-006-01` | 006 | NFR | Latencia de consulta | p95 < 300 ms | Implementado |
| `FR-007-01` | 007 | FR | Listar ofertas publicadas dentro de un radio de la ubicación enviada | Must | Pendiente |
| `FR-007-02` | 007 | FR | Excluir ofertas vencidas, pausadas, archivadas o sin stock | Must | Pendiente |
| `FR-007-03` | 007 | FR | Filtrar por categoría del catálogo | Should | Pendiente |
| `FR-007-04` | 007 | FR | Aplicar pageSize 20 por defecto y 100 máximo | Must | Pendiente |
| `FR-007-05` | 007 | FR | Ordenar por distancia ascendente y desempatar por fecha de publicación | Must | Pendiente |
| `FR-007-06` | 007 | FR | Limitar el radio a 50 km y rechazar valores fuera de rango | Must | Pendiente |
| `FR-007-07` | 007 | FR | Devolver la distancia en metros junto a cada oferta | Should | Pendiente |
| `NFR-007-01` | 007 | NFR | Latencia del listado | p95 < 500 ms | Pendiente |
| `NFR-007-02` | 007 | NFR | Consulta geoespacial | Sin recorrido completo de colección (índice 2dsphere) | Pendiente |
| `FR-008-01` | 008 | FR | Eliminar el grupo de rutas /api/v1/comercio | Must | Pendiente |
| `FR-008-02` | 008 | FR | Eliminar el alias /api/v1/comercios | Must | Pendiente |
| `FR-008-03` | 008 | FR | Devolver 404 en las rutas retiradas | Must | Pendiente |
| `FR-008-04` | 008 | FR | Actualizar README, Sensus.Api.http y OpenAPI | Must | Pendiente |
| `FR-008-05` | 008 | FR | No dejar referencias a rutas en español en el código | Must | Pendiente |
| `NFR-008-01` | 008 | NFR | Rutas duplicadas | Cero en el contrato OpenAPI | Pendiente |
| `NFR-008-02` | 008 | NFR | Cobertura del contrato | 100 % de rutas bajo /api/v1 en inglés | Pendiente |
| `FR-009-01` | 009 | FR | Toda respuesta de error usa ProblemDetails | Must | Pendiente |
| `FR-009-02` | 009 | FR | Incluir traceId o correlationId en cada error | Must | Pendiente |
| `FR-009-03` | 009 | FR | No exponer stack traces ni nombres de colecciones | Must | Pendiente |
| `FR-009-04` | 009 | FR | Mantener un catálogo de códigos de error documentado | Should | Pendiente |
| `NFR-009-01` | 009 | NFR | Estabilidad del formato | Sin cambios incompatibles dentro de v1 | Pendiente |
| `FR-010-01` | 010 | FR | Exponer PagedResult con items, page, pageSize, total y totalPages | Must | Pendiente |
| `FR-010-02` | 010 | FR | Aplicar 20 elementos por defecto y 100 como máximo | Must | Pendiente |
| `FR-010-03` | 010 | FR | Rechazar pageSize fuera de rango con 400 | Must | Pendiente |
| `FR-010-04` | 010 | FR | Aplicar el contrato a todo endpoint de colección | Must | Pendiente |
| `NFR-010-01` | 010 | NFR | Sobrecarga de contar total | p95 < 100 ms adicionales | Pendiente |
| `FR-011-01` | 011 | FR | Exponer el documento OpenAPI en desarrollo | Must | Pendiente |
| `FR-011-02` | 011 | FR | Documentar todos los endpoints con esquemas y códigos de respuesta | Must | Pendiente |
| `FR-011-03` | 011 | FR | Validar el documento en CI y fallar ante cambios incompatibles | Must | Pendiente |
| `FR-011-04` | 011 | FR | Versionar el contrato junto con la API | Should | Pendiente |
| `NFR-011-01` | 011 | NFR | Cobertura del contrato | 100 % de endpoints publicados | Pendiente |
| `FR-012-01` | 012 | FR | Definir orígenes permitidos por configuración y entorno | Must | Pendiente |
| `FR-012-02` | 012 | FR | Prohibir AllowAnyOrigin fuera de desarrollo | Must | Pendiente |
| `FR-012-03` | 012 | FR | Redirigir a HTTPS fuera de desarrollo | Must | Pendiente |
| `FR-012-04` | 012 | FR | Habilitar HSTS en producción | Should | Pendiente |
| `NFR-012-01` | 012 | NFR | Orígenes permitidos | Lista explícita, cero comodines en producción | Pendiente |
| `FR-013-01` | 013 | FR | Limitar peticiones por IP | Must | Pendiente |
| `FR-013-02` | 013 | FR | Limitar peticiones por usuario autenticado | Must | Pendiente |
| `FR-013-03` | 013 | FR | Responder 429 con Retry-After | Must | Pendiente |
| `FR-013-04` | 013 | FR | Configurar umbrales por entorno sin recompilar | Must | Pendiente |
| `FR-013-05` | 013 | FR | Aplicar límites más estrictos en autenticación | Should | Pendiente |
| `NFR-013-01` | 013 | NFR | Sobrecarga del limitador | < 10 ms por petición | Pendiente |
| `NFR-013-02` | 013 | NFR | Disponibilidad ante picos | Sin degradar a 5xx | Pendiente |
| `FR-014-01` | 014 | FR | Emitir logs en JSON estructurado | Must | Pendiente |
| `FR-014-02` | 014 | FR | Aceptar o generar correlation-id por petición | Must | Pendiente |
| `FR-014-03` | 014 | FR | Excluir PII, tokens y contenido moderado | Must | Pendiente |
| `FR-014-04` | 014 | FR | Registrar método, ruta, estado y duración | Should | Pendiente |
| `NFR-014-01` | 014 | NFR | Campos obligatorios | timestamp, level, correlationId, message | Pendiente |
| `NFR-014-02` | 014 | NFR | Sobrecarga | < 5 ms | Pendiente |
| `FR-015-01` | 015 | FR | Publicar métricas de latencia, errores y throttling | Must | Pendiente |
| `FR-015-02` | 015 | FR | Definir alarmas en IaC con umbrales explícitos | Must | Pendiente |
| `FR-015-03` | 015 | FR | Notificar alarmas a un canal definido | Should | Pendiente |
| `NFR-015-01` | 015 | NFR | Coste de alarmas | Dentro del Free Tier (10 alarmas) | Pendiente |
| `NFR-015-02` | 015 | NFR | Latencia de detección | < 5 min | Pendiente |
| `FR-016-01` | 016 | FR | Exponer /health/live sin dependencias externas | Must | Pendiente |
| `FR-016-02` | 016 | FR | Exponer /health/ready verificando MongoDB | Must | Pendiente |
| `FR-016-03` | 016 | FR | No exponer cadenas de conexión ni versiones | Must | Pendiente |
| `NFR-016-01` | 016 | NFR | Latencia de readiness | < 500 ms | Pendiente |
| `FR-017-01` | 017 | FR | Enviar imagen y texto en la misma solicitud de moderación | Must | Pendiente |
| `FR-017-02` | 017 | FR | Consumir omni-moderation-latest con tiempo de espera acotado | Must | Pendiente |
| `FR-017-03` | 017 | FR | Interpretar flagged, categories y category_scores de las seis subcategorías | Must | Pendiente |
| `FR-017-04` | 017 | FR | No publicar si el proveedor falla, expira o responde de forma inválida | Must | Pendiente |
| `FR-017-05` | 017 | FR | Registrar veredicto, categorías, puntajes y acción adoptada | Must | Pendiente |
| `FR-017-06` | 017 | FR | Moderar toda imagen de oferta antes de exponerla públicamente | Must | Pendiente |
| `FR-017-07` | 017 | FR | Exponer el resultado al flujo que lo invoca, sin endpoint público de moderación | Must | Pendiente |
| `NFR-017-01` | 017 | NFR | Latencia de moderación | p95 < 3 s | Pendiente |
| `NFR-017-02` | 017 | NFR | Disponibilidad del flujo | Fail-closed al 100 % | Pendiente |
| `FR-018-01` | 018 | FR | Definir banda baja, intermedia y alta por categoría | Must | Pendiente |
| `FR-018-02` | 018 | FR | Externalizar los umbrales en configuración | Must | Pendiente |
| `FR-018-03` | 018 | FR | Registrar qué umbral se aplicó en cada decisión | Must | Pendiente |
| `FR-018-04` | 018 | FR | Permitir ajustar umbrales sin recompilar | Should | Pendiente |
| `NFR-018-01` | 018 | NFR | Trazabilidad | 100 % de decisiones con umbral registrado | Pendiente |
| `FR-019-01` | 019 | FR | Marcar el contenido no aprobado como PendienteRevision | Must | Pendiente |
| `FR-019-02` | 019 | FR | Impedir el acceso público al contenido en cuarentena | Must | Pendiente |
| `FR-019-03` | 019 | FR | Definir y aplicar retención máxima | Must | Pendiente |
| `FR-019-04` | 019 | FR | Permitir la liberación del contenido tras aprobación | Should | Pendiente |
| `NFR-019-01` | 019 | NFR | Retención | Máximo definido en la spec (propuesta: 30 días) | Pendiente |
| `NFR-019-02` | 019 | NFR | Coste de almacenamiento | Dentro del Free Tier de S3 | Pendiente |
| `FR-020-01` | 020 | FR | Aplicar baneo cuando la banda sea alta | Must | Pendiente |
| `FR-020-02` | 020 | FR | Permitir revertir una sanción | Must | Pendiente |
| `FR-020-03` | 020 | FR | Registrar motivo, evidencia y responsable | Must | Pendiente |
| `FR-020-04` | 020 | FR | Bloquear el acceso del usuario sancionado | Must | Pendiente |
| `NFR-020-01` | 020 | NFR | Efectividad del bloqueo | < 1 s | Pendiente |
| `NFR-020-02` | 020 | NFR | Reversibilidad | Sin pérdida de datos del usuario | Pendiente |
| `FR-021-01` | 021 | FR | Registrar cada decisión con veredicto, puntajes y umbral | Must | Pendiente |
| `FR-021-02` | 021 | FR | Impedir la modificación de registros de auditoría | Must | Pendiente |
| `FR-021-03` | 021 | FR | Permitir consulta paginada por administrador | Must | Pendiente |
| `FR-021-04` | 021 | FR | Definir retención conforme a la política de datos | Must | Pendiente |
| `NFR-021-01` | 021 | NFR | Integridad | Registros sin actualización ni borrado manual | Pendiente |
| `NFR-021-02` | 021 | NFR | Latencia de consulta | p95 < 800 ms | Pendiente |
| `FR-022-01` | 022 | FR | Aceptar una imagen multipart y devolver su identificador | Must | Pendiente |
| `FR-022-02` | 022 | FR | Validar tipo MIME real (JPEG, PNG, WebP) por firma de bytes | Must | Pendiente |
| `FR-022-03` | 022 | FR | Rechazar archivos mayores a 4,5 MB con 413 | Must | Pendiente |
| `FR-022-04` | 022 | FR | Almacenar el archivo de forma privada hasta su aprobación | Must | Pendiente |
| `FR-022-05` | 022 | FR | Registrar propietario, estado y clave de S3 | Must | Pendiente |
| `FR-022-06` | 022 | FR | Moderar la imagen antes de permitir su uso público | Must | Pendiente |
| `FR-022-07` | 022 | FR | Limitar la carga a 20 archivos por usuario y día | Should | Pendiente |
| `NFR-022-01` | 022 | NFR | Tamaño máximo por archivo | 4,5 MB | Pendiente |
| `NFR-022-02` | 022 | NFR | Latencia de carga | p95 < 2 s para 1 MB | Pendiente |
| `NFR-022-03` | 022 | NFR | Coste de almacenamiento | Dentro del Free Tier de S3 (5 GB) | Pendiente |
| `FR-023-01` | 023 | FR | Servir multimedia mediante CloudFront | Must | Pendiente |
| `FR-023-02` | 023 | FR | Firmar URLs cuando el contenido sea privado | Must | Pendiente |
| `FR-023-03` | 023 | FR | Aplicar cabeceras de caché por tipo de contenido | Should | Pendiente |
| `FR-023-04` | 023 | FR | Invalidar caché al retirar contenido | Should | Pendiente |
| `NFR-023-01` | 023 | NFR | Latencia de entrega | p95 < 200 ms en caché | Pendiente |
| `NFR-023-02` | 023 | NFR | Coste | Dentro del Free Tier de CloudFront | Pendiente |
| `FR-024-01` | 024 | FR | Crear publicación con texto de 1 a 500 caracteres e imagen opcional | Must | Pendiente |
| `FR-024-02` | 024 | FR | No publicar hasta obtener veredicto favorable | Must | Pendiente |
| `FR-024-03` | 024 | FR | Registrar autor, fechas y estado | Must | Pendiente |
| `FR-024-04` | 024 | FR | Devolver 202 mientras la moderación está pendiente | Should | Pendiente |
| `FR-024-05` | 024 | FR | Permitir al autor eliminar su propia publicación | Must | Pendiente |
| `FR-024-06` | 024 | FR | Permitir a un administrador eliminar cualquier publicación | Should | Pendiente |
| `FR-024-07` | 024 | FR | Excluir del feed las publicaciones eliminadas de inmediato | Must | Pendiente |
| `NFR-024-01` | 024 | NFR | Latencia percibida | p95 < 1 s hasta aceptar la solicitud | Pendiente |
| `NFR-024-02` | 024 | NFR | Consistencia | Sin publicaciones sin veredicto | Pendiente |
| `FR-025-01` | 025 | FR | Crear comentario en una publicación existente y visible | Must | Pendiente |
| `FR-025-02` | 025 | FR | Moderar el texto antes de publicarlo | Must | Pendiente |
| `FR-025-03` | 025 | FR | Listar comentarios paginados por publicación | Must | Pendiente |
| `FR-025-04` | 025 | FR | Rechazar comentarios en publicaciones no visibles con 404 | Must | Pendiente |
| `FR-025-05` | 025 | FR | Limitar el comentario a 300 caracteres | Must | Pendiente |
| `FR-025-06` | 025 | FR | Ordenar los comentarios por fecha ascendente | Should | Pendiente |
| `NFR-025-01` | 025 | NFR | Latencia de listado | p95 < 400 ms | Pendiente |
| `NFR-025-02` | 025 | NFR | Paginación | 20 por defecto, 100 máximo | Pendiente |
| `FR-026-01` | 026 | FR | Listar publicaciones publicadas dentro del radio, en orden cronológico inverso | Must | Pendiente |
| `FR-026-02` | 026 | FR | Excluir contenido en cuarentena, rechazado o eliminado | Must | Pendiente |
| `FR-026-03` | 026 | FR | Aplicar paginación estándar | Must | Pendiente |
| `FR-026-04` | 026 | FR | Limitar el radio a 50 km y rechazar valores fuera de rango | Must | Pendiente |
| `FR-026-05` | 026 | FR | Excluir publicaciones sin ubicación del feed cercano | Must | Pendiente |
| `FR-026-06` | 026 | FR | Usar caché de corta duración para páginas recurrentes | Should | Pendiente |
| `NFR-026-01` | 026 | NFR | Latencia del feed | p95 < 300 ms con caché | Pendiente |
| `NFR-026-02` | 026 | NFR | Índice | Compuesto por status + location + createdAt | Pendiente |
| `FR-027-01` | 027 | FR | Registrar token de dispositivo con plataforma y deviceId | Must | Pendiente |
| `FR-027-02` | 027 | FR | Actualizar el token existente sin duplicar el dispositivo | Must | Pendiente |
| `FR-027-03` | 027 | FR | Despachar notificaciones mediante IPushDispatcher | Must | Pendiente |
| `FR-027-04` | 027 | FR | Seleccionar el adaptador FCM o SNS por configuración | Must | Pendiente |
| `FR-027-05` | 027 | FR | Eliminar tokens rechazados por el proveedor | Must | Pendiente |
| `FR-027-06` | 027 | FR | Reasignar un token que cambia de usuario sin dejar acceso residual | Must | Pendiente |
| `FR-027-07` | 027 | FR | No fallar si el usuario no tiene dispositivos activos | Should | Pendiente |
| `NFR-027-01` | 027 | NFR | Latencia de registro | p95 < 300 ms | Pendiente |
| `NFR-027-02` | 027 | NFR | Latencia de despacho | p95 < 2 s | Pendiente |
| `NFR-027-03` | 027 | NFR | Acoplamiento al proveedor | Cero referencias al SDK fuera de Infrastructure | Pendiente |
| `FR-028-01` | 028 | FR | Exponer un endpoint de recepción de eventos | Must | Backlog |
| `FR-028-02` | 028 | FR | Verificar la firma del emisor | Must | Backlog |
| `FR-028-03` | 028 | FR | Garantizar idempotencia por eventId | Must | Backlog |
| `FR-028-04` | 028 | FR | Responder 2xx en menos de 1 s y procesar de forma asíncrona | Must | Backlog |
| `FR-028-05` | 028 | FR | Registrar eventos fallidos para reintento | Should | Backlog |
| `NFR-028-01` | 028 | NFR | Latencia de respuesta | p95 < 500 ms | Backlog |
| `NFR-028-02` | 028 | NFR | Duplicados procesados | Cero efectos duplicados | Backlog |
| `FR-029-01` | 029 | FR | Exponer POST /api/v1/auth/recover y responder 202 siempre | Must | Pendiente |
| `FR-029-02` | 029 | FR | Generar token de 32 bytes, almacenar solo su hash y caducarlo en 30 minutos | Must | Pendiente |
| `FR-029-03` | 029 | FR | Invalidar tokens anteriores del mismo usuario al emitir uno nuevo | Must | Pendiente |
| `FR-029-04` | 029 | FR | Enviar el enlace por correo mediante Amazon SES | Must | Pendiente |
| `FR-029-05` | 029 | FR | Exponer POST /api/v1/auth/recover/confirm para fijar la nueva contraseña | Must | Pendiente |
| `FR-029-06` | 029 | FR | Rechazar tokens expirados, ya usados o inexistentes con 400 | Must | Pendiente |
| `FR-029-07` | 029 | FR | Revocar todas las sesiones activas tras el cambio de contraseña | Must | Pendiente |
| `NFR-029-01` | 029 | NFR | Latencia de solicitud | p95 < 700 ms | Pendiente |
| `NFR-029-02` | 029 | NFR | Enumeración de cuentas | Cero diferencia de respuesta entre correo existente e inexistente | Pendiente |
| `NFR-029-03` | 029 | NFR | Límite de solicitudes | 3 por correo y hora | Pendiente |
| `FR-030-01` | 030 | FR | Exponer DELETE /api/v1/users/account con confirmación explícita y contraseña | Must | Pendiente |
| `FR-030-02` | 030 | FR | Marcar la cuenta como deleted y revocar todas sus sesiones de inmediato | Must | Pendiente |
| `FR-030-03` | 030 | FR | Impedir el login y cualquier operación autenticada tras la eliminación | Must | Pendiente |
| `FR-030-04` | 030 | FR | Purgar los datos personales a los 30 días mediante proceso programado | Must | Pendiente |
| `FR-030-05` | 030 | FR | Anonimizar publicaciones y comentarios del usuario en lugar de conservarlos asociados | Must | Pendiente |
| `FR-030-06` | 030 | FR | Desactivar el comercio y sus ofertas activas al eliminar una cuenta comercial | Must | Pendiente |
| `FR-030-07` | 030 | FR | Conservar la auditoría de moderación anonimizada durante 90 días | Must | Pendiente |
| `NFR-030-01` | 030 | NFR | Efectividad del bloqueo | < 1 s desde la eliminación | Pendiente |
| `NFR-030-02` | 030 | NFR | Purga | 100 % de cuentas marcadas, en ≤ 31 días | Pendiente |
| `NFR-030-03` | 030 | NFR | Trazabilidad | Toda eliminación registrada sin PII | Pendiente |
| `FR-031-01` | 031 | FR | Actualizar los datos editables del comercio propio | Must | Pendiente |
| `FR-031-02` | 031 | FR | Recalcular location y geohash cuando cambien las coordenadas | Must | Pendiente |
| `FR-031-03` | 031 | FR | Validar categoría del catálogo y rangos de coordenadas | Must | Pendiente |
| `FR-031-04` | 031 | FR | Rechazar la edición sobre comercios ajenos con 403 | Must | Pendiente |
| `FR-031-05` | 031 | FR | Registrar quién cambió qué y cuándo | Should | Pendiente |
| `FR-031-06` | 031 | FR | Mantener las ofertas publicadas sin cambios de identificador | Must | Pendiente |
| `NFR-031-01` | 031 | NFR | Latencia de actualización | p95 < 600 ms | Pendiente |
| `NFR-031-02` | 031 | NFR | Consistencia espacial | 100 % de comercios con location válida | Pendiente |
| `FR-032-01` | 032 | FR | Devolver el detalle de una oferta visible con comercio, vigencia, stock y precio | Must | Pendiente |
| `FR-032-02` | 032 | FR | Responder 404 si la oferta no existe o no es visible | Must | Pendiente |
| `FR-032-03` | 032 | FR | Registrar una vista por usuario, oferta y día | Must | Pendiente |
| `FR-032-04` | 032 | FR | Responder 204 a la vista sin revelar datos internos | Must | Pendiente |
| `FR-032-05` | 032 | FR | Indicar si el usuario ya tiene un apartado activo sobre la oferta | Should | Pendiente |
| `FR-032-06` | 032 | FR | No permitir vistas sobre ofertas archivadas | Must | Pendiente |
| `NFR-032-01` | 032 | NFR | Latencia del detalle | p95 < 350 ms | Pendiente |
| `NFR-032-02` | 032 | NFR | Sobrecarga de la vista | < 100 ms y sin bloquear la respuesta | Pendiente |
| `FR-033-01` | 033 | FR | Crear un apartado descontando stock de forma atómica | Must | Pendiente |
| `FR-033-02` | 033 | FR | Rechazar el apartado si no hay stock o la oferta no es visible | Must | Pendiente |
| `FR-033-03` | 033 | FR | Permitir un solo apartado activo por usuario y oferta | Must | Pendiente |
| `FR-033-04` | 033 | FR | Generar un código firmado con claimId, offerId y expiración, y su imagen QR | Must | Pendiente |
| `FR-033-05` | 033 | FR | Caducar el apartado a las 24 horas y devolver el stock | Must | Pendiente |
| `FR-033-06` | 033 | FR | Exponer POST /api/v1/offers/claims/{code}/redeem para el comerciante propietario | Must | Pendiente |
| `FR-033-07` | 033 | FR | Marcar el canje como usado y rechazar un segundo canje del mismo código | Must | Pendiente |
| `FR-033-08` | 033 | FR | Impedir que un comerciante canjee códigos de otro comercio | Must | Pendiente |
| `NFR-033-01` | 033 | NFR | Latencia del apartado | p95 < 700 ms | Pendiente |
| `NFR-033-02` | 033 | NFR | Consistencia de stock | Cero unidades negativas bajo concurrencia | Pendiente |
| `NFR-033-03` | 033 | NFR | Liberación de stock | ≤ 5 min tras la expiración | Pendiente |
| `FR-034-01` | 034 | FR | Crear una oferta asociada al comercio del token | Must | Pendiente |
| `FR-034-02` | 034 | FR | Validar precio, descuento entre 1 y 90 %, stock ≥ 1 y vigencia coherente | Must | Pendiente |
| `FR-034-03` | 034 | FR | Moderar texto e imágenes antes de publicar | Must | Pendiente |
| `FR-034-04` | 034 | FR | Permitir guardar como Draft sin moderación y sin visibilidad pública | Must | Pendiente |
| `FR-034-05` | 034 | FR | Limitar a 20 ofertas activas por comercio | Should | Pendiente |
| `FR-034-06` | 034 | FR | Calcular y persistir el precio final a partir del descuento | Must | Pendiente |
| `FR-034-07` | 034 | FR | Devolver 202 cuando la moderación quede pendiente | Should | Pendiente |
| `NFR-034-01` | 034 | NFR | Latencia de creación | p95 < 1 s sin moderación | Pendiente |
| `NFR-034-02` | 034 | NFR | Publicaciones sin veredicto | Cero | Pendiente |
| `FR-035-01` | 035 | FR | Actualizar los campos editables de una oferta propia | Must | Pendiente |
| `FR-035-02` | 035 | FR | Rechazar la edición de ofertas de otro comercio con 403 | Must | Pendiente |
| `FR-035-03` | 035 | FR | Re-moderar cuando cambie el texto o las imágenes | Must | Pendiente |
| `FR-035-04` | 035 | FR | Impedir reducir el stock por debajo de los apartados activos | Must | Pendiente |
| `FR-035-05` | 035 | FR | Rechazar ediciones concurrentes con 409 usando version | Must | Pendiente |
| `FR-035-06` | 035 | FR | No cambiar el identificador ni los apartados existentes | Must | Pendiente |
| `FR-035-07` | 035 | FR | Mantener la oferta oculta mientras la re-moderación está pendiente | Must | Pendiente |
| `NFR-035-01` | 035 | NFR | Latencia de edición | p95 < 700 ms | Pendiente |
| `NFR-035-02` | 035 | NFR | Pérdida de cambios | Cero sobrescrituras silenciosas | Pendiente |
| `FR-036-01` | 036 | FR | Cambiar el estado de una oferta propia entre Published, Paused y Archived | Must | Pendiente |
| `FR-036-02` | 036 | FR | Rechazar transiciones no permitidas con 409 | Must | Pendiente |
| `FR-036-03` | 036 | FR | Ocultar de inmediato las ofertas pausadas o archivadas | Must | Pendiente |
| `FR-036-04` | 036 | FR | Impedir archivar una oferta con apartados activos | Must | Pendiente |
| `FR-036-05` | 036 | FR | Publicar solo ofertas con moderación aprobada y stock disponible | Must | Pendiente |
| `FR-036-06` | 036 | FR | Registrar estado anterior, nuevo estado, actor y fecha | Must | Pendiente |
| `NFR-036-01` | 036 | NFR | Efectividad del cambio | < 2 s en la consulta de cercanía | Pendiente |
| `NFR-036-02` | 036 | NFR | Auditoría | 100 % de transiciones registradas | Pendiente |
| `FR-037-01` | 037 | FR | Listar las ofertas del comercio autenticado, incluidas las inactivas | Must | Pendiente |
| `FR-037-02` | 037 | FR | Filtrar por estado de la oferta | Should | Pendiente |
| `FR-037-03` | 037 | FR | Incluir apartados activos, canjes y vistas por oferta | Must | Pendiente |
| `FR-037-04` | 037 | FR | Aplicar pageSize 20 por defecto y 100 máximo | Must | Pendiente |
| `FR-037-05` | 037 | FR | Ordenar por fecha de creación descendente | Must | Pendiente |
| `FR-037-06` | 037 | FR | Impedir ver ofertas de otros comercios | Must | Pendiente |
| `NFR-037-01` | 037 | NFR | Latencia del listado | p95 < 600 ms | Pendiente |
| `NFR-037-02` | 037 | NFR | Costo del agregado | Sin recorrido completo por oferta | Pendiente |
| `FR-038-01` | 038 | FR | Alternar el favorito del usuario autenticado sobre una oferta visible | Must | Pendiente |
| `FR-038-02` | 038 | FR | Devolver el estado resultante y el conteo | Must | Pendiente |
| `FR-038-03` | 038 | FR | Rechazar ofertas no visibles con 404 | Must | Pendiente |
| `FR-038-04` | 038 | FR | Garantizar un solo favorito por usuario y oferta | Must | Pendiente |
| `FR-038-05` | 038 | FR | Exponer la afinidad de categoría al ranking de recomendaciones | Should | Pendiente |
| `FR-038-06` | 038 | FR | Limitar la operación a 120 alternancias por usuario y hora | Should | Pendiente |
| `NFR-038-01` | 038 | NFR | Latencia de la operación | p95 < 300 ms | Pendiente |
| `NFR-038-02` | 038 | NFR | Consistencia | Cero conteos negativos | Pendiente |
| `FR-039-01` | 039 | FR | Alternar el me gusta del usuario autenticado sobre una publicación | Must | Pendiente |
| `FR-039-02` | 039 | FR | Devolver el estado resultante y el conteo actualizado | Must | Pendiente |
| `FR-039-03` | 039 | FR | Rechazar publicaciones no visibles con 404 | Must | Pendiente |
| `FR-039-04` | 039 | FR | Garantizar un solo registro por usuario y publicación | Must | Pendiente |
| `FR-039-05` | 039 | FR | Mantener el conteo consistente ante peticiones repetidas | Must | Pendiente |
| `FR-039-06` | 039 | FR | Limitar la operación a 60 alternancias por usuario y hora | Should | Pendiente |
| `NFR-039-01` | 039 | NFR | Latencia de la operación | p95 < 300 ms | Pendiente |
| `NFR-039-02` | 039 | NFR | Consistencia del conteo | Cero conteos negativos | Pendiente |
| `FR-040-01` | 040 | FR | Crear un reporte sobre una oferta, publicación o comentario existente | Must | Pendiente |
| `FR-040-02` | 040 | FR | Aceptar solo motivos tipificados: spam, ofensivo, fraude u otro | Must | Pendiente |
| `FR-040-03` | 040 | FR | Limitar los detalles a 500 caracteres | Must | Pendiente |
| `FR-040-04` | 040 | FR | Permitir un solo reporte por usuario y objetivo cada 24 horas | Must | Pendiente |
| `FR-040-05` | 040 | FR | Marcar el contenido para revisión al alcanzar 3 reportes en 24 horas | Must | Pendiente |
| `FR-040-06` | 040 | FR | Rechazar y no persistir reportes de material de explotación infantil | Must | Pendiente |
| `FR-040-07` | 040 | FR | Impedir que el autor reporte su propio contenido | Should | Pendiente |
| `FR-040-08` | 040 | FR | No revelar al reportado la identidad del reportante | Must | Pendiente |
| `NFR-040-01` | 040 | NFR | Latencia de creación | p95 < 400 ms | Pendiente |
| `NFR-040-02` | 040 | NFR | Privacidad | Cero exposición de la identidad del reportante | Pendiente |
| `FR-041-01` | 041 | FR | Buscar ofertas visibles por texto libre de 2 a 80 caracteres | Must | Pendiente |
| `FR-041-02` | 041 | FR | Calcular un puntaje determinista con pesos de texto, distancia y popularidad | Must | Pendiente |
| `FR-041-03` | 041 | FR | Excluir ofertas vencidas, pausadas o sin stock | Must | Pendiente |
| `FR-041-04` | 041 | FR | Aplicar paginación estándar | Must | Pendiente |
| `FR-041-05` | 041 | FR | Devolver ofertas cercanas cuando la consulta esté vacía | Should | Pendiente |
| `FR-041-06` | 041 | FR | Excluir del resultado las ofertas de comercios inactivos | Must | Pendiente |
| `FR-041-07` | 041 | FR | Versionar y configurar los pesos del ranking por entorno | Must | Pendiente |
| `NFR-041-01` | 041 | NFR | Latencia de búsqueda | p95 < 600 ms | Pendiente |
| `NFR-041-02` | 041 | NFR | Relevancia | ≥ 90 % de consultas con coincidencia exacta de categoría en el top 10 | Pendiente |
| `NFR-041-03` | 041 | NFR | Determinismo | Mismo conjunto de datos y consulta produce el mismo orden | Pendiente |
| `FR-042-01` | 042 | FR | Devolver un máximo de 20 ofertas recomendadas para el usuario | Must | Pendiente |
| `FR-042-02` | 042 | FR | Calcular el puntaje con pesos configurables de proximidad, afinidad, vigencia y popularidad | Must | Pendiente |
| `FR-042-03` | 042 | FR | Derivar la afinidad de favoritos, me gusta y apartados del usuario | Should | Pendiente |
| `FR-042-04` | 042 | FR | Excluir ofertas vencidas, pausadas, sin stock o de comercios inactivos | Must | Pendiente |
| `FR-042-05` | 042 | FR | Aplicar un fallback a ofertas cercanas cuando no haya señales del usuario | Must | Pendiente |
| `FR-042-06` | 042 | FR | Devolver resultados estables para el mismo usuario e insumos | Must | Pendiente |
| `FR-042-07` | 042 | FR | No usar datos personales sensibles en el cálculo | Must | Pendiente |
| `NFR-042-01` | 042 | NFR | Latencia | p95 < 800 ms | Pendiente |
| `NFR-042-02` | 042 | NFR | Cobertura | ≥ 90 % de usuarios con al menos 5 recomendaciones válidas | Pendiente |
| `NFR-042-03` | 042 | NFR | Explicabilidad | 100 % de los pesos documentados y versionados | Pendiente |
| `FR-043-01` | 043 | FR | Devolver alternativas visibles de la misma categoría que la oferta consultada | Must | Pendiente |
| `FR-043-02` | 043 | FR | Limitar la distancia a 10 km de la oferta original | Must | Pendiente |
| `FR-043-03` | 043 | FR | Excluir la oferta original del resultado | Must | Pendiente |
| `FR-043-04` | 043 | FR | Filtrar por precio final dentro del 120 % del precio de la oferta original | Should | Pendiente |
| `FR-043-05` | 043 | FR | Ordenar por cercanía y vigencia | Must | Pendiente |
| `FR-043-06` | 043 | FR | Responder 404 si la oferta original no existe o no es visible | Must | Pendiente |
| `FR-043-07` | 043 | FR | Limitar el resultado a 10 elementos, con 5 por defecto | Must | Pendiente |
| `NFR-043-01` | 043 | NFR | Latencia | p95 < 500 ms | Pendiente |
| `NFR-043-02` | 043 | NFR | Calidad | ≥ 80 % de consultas devuelven al menos 3 alternativas válidas | Pendiente |
| `FR-044-01` | 044 | FR | Devolver las preferencias de notificación del usuario | Must | Pendiente |
| `FR-044-02` | 044 | FR | Actualizar pushEnabled, offerAlerts, socialAlerts y radiusKm | Must | Pendiente |
| `FR-044-03` | 044 | FR | Validar radiusKm entre 1 y 50 | Must | Pendiente |
| `FR-044-04` | 044 | FR | Respetar pushEnabled y las categorías al despachar | Must | Pendiente |
| `FR-044-05` | 044 | FR | Aplicar valores por defecto cuando el usuario no tiene preferencias | Must | Pendiente |
| `FR-044-06` | 044 | FR | Devolver 200 con el estado resultante tras la actualización | Must | Pendiente |
| `NFR-044-01` | 044 | NFR | Latencia de lectura | p95 < 250 ms | Pendiente |
| `NFR-044-02` | 044 | NFR | Respeto de preferencias | 100 % de envíos filtrados según configuración | Pendiente |

Total: **336 requisitos** en 44 specs.
