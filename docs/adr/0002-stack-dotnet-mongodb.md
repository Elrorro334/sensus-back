# ADR 0002 — Stack fijado: .NET 10 LTS, C# 14 y MongoDB 8

| Campo | Valor |
|---|---|
| Estado | Aceptada |
| Fecha | 2026-10-28 |
| Principios afectados | II, VI |
| Specs relacionados | Todos |
| Fuente | `arquitecturaNva.docx` |

## Contexto

El proyecto necesita versiones estables y soportadas a largo plazo. Las dependencias flotantes
(«última versión») hacen que el comportamiento cambie sin que nadie lo decida y rompen la reproducibilidad
del build. La fuente autoritativa fija .NET y MongoDB como base tecnológica y descarta bases relacionales.

## Decisión

| Componente | Versión fijada |
|---|---|
| .NET SDK / runtime | 10.0.401 (`global.json`) |
| Target framework | `net10.0` |
| Lenguaje | C# 14 (`LangVersion 14.0`) |
| MongoDB | 8.x |
| Driver | `MongoDB.Driver` 3.x |

- `global.json` fija el SDK con `rollForward: latestFeature`.
- `Directory.Build.props` centraliza `TargetFramework`, `LangVersion`, `Nullable` e `ImplicitUsings`.
- Las versiones de paquetes se fijan explícitamente en cada `.csproj`.
- Las actualizaciones mayores pasan por un PR con evidencia de pruebas, no por actualización automática.

## Consecuencias

**Positivas**

- Build reproducible en cualquier máquina y en CI.
- Soporte a largo plazo y parches de seguridad previsibles.
- Sin sorpresas por *breaking changes* automáticos.

**Negativas**

- Las actualizaciones requieren trabajo manual planificado.
- El SDK debe estar instalado en todas las máquinas de desarrollo.

**Mitigación**

- Dependabot propone parches y menores; las mayores se ignoran por configuración.
- ADR nueva si se cambia de versión mayor.

## Alternativas consideradas

| Alternativa | Motivo del descarte |
|---|---|
| .NET 9 (STS) | Soporte más corto; obliga a migrar en menos de un año |
| .NET 8 LTS | Ya no es la versión LTS vigente del stack objetivo |
| Versiones flotantes (`*`) | Rompe la reproducibilidad y viola §2.1 |

## Referencias

- `constitution.md` §2.1
- `global.json`, `Directory.Build.props`
- `.github/dependabot.yml`
