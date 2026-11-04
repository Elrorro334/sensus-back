# ADR 0007 — Despacho de notificaciones push mediante abstracción

| Campo | Valor |
|---|---|
| Estado | Aceptada |
| Fecha | 2026-10-29 |
| Principios afectados | II, IV, VI |
| Specs relacionados | 027, 044 |

## Contexto

El catálogo de endpoints exige notificaciones push y la aplicación móvil espera Firebase Cloud
Messaging, pero la constitución prohíbe proveedores fuera de AWS salvo excepción explícita. Elegir un
proveedor dentro de `Sensus.Api` o `Sensus.Application` acoplaría el dominio a un SDK externo y haría
costoso cualquier cambio.

## Decisión

- Definir `IPushDispatcher` en `Sensus.Domain` y el modelo de notificación en `Sensus.Application`.
- Implementar dos adaptadores en `Sensus.Infrastructure`: FCM y Amazon SNS.
- Seleccionar el adaptador por configuración de entorno (`Push:Provider`), con FCM o SNS como valores
  válidos y sin ramas condicionales fuera de la composición en `Program.cs`.
- Tratar el token de dispositivo como dato opaco del adaptador; el dominio no conoce formatos de FCM ni
  de SNS.
- Respetar las preferencias del usuario (spec 044) **antes** de invocar el despachador.

## Consecuencias

**Positivas**

- Cambiar de proveedor no toca dominio ni aplicación.
- Las pruebas usan un despachador simulado y no dependen de credenciales externas.
- Se cumple la regla de dependencias de la capa `Domain`.

**Negativas**

- Una indirección adicional y dos adaptadores que mantener.
- Las particularidades de cada proveedor (tokens inválidos, prioridades, cargas máximas) deben
  normalizarse en la interfaz.

**Mitigación**

- Contrato mínimo de despacho y pruebas de contrato por adaptador.
- Depuración automática de tokens rechazados (spec 027).

## Alternativas consideradas

| Alternativa | Motivo del descarte |
|---|---|
| FCM directo en Application | Acopla el dominio a un SDK externo |
| Solo Amazon SNS | Cumple la constitución, pero ignora la expectativa del cliente móvil y complica el registro de tokens |
| Push desde la app con un tercero | Fuera del alcance del backend y sin auditoría central |

## Referencias

- `constitution.md` §2.1 y §2.2
- `specs/027-registro-dispositivo/`
- `specs/044-ajustes-notificaciones/`
