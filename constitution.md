# Constitución del Proyecto Sensus Backend

| Campo | Valor |
|---|---|
| Proyecto | Sensus — Backend API RESTful |
| Documento | Constitución del proyecto (normativo) |
| Versión | **1.2.0** |
| Estado | Ratificada |
| Fecha de ratificación | 2026-10-28 |
| Última enmienda | 2026-10-29 (rutas versionadas, abstracción de proveedores y catálogo de endpoints) |
| Fuente autoritativa | `arquitecturaNva.docx` |
| Documento derogado | `Arquitectura Back.docx` (ver §4.2) |
| Metodología | Spec-Driven Development (SDD) + DevSecOps |
| Ámbito de aplicación | Todo el código, infraestructura, pruebas y documentación del backend Sensus |

---

## Preámbulo

Esta constitución establece los principios no negociables que gobiernan el diseño, la construcción y la
operación del backend Sensus. Es el documento de máxima autoridad del proyecto y prevalece sobre
cualquier especificación, plan, tarea, decisión de implementación o convención local.

**Orden de precedencia:**

```
Constitución  >  spec.md  >  plan.md  >  tasks.md  >  checklist.md  >  código  >  convenciones locales
```

Toda contradicción MUST resolverse a favor del documento de mayor precedencia. Si la constitución
resulta ambigua o incompleta, MUST enmendarse (§4.3) antes de continuar; MUST NOT interpretarse
libremente para acomodar una implementación.

Las palabras clave **MUST**, **MUST NOT**, **SHOULD**, **SHOULD NOT** y **MAY** se interpretan conforme a
RFC 2119.

**Índice**

1. Principios fundamentales (I–VII)
2. Restricciones técnicas y límites de plataforma
3. Flujo de desarrollo SDD y puertas de calidad
4. Gobernanza
5. Apéndices

---

## 1. Principios fundamentales

Los siete principios siguientes son de cumplimiento obligatorio y verificable. Cada uno se compone de
enunciado, reglas normativas, justificación, criterios de verificación y excepciones admitidas.

### Principio I — Especificación primero y trazabilidad

**Enunciado.** Todo cambio en el backend MUST originarse en una especificación aprobada y MUST ser
trazable desde el requisito hasta la prueba que lo verifica.

**Reglas normativas**

- Ningún commit MUST introducir comportamiento no descrito en una especificación aprobada.
- Toda especificación MUST tener un identificador numérico estable de tres dígitos (`NNN`) y residir en
  `specs/NNN-slug/`. El identificador MUST NOT reutilizarse ni renumerarse.
- Toda especificación MUST constar de cuatro artefactos obligatorios en su propia carpeta: `spec.md`,
  `plan.md`, `tasks.md` y `checklist.md`. La ausencia de cualquiera de ellos invalida la especificación.
- Todo requisito MUST tener un identificador estable compuesto por su spec y un número secuencial:
  `FR-<spec>-<nn>` para funcionales y `NFR-<spec>-<nn>` para no funcionales (por ejemplo `FR-017-03`).
  El prefijo de la spec MUST coincidir con el identificador de su carpeta.
- Toda tarea MUST referenciar al menos un requisito, y todo requisito MUST tener al menos una prueba
  asociada registrada en `checklist.md`.
- Toda especificación MUST declarar explícitamente su alcance y sus exclusiones.
- Los criterios de aceptación MUST ser observables y medibles. MUST NOT aceptarse como criterio un
  adjetivo sin métrica («rápido», «seguro», «amigable», «escalable»).
- Un cambio de alcance MUST reflejarse primero en la especificación y solo después en el código.
- Las decisiones de arquitectura relevantes SHOULD registrarse como ADR vinculados a la especificación.

**Justificación.** SDD sustituye la improvisación por contratos verificables. Sin trazabilidad, las
pruebas dejan de demostrar que el sistema hace lo especificado, y el alcance deriva sin control.

**Criterios de verificación**

- Plantilla de PR que exige el identificador de requisito.
- Script de trazabilidad en CI que falla si existe una tarea sin requisito o un requisito sin prueba.
- Revisión de la especificación antes de aprobar el plan.

**Excepciones.** Un *hotfix* de seguridad crítica MAY implementarse antes de la especificación, siempre
que se documente la especificación retroactiva en un plazo máximo de 48 horas.

---

### Principio II — Clean Architecture con dependencias unidireccionales

