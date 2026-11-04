# Política de seguridad — Sensus Backend

## Versiones soportadas

El backend está en desarrollo activo. Solo la rama desplegable recibe correcciones de seguridad.

| Versión | Soportada |
|---|---|
| Rama desplegable (`main`) | Sí |
| Ramas de funcionalidad y *spikes* | No |

---

## Cómo reportar una vulnerabilidad

**No abras un issue público.** Los reportes públicos exponen a las personas usuarias antes de que exista
un arreglo.

Envía el reporte por un canal privado al responsable del proyecto (equipo de Sensus) con:

1. Descripción del problema y componente afectado.
2. Pasos de reproducción o prueba de concepto.
3. Impacto estimado (confidencialidad, integridad, disponibilidad).
4. Versión o commit afectado.
5. Cualquier mitigación sugerida.

### Compromiso de respuesta

| Etapa | Plazo objetivo |
|---|---|
| Acuse de recibo | 72 horas |
| Clasificación y severidad | 5 días hábiles |
| Corrección o mitigación | Según severidad, con seguimiento |
| Divulgación coordinada | Tras publicar la corrección |

No se ofrece recompensa económica. Se reconoce públicamente la contribución si la persona lo autoriza.

---

## Alcance

**En alcance**

- Autenticación, autorización y gestión de sesiones (JWT, revocación).
- Exposición de datos personales o de contenido moderado.
- Inyección, *bypass* de validación y de moderación.
- Configuración insegura de CORS, TLS, cabeceras o rate limiting.
- Manejo de secretos y de credenciales.
- Dependencias con vulnerabilidades conocidas.

**Fuera de alcance**

- La aplicación móvil, la web y la *landing* (no forman parte de este repositorio).
- Ataques de denegación de servicio volumétricos.
- Ingeniería social sobre el equipo.
- Reportes generados automáticamente sin impacto demostrable.

---

## Reglas de seguridad del proyecto

Estas reglas son normativas y se verifican en cada cambio (`constitution.md` P-IV):

- Autenticación JWT sin estado, validada en firma, emisor, audiencia y expiración.
- Cierre de sesión con invalidación demostrable mediante prueba automatizada.
- TLS 1.2 o superior; HSTS en producción.
- CORS restringido a orígenes explícitos fuera de desarrollo.
- Rate limiting por cliente y por endpoint sensible.
- Secretos solo en AWS Secrets Manager o SSM Parameter Store.
- Sin PII, tokens ni contenido moderado en logs; `correlation-id` obligatorio.
- Moderación **antes** de publicar y **fail-closed** ante fallo o timeout.
- Decisiones de moderación auditables: veredicto, categorías, puntajes, umbral y acción.
- Escaneo de secretos y de dependencias en CI, con fallo bloqueante.

---

## Incidentes conocidos y deuda de seguridad

| Elemento | Estado | Seguimiento |
|---|---|---|
| Secreto JWT versionado en `src/Sensus.Api/appsettings.json` | Abierto | Rotar y mover a configuración segura (specs 001, 002) |
| CORS `AllowAnyOrigin` en `Program.cs` | Abierto | Spec 012 |
| `RequireHttpsMetadata = false` | Abierto | Spec 012 |
| Ausencia de cierre de sesión con revocación | Abierto | Spec 004 |
| Ausencia de rate limiting | Abierto | Spec 013 |
| Ausencia de pruebas de integración | Abierto | Crear `tests/Sensus.IntegrationTests/` |

---

**Alineado a `constitution.md` v1.2.0 y `AGENTS.md` v1.2.0.**
