# RealEstate API (.NET 8)

API para gestión de propiedades inmobiliarias, construida con **.NET 8**, arquitectura en capas (**Api / Application / Infrastructure / Domain**), **EF Core 8 / SQL Server**, autenticación **JWT**, carga de imágenes vía **multipart/form-data**, y pruebas con **NUnit**.

## Contenido

- [Arquitectura](#arquitectura)
- [Requisitos](#requisitos)
- [Configuración](#configuración)
  - [Cadena de conexión (SQL Server)](#cadena-de-conexión-sql-server)
  - [JWT (seguridad)](#jwt-seguridad)
  - [Carpeta de imágenes](#carpeta-de-imágenes)
- [Base de datos](#base-de-datos)
  - [Carga DB: restaurar el backup (.bak)](#opción-a-restaurar-el-backup-bak)
- [Ejecución](#ejecución)
- [Swagger y autenticación](#swagger-y-autenticación)
  - [Obtener token](#obtener-token)
  - [Autorizar en Swagger](#autorizar-en-swagger)
  - [Errores comunes de autenticación](#errores-comunes-de-autenticación)
- [Endpoints principales (según prueba técnica)](#endpoints-principales-según-prueba-técnica)
- [Validaciones y seguridad](#validaciones-y-seguridad)
- [Pruebas (NUnit)](#pruebas-nunit)
  - [Ejecución de pruebas](#ejecución-de-pruebas)
  - [Cómo están hechas (importante)](#cómo-están-hechas-importante)
- [Notas sobre imágenes](#notas-sobre-imágenes)
- [Solución de problemas](#solución-de-problemas)


---

## Arquitectura

- **RealEstate.Api**: controladores, Swagger, autenticación JWT, exposición de carpeta `/images`.
- **RealEstate.Application**: DTOs, perfiles de AutoMapper, servicios y reglas de negocio.
- **RealEstate.Infrastructure**: `DbContext`, repositorios EF Core, inyección de dependencias.
- **RealEstate.Domain**: entidades y contratos (interfaces de repos).
- **RealEstate.Tests**: pruebas (NUnit) unitarias y de integración.

---

## Requisitos

- **.NET SDK 8.0**
- **SQL Server** (local o remoto)
- Visual Studio 2022 o VS Code (opcional)
- EF Core Tools (para migraciones):  
  `dotnet tool install --global dotnet-ef` (si no lo tienes)

---

## Configuración

### Cadena de conexión (SQL Server)

Edita **`RealEstate.Api/appsettings.json`**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=RealEstateDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "TokenSecretoDeMuchosCaracteres12345!!",
    "Issuer": "RealEstateApi",
    "Audience": "RealEstateApiUsers"
  },
  "AllowedHosts": "*"
}
```

- Cambiar `TU_SERVIDOR` por la instancia (ej. `DESKTOP-XXXX\\SQLEXPRESS` o `localhost`).
- La BD por defecto se llama **`RealEstateDB`**.

> **Importante:** Si se usa autenticación SQL en lugar de Windows, utilizar un connection string con `User Id=...;Password=...;`.

### JWT (seguridad)

En el mismo `appsettings.json`:

- `Jwt:Key` **debe tener al menos 32 caracteres** (requisito de HS256).
- `Issuer` y `Audience` deben coincidir con los usados en el `AuthController`.

> En esta prueba, el **login es “fake”** y está pensado solo para demostrar seguridad:
> - **Usuario:** `admin`
> - **Clave:** `1234`

### Carpeta de imágenes

La API **expone** la carpeta **`images`** que se encuentra en la **raíz del proyecto `RealEstate.Api`**.  
En `Program.cs` se mapea como **contenido estático** en `/images`.

- Si no existe, se crea al iniciar.
- Las URL públicas quedan así:  
  `https://localhost:{puerto}/images/house1.jpg`

---

## Base de datos

El repo incluye un **archivo .bak** (backup) para que se puedan cargar datos reales rápidamente.

### CARGA DB: restaurar el backup (.bak)

1. En SQL Server, **restaura** el backup incluido (por ejemplo a `RealEstateDB`).
2. Ajustar la cadena de conexión en `appsettings.json` si cambian el nombre de la BD o el servidor.
3. **Listo**: habrán datos de prueba en tablas `Owner`, `Property`, `PropertyImage`, `PropertyTrace`.

---

## Ejecución

### Visual Studio
- Establezca `RealEstate.Api` como proyecto de inicio.
- Presionar **F5** (o `Ctrl+F5`).
- Abre Swagger en: `https://localhost:{puerto}/swagger`

### CLI
```bash
cd RealEstate.Api
dotnet run
```

---

## Swagger y autenticación

El controlador `PropertiesController` está marcado con `[Authorize]`, por lo que **requiere token**.

### Obtener token

1. En Swagger, abre **`POST /api/auth/login`**.
2. Envía el body:
   ```json
   {
     "username": "admin",
     "password": "1234"
   }
   ```
3. Recibirás:
   ```json
   {
     "token": "eyJhbGciOiJIUzI1NiIs...",
     "expiration": "2025-09-02T21:30:00Z"
   }
   ```

### Autorizar en Swagger

- Hacer clic en el botón **Authorize** (candado).
- **Pegar solo el token crudo** (Swagger agrega `Bearer` automáticamente).  

### Errores comunes de autenticación

- **401 Unauthorized** y log: `JWT is not well formed... no dots`:  
  — Pegaste algo que **no es el token**, o copiaste con comillas.  
  — Solución: pega **solo** el string del token (`xxxxx.yyyyy.zzzzz`).

- **IDX10720: key size must be greater than 256 bits**:  
  — El `Jwt:Key` es corto.  
  — Solución: usar **32+ caracteres**.

---

## Endpoints principales (según prueba técnica)

> **Base**: `/api/properties` (RESTful, cada operación se diferencia por el verbo HTTP)

### 1) List Property with Filters — `GET /api/properties`

### 2) Create Property Building — `POST /api/properties`

### 3) Add Image from Property — `POST /api/properties/{id}/images`

### 4) Change Price — `PUT /api/properties/{id}/price`

### 5) Update Property — `PUT /api/properties/{id}`

Cada endpoint valida y retorna los códigos HTTP adecuados (`200/201/400/401/403/404/409`).

---

## Validaciones y seguridad

- **DTOs** con **DataAnnotations** (requeridos, rangos, longitudes). Swagger los refleja.
- **`[Authorize]`** en `PropertiesController`: requiere JWT en cada request.
- **JWT** firmado con **HS256**.  
- **Subida de imágenes**:
  - Validación de extensión.
  - Carpeta `images` expuesta como contenido estático.
  - **Regla anti-duplicado** por `(PropertyId, File)` vía índice único en BD.
- **HTTPS** habilitado (`UseHttpsRedirection`).

---

## Pruebas (NUnit)

### Ejecución de pruebas

**Visual Studio**:
- `Test > Test Explorer > Run All`.

**CLI**:
```bash
dotnet test RealEstate.Tests/RealEstate.Tests.csproj
```

(Extra) Generar reporte:
```bash
dotnet test RealEstate.Tests/RealEstate.Tests.csproj --logger "trx;LogFileName=TestResults.trx"
```

### Cómo están hechas (importante)

- **Unit tests de servicios**:  
  Usan la **misma BD real** pero encapsulados en un `TransactionScope`, por lo que **hacen rollback** al finalizar cada test.  
  Esto permite validar reglas con datos reales **sin ensuciar** la base.

- **Tests de integración (API)**:  
  Usan `WebApplicationFactory<Program>` para levantar el pipeline real (Swagger/JWT/Controllers).  
  Se **obtiene un token real** con `/api/auth/login` y se prueban endpoints protegidos.  
  > Al usar la BD real, si algún test hace inserciones vía API, **pueden quedar en la BD**. En este caso se usa el código interno con prefijo de pruebas (ej. `IT-...`) para distinguirlo o se hace cleanup (Delete) manual si se desea.

---

## Notas sobre imágenes

- Las imágenes “mock” (ej. `house1.jpg` … `house50.jpg`) deben estar en **`RealEstate.Api/images`** para ser servidas/mostradas.  
- Al subir una imagen por el endpoint, el archivo se **copia en esa carpeta** y la API guarda solo el **nombre** en BD.  
- Las URL se construyen como:  
  `https://{host}/images/{file}`

---

## Solución de problemas

- **401 Unauthorized** al llamar endpoints protegidos:
  - Asegúrate de **loguearte** en `/api/auth/login`.
  - En Swagger, pega **solo** el token sin `Bearer` (Swagger lo agrega).

- **IDX10720 (key size)**:
  - Aumenta `Jwt:Key` a **32+ chars**.

- **Swagger 500 al cargar `IFormFile`**:
  - Usa `[FromForm]` y un DTO con una propiedad `IFormFile File` (p. ej. `PropertyImageUploadDto`).

- **testhost.deps.json** faltante (tests de integración):
  - Agregar en `RealEstate.Api.csproj`:  
    `<PreserveCompilationContext>true</PreserveCompilationContext>`
  - En `RealEstate.Tests.csproj`:  
    `<CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>`
  - Verificar que `RealEstate.Tests` **referencie** a `RealEstate.Api`.

---