**Enunciado.** El backend MUST organizarse en cuatro capas —Domain, Application, Infrastructure y
Presentation— con dependencias que apunten exclusivamente hacia el dominio.

**Reglas normativas**

- La capa **Domain** MUST NOT referenciar frameworks, ORMs, SDKs de nube, proveedores de moderación ni
  paquetes de Infrastructure o Presentation.
- Las dependencias MUST respetar esta dirección: `Application → Domain`, `Infrastructure → Application`,
  `Presentation → Application`. Las referencias inversas MUST NOT existir.
- Toda dependencia externa MUST declararse como interfaz en Domain o Application y resolverse por
  inyección de dependencias en el punto de composición.
- Los controladores MUST NOT contener reglas de negocio ni acceso directo a la persistencia.
- Las reglas e invariantes de negocio MUST residir en Domain; la orquestación de casos de uso MUST
  residir en Application.
- Un cambio de proveedor de infraestructura (base de datos, correo, almacenamiento, moderación) MUST NOT
  requerir cambios en Domain ni en Application.

**Justificación.** El desacoplamiento estricto es el requisito central de la arquitectura declarada y la
condición para poder sustituir tecnologías sin reescribir las reglas del negocio.

**Criterios de verificación**

- Test de arquitectura con NetArchTest que falla el build ante cualquier referencia prohibida.
- Checklist de capas en la revisión de PR.
- Pruebas de dominio que se ejecutan sin infraestructura ni contenedores.

**Excepciones.** Ninguna. Cualquier desviación requiere enmienda (§4.3).

---

### Principio III — CQRS y contratos explícitos

**Enunciado.** Los flujos de lectura y escritura MUST modelarse por separado mediante CQRS, y la API
MUST exponer contratos explícitos, nunca el modelo interno.

**Reglas normativas**

- Comandos y consultas MUST modelarse de forma independiente; un handler MUST NOT mezclar escritura y
  lectura.
- La API MUST exponer DTOs. MUST NOT exponerse entidades de dominio, documentos de persistencia ni
  esquemas de MongoDB.
- Todo mapeo entre dominio, aplicación y contrato externo MUST ser explícito y MUST estar cubierto por
  pruebas.
- La validación MUST ocurrir en dos niveles: en el borde (formato, autenticación, autorización) y en el
  dominio (invariantes). La validación de borde MUST NOT sustituir las invariantes de dominio.
- Los contratos REST MUST documentarse en OpenAPI y versionarse; todo cambio incompatible MUST
  publicarse como nueva versión.
- Los errores MUST seguir un formato uniforme basado en ProblemDetails y MUST NOT filtrar trazas,
  nombres de colecciones ni detalles internos.
- Las operaciones de escritura SHOULD ser idempotentes cuando el cliente pueda reintentar.

**Justificación.** CQRS aísla la complejidad de escritura de la optimización de lectura, y los DTOs
evitan que el modelo de persistencia se convierta en contrato público de facto.

**Criterios de verificación**

- Pruebas de contrato sobre el OpenAPI publicado.
- Diff de OpenAPI en CI que detecta cambios incompatibles.
- Revisión de PR que verifica ausencia de entidades de dominio en controladores.

**Excepciones.** Una consulta de proyección simple MAY omitir la capa de dominio, pero MUST seguir
exponiendo un DTO y MUST NOT acceder a la colección sin índice declarado (Principio VII).

---

### Principio IV — Seguridad y privacidad por defecto

**Enunciado.** La seguridad MUST ser una propiedad estructural del sistema, no una capa añadida al final,
y el sistema MUST fallar cerrado ante cualquier duda.

**Reglas normativas**

- La autenticación MUST basarse en JWT sin estado. El token MUST validarse en firma, emisor, audiencia y
  expiración, y MUST tener una vigencia corta.
- MUST existir un endpoint de cierre de sesión que invalide efectivamente el token o la sesión del
  cliente (lista de revocación o rotación de *refresh token*). La invalidación MUST ser demostrable
  mediante prueba automatizada.
- Toda comunicación MUST usar TLS 1.2 o superior. Las solicitudes en texto plano MUST rechazarse o
  redirigirse. HSTS SHOULD estar habilitado.
- El control de tasa de peticiones MUST aplicarse por cliente y por endpoint sensible, con umbrales
  configurables por entorno.
