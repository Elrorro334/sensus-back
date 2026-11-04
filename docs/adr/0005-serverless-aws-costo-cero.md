# ADR 0005 — Operación serverless en AWS con costo objetivo 0 USD e IaC

| Campo | Valor |
|---|---|
| Estado | Aceptada |
| Fecha | 2026-10-28 |
| Principios afectados | VI |
| Specs relacionados | 015, 022, 023 |
| Fuente | `arquitecturaNva.docx` |

## Contexto

El costo cero es un requisito de producto, no una preferencia: el proyecto no puede sostener facturación
variable. En un modelo de pago por uso, un error de configuración se traduce de inmediato en facturación
real, y los recursos creados a mano en consola son invisibles para el control de versiones y para la
detección de deriva.

## Decisión

- Cómputo exclusivamente **AWS Lambda** detrás de **AWS API Gateway**.
- Multimedia en **Amazon S3** y entrega mediante **Amazon CloudFront** con bucket privado y URLs firmadas.
- Observabilidad exclusivamente en **Amazon CloudWatch** (logs, métricas, alarmas).
- Todo recurso aprovisionado con **AWS CDK en C#** dentro de `infra/`; prohibido crear recursos por
  consola o CLI.
- `cdk synth` obligatorio en CI y `cdk diff` obligatorio en PR; `cdk deploy` solo desde el pipeline y con
  autorización explícita.
- Presupuesto con alarma de facturación, revisado mensualmente.
- Etiquetado obligatorio de todo recurso: `project`, `environment`, `owner`.
- Entornos separados; datos de producción nunca se usan en desarrollo ni en pruebas.
- Prohibidos EC2, ECS, EKS, contenedores permanentes y cualquier servicio fuera del nivel gratuito.
- `docker-compose.yml` es solo para desarrollo local; no define la arquitectura de despliegue.

## Consecuencias

**Positivas**

- Costo dentro del Free Tier verificable y auditable.
- Infraestructura reproducible y revisable en PR.
- Detección de deriva entre lo desplegado y el código.

**Negativas**

- Límites del Free Tier (invocaciones, GB-segundo, almacenamiento, transferencia) condicionan el diseño.
- *Cold starts* y límites de tiempo de Lambda.
- El equipo debe aprender CDK y el modelo de permisos de IAM.

**Mitigación**

- Alarmas de facturación y de consumo desde el primer despliegue.
- Caché para lecturas recurrentes (spec 026) y consultas indexadas (P-VII).
- Pruebas de carga contra los objetivos `NFR` antes de producción.

## Alternativas consideradas

| Alternativa | Motivo del descarte |
|---|---|
| EC2 o contenedores permanentes | Costo fijo mensual; prohibido por P-VI |
| ECS Fargate | Fuera del Free Tier en operación continua |
| Recursos creados en consola | No versionados, no auditables, riesgo de deriva |
| Otro proveedor de nube | Prohibido por §2.2 |

## Referencias

- `constitution.md` P-VI y §2.1
- `docs/architecture/overview.md`
- `specs/015-metricas-y-alarmas/`, `specs/022-carga-multimedia/`, `specs/023-entrega-cdn/`
