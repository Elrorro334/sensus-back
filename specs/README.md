# Índice de especificaciones — Sensus Backend

Cada spec es una unidad independiente de alcance con su propio `spec.md`, `plan.md`, `tasks.md` y `checklist.md`.
La numeración es estable: un spec no cambia de ID una vez creado.

| ID | Spec | Área | Tipo | Ola | Estado | Dependencias |
|---|---|---|---|---|---|---|
| 001 | [Registro de cuenta unificada](001-registro-usuario/spec.md) | Autenticación | Retro-spec | 1 | Implementado (retro-spec) | Ninguna |
| 002 | [Inicio de sesión](002-inicio-sesion/spec.md) | Autenticación | Retro-spec | 1 | Implementado (retro-spec) | 001 |
| 003 | [Perfil del usuario autenticado](003-perfil-usuario/spec.md) | Autenticación | Retro-spec | 1 | Implementado (retro-spec) | 002, 022 |
| 004 | [Cierre de sesión y revocación de token](004-cierre-sesion-y-revocacion/spec.md) | Autenticación | Nuevo | 6 | Pendiente | 002 |
| 005 | [Activación del rol comercial](005-activacion-comerciante/spec.md) | Comercios | Retro-spec | 2 | Implementado (retro-spec) | 001, 002 |
| 006 | [Consulta de mi comercio](006-consulta-mi-comercio/spec.md) | Comercios | Retro-spec | 2 | Implementado (retro-spec) | 005 |
| 007 | [Ofertas cercanas](007-ofertas-cercanas/spec.md) | Ofertas | Nuevo | 2 | Pendiente | 005, 010 |
| 008 | [Retiro de endpoints legacy](008-retiro-endpoints-legacy/spec.md) | Plataforma | Nuevo | 6 | Pendiente | 001, 005, 006 |
| 009 | [Errores uniformes con ProblemDetails](009-errores-problemdetails/spec.md) | Plataforma | Nuevo | 0 | Parcial | Ninguna |
| 010 | [Paginación estándar](010-paginacion-estandar/spec.md) | Plataforma | Nuevo | 0 | Parcial | 009 |
| 011 | [Contrato OpenAPI](011-contrato-openapi/spec.md) | Plataforma | Nuevo | 0 | Parcial | 009, 010 |
| 012 | [CORS y HTTPS por entorno](012-cors-y-https/spec.md) | Seguridad | Nuevo | 6 | Pendiente | Ninguna |
| 013 | [Control de tasa de peticiones](013-rate-limiting/spec.md) | Seguridad | Nuevo | 6 | Pendiente | 012 |
| 014 | [Logs estructurados y correlación](014-logs-estructurados-correlacion/spec.md) | Observabilidad | Nuevo | 6 | Pendiente | 009 |
| 015 | [Métricas y alarmas](015-metricas-y-alarmas/spec.md) | Observabilidad | Nuevo | 6 | Pendiente | 014 |
| 016 | [Health checks](016-health-checks/spec.md) | Observabilidad | Nuevo | 6 | Parcial | 014 |
| 017 | [Moderación multimodal de contenido](017-moderacion-multimodal/spec.md) | Moderación | Nuevo | 4 | Pendiente | 009, 022 |
| 018 | [Umbrales y bandas de decisión](018-umbrales-y-bandas/spec.md) | Moderación | Nuevo | 4 | Pendiente | 017 |
| 019 | [Cuarentena de contenido](019-cuarentena-contenido/spec.md) | Moderación | Nuevo | 4 | Pendiente | 017, 018 |
| 020 | [Sanciones y baneo de usuario](020-sanciones-y-baneo/spec.md) | Moderación | Nuevo | 4 | Pendiente | 018, 019 |
| 021 | [Auditoría de moderación](021-auditoria-moderacion/spec.md) | Moderación | Nuevo | 4 | Pendiente | 017, 020 |
| 022 | [Carga de multimedia](022-carga-multimedia/spec.md) | Multimedia | Nuevo | 4 | Pendiente | 017, 013 |
| 023 | [Entrega de contenido por CDN](023-entrega-cdn/spec.md) | Multimedia | Nuevo | 4 | Pendiente | 022 |
| 024 | [Publicaciones del feed](024-publicaciones-feed/spec.md) | Contenido | Nuevo | 3 | Pendiente | 017, 022 |
| 025 | [Comentarios del feed](025-comentarios/spec.md) | Contenido | Nuevo | 3 | Pendiente | 017, 024, 010 |
| 026 | [Feed geolocalizado](026-feed-cercano/spec.md) | Contenido | Nuevo | 3 | Pendiente | 024, 007, 010 |
| 027 | [Registro de dispositivo para push](027-registro-dispositivo/spec.md) | Integraciones | Nuevo | 4 | Pendiente | 003, 013 |
| 028 | [Webhooks entrantes (backlog)](028-webhooks-entrantes/spec.md) | Integraciones | Backlog | Backlog | Backlog | 009, 014 |
| 029 | [Recuperación de contraseña](029-recuperacion-password/spec.md) | Autenticación | Nuevo | 1 | Pendiente | 001, 004, 013 |
| 030 | [Eliminación de cuenta y datos](030-eliminacion-cuenta/spec.md) | Autenticación | Nuevo | 1 | Pendiente | 003, 004, 020 |
| 031 | [Edición del perfil de comercio](031-edicion-perfil-comercio/spec.md) | Comercios | Nuevo | 2 | Pendiente | 005, 006 |
| 032 | [Detalle de oferta y contador de vistas](032-detalle-oferta-y-vistas/spec.md) | Ofertas | Nuevo | 2 | Pendiente | 007 |
| 033 | [Apartado de oferta con código QR](033-apartado-con-qr/spec.md) | Ofertas | Nuevo | 2 | Pendiente | 007, 032, 037 |
| 034 | [Creación de oferta](034-creacion-oferta/spec.md) | Inventario | Nuevo | 2 | Pendiente | 005, 017, 022 |
| 035 | [Edición de oferta](035-edicion-oferta/spec.md) | Inventario | Nuevo | 2 | Pendiente | 034, 033 |
| 036 | [Ciclo de vida de la oferta](036-ciclo-vida-oferta/spec.md) | Inventario | Nuevo | 2 | Pendiente | 034, 035 |
| 037 | [Inventario de ofertas del comerciante](037-inventario-comerciante/spec.md) | Inventario | Nuevo | 2 | Pendiente | 034, 033, 010 |
| 038 | [Favoritos de ofertas](038-favoritos/spec.md) | Recomendaciones | Nuevo | 5 | Pendiente | 032, 042 |
| 039 | [Me gusta en publicaciones](039-me-gusta-publicaciones/spec.md) | Contenido | Nuevo | 3 | Pendiente | 024, 013 |
| 040 | [Reportes de contenido](040-reportes-contenido/spec.md) | Contenido | Nuevo | 3 | Pendiente | 024, 025, 017, 018 |
| 041 | [Búsqueda de ofertas](041-busqueda-inteligente/spec.md) | Recomendaciones | Nuevo | 5 | Pendiente | 007, 010, 011 |
| 042 | [Recomendaciones personalizadas](042-recomendaciones/spec.md) | Recomendaciones | Nuevo | 5 | Pendiente | 007, 038, 039, 041 |
| 043 | [Alternativas a una oferta](043-alternativas-oferta/spec.md) | Recomendaciones | Nuevo | 5 | Pendiente | 032, 041 |
| 044 | [Ajustes de notificaciones](044-ajustes-notificaciones/spec.md) | Integraciones | Nuevo | 4 | Pendiente | 027, 003 |

## Estados

| Estado | Significado |
|---|---|
| Pendiente | Sin trabajo iniciado |
| Parcial | Existe implementación sin spec o con brechas |
| Implementado (retro-spec) | Funcionalidad existente documentada a posteriori |
| Backlog | Capacidad fuera del catálogo vigente, sin compromiso de entrega |

## Reglas

- Un spec nuevo se crea con el siguiente número libre y una carpeta `NNN-slug`.
- No se implementa nada que no esté en un spec con Constitution Check evaluado.
- Todo cambio de alcance se refleja primero en `spec.md`.
- Los requisitos se registran en [`requirements.md`](requirements.md).

Total: **44 specs** — Ola 0: 3, Ola 1: 5, Ola 2: 10, Ola 3: 5, Ola 4: 9, Ola 5: 4, Ola 6: 7, Backlog: 1.
