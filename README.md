# Sensus Backend - Arquitectura Limpia en .NET 10

Backend empresarial para **Sensus** desarrollado con **.NET 10**, siguiendo los principios de **Clean Architecture**, **CQRS** (MediatR), **FluentValidation**, **BCrypt** y persistencia en **MongoDB Atlas** (migrado desde el esquema relacional SQL).

---

## 📋 Historias de Usuario Implementadas

### 🔹 SE-001: Registro con Correo y Contraseña (Modelo Unificado)
- **Endpoint**: `POST /api/v1/auth/register`
- **Criterios de Aceptación Cumplidos**:
  - Registro seguro mediante correo electrónico, nombre completo y contraseña.
  - Validación de formato estándar de correo electrónico y disponibilidad (único en MongoDB Atlas mediante índice único).
  - Exigencia estricta de contraseña segura: mínimo 8 caracteres, al menos una mayúscula, una minúscula y un número (`FluentValidation`).
  - Cifrado unidireccional de contraseñas mediante **BCrypt con salt**.
  - **Modelo Unificado (Airbnb)**: La cuenta se inicializa con el rol `Consumidor`, lista para alternar o añadir el rol `Comerciante` (SE-003) sin crear cuentas duplicadas.
  - Retorna sesión activa con token **JWT**.

### 🔹 SE-002: Inicio de Sesión Seguro (BCrypt + JWT)
- **Endpoint**: `POST /api/v1/auth/login`
- **Criterios de Aceptación Cumplidos**:
  - Validación de credenciales contra contraseñas protegidas con **BCrypt**.
  - Generación y retorno de token de sesión **JWT Bearer** con claims (`sub`, `email`, `name`, `roles`, `jti`, expiración).
  - **Mensaje genérico de seguridad**: Si el correo no existe o la contraseña es errónea, responde con `401 Unauthorized` y el mensaje `"Correo o contraseña incorrectos."` sin filtrar información sensible sobre la existencia de la cuenta.
  - Actualización atómica de la fecha y hora de `UltimoAcceso`.
- **Endpoint Protegido**: `GET /api/v1/auth/me` para verificar identidad y roles mediante el token JWT.

---

## 🍃 Migración de Base de Datos: de SQL Server a MongoDB Atlas

El proyecto originalmente contaba con el script relacional [Base_Datos_Movi;.sql](file:///C:/Users/Rodrigo/Downloads/Base_Datos_Movi;.sql) con 3 tablas:
- `Usuarios` (Datos del usuario y hash)
- `Roles` (Catálogo de roles)
- `UsuarioRoles` (Tabla intermedia muchos a muchos)

### Modelo de Documento en MongoDB Atlas
En MongoDB, eliminamos la necesidad de joins relacionales y tablas intermedias. El usuario se almacena como un documento autocontenido en la colección `usuarios`:

```json
{
  "_id": "d3a4f9c0-561a-482a-a1bf-6586f740d1df",
  "nombreCompleto": "Oscar José Salinas",
  "correo": "oscarsa48700@gmail.com",
  "passwordHash": "$2a$11$e8V... (hash BCrypt)",
  "aceptoTerminos": true,
  "fechaAceptacionTerminos": "2026-10-22T12:00:00Z",
  "estatus": true,
  "fechaRegistro": "2026-10-22T12:00:00Z",
  "ultimoAcceso": "2026-10-22T13:30:00Z",
  "roles": [
    "Consumidor"
  ]
}
```

### Servicio de Migración Automática (`SqlToMongoMigrationService`)
Al iniciar la aplicación en modo desarrollo, el servicio detecta si la base de datos está vacía y migra automáticamente el registro inicial del script SQL (`oscarsa48700@gmail.com`), asignándole un hash seguro de BCrypt (`Password123!`) y el rol `Consumidor`.

---

## ⚙️ ¿Qué se necesita para conectar tu MongoDB Atlas?

Para conectar tu clúster de MongoDB Atlas, solo requieres configurar tu cadena de conexión en [appsettings.json](file:///c:/Users/Rodrigo/source/repos/sensus-back/src/Sensus.Api/appsettings.json):

```json
"MongoDbSettings": {
  "ConnectionString": "mongodb+srv://<usuario>:<password>@<cluster>.mongodb.net/?retryWrites=true&w=majority",
  "DatabaseName": "SensusDb",
  "UsersCollectionName": "usuarios"
}
```

> **Nota de Resiliencia**: Si dejas `ConnectionString` vacío o sin conexión a internet, el backend opera automáticamente con un almacén local en memoria, permitiéndote compilar, ejecutar y probar los endpoints inmediatamente sin depender de Atlas.

---

## 🧪 Pruebas Automatizadas

El proyecto cuenta con **34 pruebas automatizadas (100% aprobadas)**:
- **4 Pruebas de Arquitectura (NetArchTest)**: Garantizan que las capas de Dominio y Aplicación no tengan dependencias externas.
- **30 Pruebas Unitarias (xUnit + NSubstitute + FluentAssertions)**: Prueban los Handlers y Validadores de `RegisterUser` (SE-001) y `Login` (SE-002).

Para ejecutar las pruebas:
```bash
dotnet test sensus-back.sln
```

---

## 🚀 Probar los Endpoints

Abre el archivo [Sensus.Api.http](file:///c:/Users/Rodrigo/source/repos/sensus-back/src/Sensus.Api/Sensus.Api.http) en Visual Studio o VS Code (con la extensión REST Client) para ejecutar con un solo clic:
1. `POST /api/v1/auth/register` (SE-001: Registro exitoso, duplicado, contraseña débil, sin términos).
2. `POST /api/v1/auth/login` (SE-002: Login exitoso, credenciales inválidas, usuario migrado de SQL).
3. `GET /api/v1/auth/me` (Consulta de perfil con token Bearer).