- Los secretos MUST residir en AWS Secrets Manager o SSM Parameter Store. MUST NOT existir secretos en el
  repositorio, en el código, en imágenes, en artefactos de build ni en logs.
- Los logs MUST NOT contener datos personales, tokens, credenciales ni contenido moderado. MUST incluir
  un identificador de correlación por petición.
- La autorización MUST verificarse en el servidor en cada operación. El cliente MUST NOT ser fuente de
  verdad de permisos ni de identidad.
- La moderación de contenido MUST ejecutarse **antes** de publicar o persistir el contenido en S3.
- La moderación MUST enviar la imagen y el texto asociado en la misma solicitud (ver §2.4).
- La moderación MUST ser **fail-closed**: ante error, timeout o respuesta inválida del proveedor, el
  contenido MUST NOT publicarse y MUST quedar en cuarentena.
- Los umbrales de decisión SHOULD externalizarse en configuración y MUST registrarse junto con cada
  decisión para permitir auditoría.
- Toda decisión de moderación MUST ser auditable: veredicto, categorías, puntajes, umbral aplicado,
  acción ejecutada y marca temporal.
- Los datos personales SHOULD limitarse al mínimo necesario, con política de retención declarada en la
  especificación y mecanismo de borrado a solicitud del titular.
- Las dependencias MUST escanearse en busca de vulnerabilidades conocidas en cada pipeline.

**Justificación.** El sistema recibe contenido generado por usuarios, aplica sanciones automáticas y
opera en una plataforma pública. Una brecha o una moderación evadible afecta directamente a las personas
usuarias y a la viabilidad legal del producto.

**Criterios de verificación**

- Escaneo de secretos y de dependencias en CI, con fallo bloqueante.
- Pruebas de autorización por rol y por recurso.
- Prueba que demuestra que el token revocado es rechazado tras el logout.
- Prueba que demuestra que un error del proveedor de moderación impide la publicación.
- Prueba que garantiza ausencia de PII en los logs.

**Excepciones.** Ninguna admite el modo *fail-open* en moderación.

---

### Principio V — Verificación automatizada (Test-First y DevSecOps)

**Enunciado.** Ninguna funcionalidad se considera terminada sin pruebas automatizadas que la verifiquen,
ejecutadas de forma continua en el pipeline.

**Reglas normativas**

- Toda regla e invariante de dominio MUST tener pruebas unitarias.
- Toda integración con persistencia MUST probarse contra una instancia real del motor mediante
  Testcontainers; MUST NOT sustituirse por dobles que simulen el comportamiento de MongoDB.
- Todo endpoint MUST tener pruebas de integración que cubran caso exitoso, entradas inválidas, ausencia
  de autenticación y falta de permisos.
- Las integraciones externas (moderación, correo, push, almacenamiento) MUST probarse con dobles que
  permitan simular fallos, latencias y respuestas malformadas.
- La cobertura de pruebas MUST ser de al menos **80 %** de líneas en Domain y Application; Infrastructure
  y Presentation SHOULD alcanzar al menos 60 %.
- Cada corrección de defecto MUST acompañar una prueba de regresión que falle antes del arreglo.
- Las pruebas MUST ser deterministas, aisladas y ejecutables en cualquier entorno sin dependencias
  manuales.
- El pipeline MUST bloquear la fusión si cualquier verificación falla. MUST NOT existir fusión con
  verificaciones en rojo.
- El pipeline MUST ejecutar, como mínimo: compilación, pruebas unitarias, pruebas de integración,
  pruebas de seguridad, análisis de secretos, análisis de dependencias y validación de IaC.

**Justificación.** DevSecOps exige que la calidad y la seguridad se demuestren en cada cambio, no que se
declaren. La verificación automatizada es el único mecanismo que escala con el equipo.

**Criterios de verificación**

- Reporte de cobertura publicado por el pipeline con umbral bloqueante.
- Historial de ejecuciones del pipeline en cada PR.
- Revisión que confirma prueba de regresión en cada corrección.

**Excepciones.** Los *spikes* exploratorios MAY carecer de pruebas siempre que residan en una rama
`spike/*` y MUST NOT fusionarse a la rama principal.

---

### Principio VI — Operación serverless de costo cero gobernada por IaC

**Enunciado.** Todo el backend MUST ejecutarse sobre servicios serverless de AWS dentro del nivel
gratuito, con costo objetivo de **0 USD**, y todo recurso MUST aprovisionarse mediante infraestructura
como código.

**Reglas normativas**

- Todo recurso de AWS MUST aprovisionarse mediante IaC versionado. Los recursos creados manualmente en
  consola MUST NOT considerarse válidos y la desviación MUST corregirse en el código.
- El cómputo MUST implementarse con AWS Lambda detrás de AWS API Gateway. MUST NOT utilizarse EC2, ECS,
  EKS ni contenedores permanentes mientras exista una alternativa dentro del nivel gratuito.
- Solo MAY utilizarse servicios incluidos en el nivel gratuito. Cualquier servicio con costo MUST
  requerir enmienda a esta constitución (§4.3).
- MUST configurarse un presupuesto con alarma de facturación y MUST revisarse mensualmente.
- La infraestructura MUST superar `cdk synth` y `cdk diff` en CI, y el despliegue MUST realizarse
  exclusivamente a través del pipeline.
- Todo recurso MUST etiquetarse con proyecto, entorno y propietario.
- Los entornos MUST estar separados. Datos de producción MUST NOT utilizarse en desarrollo ni en pruebas.
- Toda decisión técnica MUST declarar su costo estimado mensual y su consumo respecto a los límites del
  nivel gratuito.
- Los límites del nivel gratuito relevantes (invocaciones, GB-segundo, almacenamiento, transferencia)
  MUST documentarse y vigilarse con alarmas.
- El procesamiento y la entrega de multimedia MUST realizarse con Amazon S3 y Amazon CloudFront.

**Justificación.** El costo cero es un requisito de producto, no una preferencia. En un modelo de pago
por uso, un error de configuración se traduce de inmediato en facturación real; IaC es el único control
que hace el límite verificable.

**Criterios de verificación**

- `cdk synth` y `cdk diff` obligatorios en CI.
- Existencia verificable de presupuesto y alarmas de facturación.
- Revisión de que ningún recurso se creó fuera del código (detección de deriva).
- Etiquetado validado automáticamente.

**Excepciones.** El aprovisionamiento inicial de la cuenta (usuario raíz, MFA, presupuesto base) MAY
realizarse manualmente y MUST documentarse en el repositorio.

---

### Principio VII — Rendimiento, paginación y observabilidad medibles

**Enunciado.** El rendimiento y la observabilidad MUST tratarse como criterios de aceptación, no como
optimizaciones posteriores.

**Reglas normativas**

- Todo endpoint que devuelva una colección MUST paginar. El tamaño por defecto MUST ser 20 y el máximo
  100; la respuesta MUST incluir metadatos de paginación.
- Toda consulta expuesta por un endpoint MUST apoyarse en un índice declarado y justificado. MUST NOT
  existir recorridos completos de colección en rutas críticas.
- Las lecturas recurrentes MUST usar caché con tiempo de vida definido e invalidación documentada.
- Las respuestas MUST comprimirse y MUST serializarse con un serializador de alto rendimiento.
- Los objetivos de latencia MUST fijarse como `NFR` en la especificación y MUST validarse con pruebas de
  carga antes del despliegue a producción.
- Los logs MUST ser estructurados en JSON e incluir identificador de correlación, nivel, operación y
  duración.
- MUST existir métricas de latencia, tasa de error, *throttling*, invocaciones y saturación, con alarmas
  definidas en IaC.
- La propagación del identificador de correlación entre API Gateway, Lambda y dependencias SHOULD estar
  habilitada.
- Amazon CloudWatch MUST ser la única plataforma de observabilidad, registro y rastreo. MUST NOT
  integrarse proveedores externos.

**Justificación.** La aplicación móvil opera en redes de ancho de banda limitado y el modelo serverless
penaliza tanto las consultas no indexadas como la ausencia de trazas. La paginación y la observabilidad
son requisitos funcionales del producto.

**Criterios de verificación**

- Pruebas que verifican paginación obligatoria y límites de tamaño.
- Revisión de índices declarados junto a cada consulta.
- Pruebas de carga contra los objetivos `NFR`.
- Alarmas versionadas y verificables en IaC.

**Excepciones.** Ninguna admite endpoints de colección sin paginación.

---

## 2. Restricciones técnicas y límites de plataforma

### 2.1 Stack autorizado

| Componente | Tecnología | Versión | Uso |
|---|---|---|---|
| Runtime | .NET (LTS) | 10 | API y capa de aplicación |
| Lenguaje | C# | 14 | Todo el backend |
| Base de datos | MongoDB | 8.x | Persistencia NoSQL orientada a documentos |
| Cómputo | AWS Lambda | — | Ejecución serverless |
| API | AWS API Gateway | — | Exposición RESTful |
| Almacenamiento | Amazon S3 | — | Contenido multimedia |
| CDN | Amazon CloudFront | — | Distribución de contenido |
| Observabilidad | Amazon CloudWatch | — | Logs, métricas, alarmas, trazas |
| Moderación | OpenAI `omni-moderation-latest` | — | Moderación multimodal |
| Notificaciones push | Abstracción `IPushDispatcher` | — | Despacho de notificaciones; adaptador FCM o SNS seleccionado por configuración |
| IaC | AWS CDK (C#) | — | Aprovisionamiento de recursos |
| Pruebas | xUnit, FluentAssertions, Testcontainers, NetArchTest | — | Unitarias, integración, arquitectura |

Las versiones indicadas MUST fijarse en el proyecto. MUST NOT aceptarse «última versión» como criterio
de cumplimiento.

### 2.2 Prohibiciones explícitas

- Bases de datos relacionales (SQL Server, PostgreSQL, MySQL u otras).
- Arquitecturas multi-nube. Los proveedores fuera de AWS MUST NOT usarse, con dos excepciones
  explícitas y acotadas: la moderación de contenido (§2.4) y el adaptador de notificaciones push
  declarado en §2.1. Cualquier otra dependencia externa MUST justificarse en una enmienda.
- Interfaces de usuario: aplicación móvil, página web o *landing*. El backend MUST limitarse a la API.
- Visión artificial local o modelos propios de moderación.
- Pasarelas de pago no contempladas en la especificación base.
- Servicios de AWS fuera del nivel gratuito.
- Recursos aprovisionados manualmente en consola.
- Exposición de entidades de dominio o documentos de persistencia en la API.
- Almacenamiento o publicación de contenido sin moderación previa.

### 2.3 Límites legales y de contenido

- El backend MUST NOT gestionar reportes de material de explotación o abuso infantil. Dichos flujos MUST
  canalizarse mediante procedimientos legales externos y MUST NOT persistirse en el sistema.
- El sistema MUST NOT presentarse como autoridad definitiva de moderación. Toda sanción MUST ser
  auditable y reversible mediante el procedimiento definido en la especificación.
- La retención de contenido moderado MUST declararse en la especificación y MUST respetar el principio de
  minimización.

### 2.4 Contrato mínimo de moderación

La moderación aplica a todo contenido generado por usuarios que vaya a exponerse públicamente:
imágenes de oferta, publicaciones del feed y comentarios. MUST NOT existir camino de publicación que
omita el veredicto.

| Aspecto | Requisito |
|---|---|
| Entrada | Imagen (Base64 o URL) **y** texto asociado (título o comentario) en la misma solicitud |
| Salida esperada | `flagged`, `categories` (booleanos) y `category_scores` (0.0–1.0) |
| Bloques evaluados | Contenido sexual (erotismo, desnudez), violencia (gráfica, general), autolesiones (suicidio, intención, instrucciones) |
| Banda baja | Publicación automática |
| Banda intermedia | Revisión manual |
| Banda alta | Rechazo automático y baneo del usuario |
| Umbrales | Externalizados en configuración, por entorno, registrados con cada decisión |
| Fallo del proveedor | Cuarentena del contenido; nunca publicación |

### 2.5 Alcance funcional del backend

- API RESTful autenticada para clientes móviles y web.
- Autenticación JWT y cierre de sesión con revocación.
- Ofertas geolocalizadas con apartado, código QR y canje en el local.
- Inventario de ofertas por comerciante con ciclo de vida explícito.
- Feed social geolocalizado con publicaciones, comentarios, reacciones y reportes.
- Búsqueda, recomendaciones y alternativas calculadas dentro del backend.
- Gestión de contenido de usuario con moderación previa.
- Almacenamiento y entrega de multimedia mediante S3 y CloudFront.
- Notificaciones push a dispositivos registrados.
- Consultas de colecciones paginadas e indexadas.
- Recepción asíncrona de eventos mediante webhooks (capacidad fuera del catálogo vigente, en backlog).

---

## 3. Flujo de desarrollo SDD y puertas de calidad

### 3.1 Fases del flujo

| Fase | Entrada | Salida | Criterio de salida |
|---|---|---|---|
| `specify` | Necesidad de negocio | `spec.md` con `FR-<spec>-<nn>`/`NFR-<spec>-<nn>` y criterios de aceptación | Alcance y exclusiones explícitos; requisitos con ID |
| `clarify` | `spec.md` | Ambigüedades resueltas | Sin preguntas abiertas ni criterios no medibles |
| `plan` | `spec.md` | `plan.md` con Constitution Check | Los siete principios evaluados en PASS o brecha registrada |
| `tasks` | `plan.md` | `tasks.md` atómico y ordenado | Toda tarea trazable a un requisito |
| `checklist` | `tasks.md` | `checklist.md` con Definition of Ready, criterios específicos y Definition of Done | Ninguna casilla pendiente antes de cerrar |
| `implement` | `tasks.md` + `checklist.md` | Código, pruebas e IaC | Definition of Done cumplida |

Las fases MUST ejecutarse en orden. MUST NOT iniciarse `implement` sin `plan.md` aprobado con
Constitution Check en PASS o con sus brechas registradas conforme a §4.4, `tasks.md` completo y
`checklist.md` creado.

### 3.2 Definition of Ready

Una historia o tarea MAY entrar en desarrollo solo si cumple todo lo siguiente:

- [ ] Requisito identificado (`FR-<spec>-<nn>` o `NFR-<spec>-<nn>`) y aprobado.
- [ ] La carpeta `specs/NNN-slug/` contiene `spec.md`, `plan.md`, `tasks.md` y `checklist.md`.
- [ ] Criterios de aceptación medibles y verificables.
- [ ] Alcance y exclusiones declarados.
- [ ] Impacto en seguridad, costo y rendimiento evaluado.
- [ ] Dependencias externas identificadas.
- [ ] Sin decisiones de arquitectura pendientes que la bloqueen.

### 3.3 Definition of Done

Una tarea MUST considerarse terminada solo si cumple todo lo siguiente:

- [ ] Código conforme a la arquitectura de capas (Principio II).
- [ ] Pruebas unitarias, de integración y de seguridad en verde (Principio V).
- [ ] Cobertura dentro de los umbrales definidos.
- [ ] Contrato OpenAPI actualizado y validado.
- [ ] IaC actualizada y `cdk diff` revisado (Principio VI).
- [ ] Logs, métricas y alarmas incorporados (Principio VII).
- [ ] Revisión de seguridad: sin secretos, sin PII en logs, autorización verificada.
- [ ] Documentación y ADR actualizados cuando aplique.
- [ ] `checklist.md` de la spec sin casillas pendientes y con la evidencia registrada.
- [ ] Pipeline completo en verde.

### 3.4 Constitution Check

Todo `plan.md` MUST incluir esta tabla, con decisión y evidencia:

| Principio | Decisión | Evidencia o justificación |
|---|---|---|
| I. Especificación y trazabilidad | PASS / FAIL | |
| II. Clean Architecture | PASS / FAIL | |
| III. CQRS y contratos | PASS / FAIL | |
| IV. Seguridad y privacidad | PASS / FAIL | |
| V. Verificación automatizada | PASS / FAIL | |
| VI. Costo cero e IaC | PASS / FAIL | |
| VII. Rendimiento y observabilidad | PASS / FAIL | |

El resultado global MUST declararse como `PASS`, `PASS CON OBSERVACIONES` o `FAIL`.

- Un `FAIL` MUST impedir la aprobación de un plan de implementación nueva.
- Un retro-spec que documente código ya existente MAY declararse `FAIL` siempre que cada brecha se
  registre como deuda técnica con tarea de remediación en `tasks.md` y con fecha de resolución.
- Un `PARCIAL` o `PASS CON OBSERVACIONES` MUST indicar la brecha concreta y su tarea asociada.

### 3.5 Convenciones de ingeniería

- **Commits**: Conventional Commits, con referencia al requisito (`feat(FR-012-03): ...`).
- **Versionado**: SemVer para la API y para esta constitución.
- **Ramas**: una rama por requisito; `main` MUST permanecer desplegable.
- **Pull requests**: pequeños, con propósito único, revisados por al menos una persona distinta al autor.
- **Prohibido**: fusionar con verificaciones en rojo, con conflictos sin resolver o sin revisión.
- **Idioma**: código y nombres técnicos en inglés; especificaciones y documentación en español.
- **Rutas**: el prefijo público MUST ser `/api/v1`. Los recursos MUST nombrarse en inglés, en plural y
  sin verbos (`/merchants`, `/offers`, `/feed/posts`). Las acciones que no encajen en CRUD MUST modelarse
  como subrecursos (`/offers/{id}/claim`, `/offers/{id}/status`). Los endpoints operativos (`/health/*`)
  quedan fuera del prefijo versionado.

### 3.6 Estructura obligatoria de una especificación

Cada capacidad MUST documentarse en su propia carpeta y con un alcance implementable en un máximo
aproximado de una semana de trabajo:

```
specs/
  NNN-slug/
    spec.md        Alcance, requisitos FR/NFR y criterios de aceptación
    plan.md        Diseño técnico y Constitution Check
    tasks.md       Tareas atómicas trazables a requisitos
    checklist.md   Definition of Ready, criterios específicos y Definition of Done
    contracts/
      openapi.yaml Contrato del spec cuando expone endpoints
  _templates/      Plantillas de los cuatro artefactos
  README.md        Índice de specs
  requirements.md  Registro consolidado de requisitos
```

Reglas adicionales:

- Una spec MUST cubrir una sola capacidad; MUST NOT agrupar funcionalidades independientes.
- El identificador de la spec MUST aparecer en los cuatro artefactos y en el prefijo de todos sus
  requisitos.
- Los contratos de `contracts/` MUST ser coherentes con el `spec.md` y con el OpenAPI publicado.
- El estado de avance MUST reflejarse en `tasks.md` y en `checklist.md`, no en documentos paralelos.

---

## 4. Gobernanza

### 4.1 Autoridad y resolución de conflictos

Esta constitución prevalece sobre cualquier otro documento del proyecto. Ante conflicto, MUST abrirse
una enmienda; MUST NOT resolverse por interpretación local. Las prácticas que no estén cubiertas aquí
MAY definirse en documentos subsidiarios, siempre que no contradigan ningún principio.

### 4.2 Versionado y derogación

La constitución se versiona con SemVer:

- **MAJOR**: se elimina un principio o se redefine su obligación.
- **MINOR**: se añade un principio o una restricción nueva.
- **PATCH**: aclaración de redacción sin cambio de obligación.

`arquitecturaNva.docx` es la fuente autoritativa de arquitectura. `Arquitectura Back.docx` queda
**derogado** en su totalidad, incluida su referencia a un *Load Balancer*, sustituida por el modelo
serverless descrito en la fuente vigente.

### 4.3 Procedimiento de enmienda

1. Registrar la propuesta con motivación y principios afectados.
2. Analizar el impacto sobre especificaciones, planes, tareas y código existente.
3. Aprobar la enmienda y actualizar la versión según §4.2.
4. Actualizar `constitution.md` y el registro de cambios (§4.6).
5. Comunicar el cambio y ajustar los artefactos afectados.

Toda enmienda MUST dejar el documento internamente consistente: MUST NOT quedar principios duplicados,
contradictorios o sin criterio de verificación.

### 4.4 Excepciones

Una excepción a un principio MUST:

- estar justificada por escrito,
- delimitar su alcance exacto,
- indicar responsables,
- tener fecha de expiración no mayor a un ciclo de release,
- registrarse en el apéndice E.

MUST NOT existir excepciones implícitas, permanentes o verbales.

### 4.5 Cumplimiento y auditoría

- Cada release MUST verificar el cumplimiento de los siete principios.
- Las violaciones detectadas MUST registrarse como deuda técnica con fecha de resolución.
- Una violación de los principios II, IV o VI MUST tratarse como bloqueante.

### 4.6 Registro de cambios

| Versión | Fecha | Cambio |
|---|---|---|
| 1.0.0 | 2026-10-28 | Ratificación inicial. Siete principios, restricciones de plataforma, flujo SDD y gobernanza. |
| 1.1.0 | 2026-10-29 | Identificadores de requisito por spec (`FR-<spec>-<nn>` / `NFR-<spec>-<nn>`); `checklist.md` como cuarto artefacto obligatorio; fase `checklist` y estructura de spec en §3.6; resultados de Constitution Check ampliados para retro-specs. |
| 1.2.0 | 2026-10-29 | Rutas públicas `/api/v1` con recursos en inglés (§3.5); abstracción de despacho push con adaptadores FCM o SNS (§2.1); excepciones acotadas a proveedores externos (§2.2); alcance funcional alineado al catálogo de 29 endpoints (§2.5). |

---

## 5. Apéndices

### Apéndice A — Glosario

| Término | Definición |
|---|---|
| **ADR** | Registro de decisión de arquitectura. |
| **CDN** | Red de distribución de contenido; en este proyecto, Amazon CloudFront. |
| **Clean Architecture** | Organización en capas con dependencias dirigidas hacia el dominio. |
| **CQRS** | Separación de comandos (escritura) y consultas (lectura). |
| **DTO** | Objeto de transferencia de datos expuesto por la API. |
| **fail-closed** | Ante un fallo, el sistema deniega la operación en lugar de permitirla. |
| **FR / NFR** | Requisito funcional / no funcional. |
| **Free Tier** | Nivel gratuito de AWS con límites de consumo mensual. |
| **IaC** | Infraestructura como código; en este proyecto, AWS CDK. |
| **p95** | Percentil 95 de latencia: el 95 % de las peticiones es más rápido que ese valor. |
| **SDD** | Spec-Driven Development: la especificación gobierna el desarrollo. |
| **SemVer** | Versionado semántico `MAJOR.MINOR.PATCH`. |

### Apéndice B — Trazabilidad con `arquitecturaNva.docx`

| Requisito de la fuente | Ubicación en esta constitución |
|---|---|
| SDD + DevSecOps | P-I, P-V, §3 |
| Clean Architecture en cuatro capas | P-II |
| CQRS, DTOs y mapeos | P-III |
| .NET y MongoDB | §2.1 |
| AWS Free Tier, costo 0 USD | P-VI |
| Lambda + API Gateway | P-VI, §2.1 |
| Aprovisionamiento por IaC | P-VI |
| S3 + CloudFront | P-VI, §2.1 |
| Indexación y paginación obligatorias | P-VII |
| Caché de consultas recurrentes | P-VII |
| CloudWatch como única observabilidad | P-VII |
| JWT sin estado y logout | P-IV |
| SSL/HTTPS | P-IV |
| Rate limiting | P-IV |
| Compresión y serialización eficiente | P-VII |
| CI/CD con pruebas unitarias, integración y seguridad | P-V, §3.1 |
| Moderación `omni-moderation-latest` | P-IV, §2.4 |
| Imagen y texto en la misma solicitud | §2.4 |
| `flagged`, `categories`, `category_scores`, seis subcategorías | §2.4 |
| Bandas de decisión: publicar, revisar, rechazar y banear | §2.4 |
| Notificaciones push y webhooks | §2.5 |
| Exclusión de interfaces de usuario | §2.2 |
| Exclusión de visión artificial local | §2.2 |
| Exclusión de reportes de explotación infantil | §2.3 |
| Exclusión de multi-nube, RDBMS y pasarelas de pago | §2.2 |

### Apéndice C — Plantilla de enmienda

```markdown
## Enmienda [MAJOR|MINOR|PATCH] — [título]

- Autor:
- Fecha:
- Principios afectados:
- Motivación:
- Impacto en especificaciones / planes / tareas / código:
- Nueva redacción propuesta:
- Criterios de verificación actualizados:
- Aprobación:
```

### Apéndice D — Plantilla de Constitution Check

```markdown
## Constitution Check — [plan]

| Principio | Decisión | Evidencia |
|---|---|---|
| I. Especificación y trazabilidad | | |
| II. Clean Architecture | | |
| III. CQRS y contratos | | |
| IV. Seguridad y privacidad | | |
| V. Verificación automatizada | | |
| VI. Costo cero e IaC | | |
| VII. Rendimiento y observabilidad | | |

Resultado global: PASS / PASS CON OBSERVACIONES / FAIL
Excepciones registradas: [Apéndice E]
```

### Apéndice E — Registro de excepciones

| ID | Principio | Alcance | Justificación | Responsable | Expira | Estado |
|---|---|---|---|---|---|---|
| — | — | — | — | — | — | — |

---

**Versión 1.1.0 — Ratificada el 2026-10-28, enmendada el 2026-10-29.** Toda modificación requiere el procedimiento de §4.3.
