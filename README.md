# AdmiProducts

Aplicación de consola desarrollada en C# para la administración de productos.

---

## Tecnologías

- C#
- .NET 10
- SQL Server
- Microsoft.Data.SqlClient
- Dependency Injection
- Repository Pattern

---

## Requisitos

Antes de ejecutar el proyecto asegúrate de tener instalado:

- Visual Studio 2026
- .NET 10 SDK
- SQL Server

---
## Base de datos

1. Abrir SQL Server Management Studio.
2. Ejecutar el script:

database.sql

Este script creará:

- Base de datos ProyectoIngSoftware
- Tablas necesarias
- Datos iniciales

---
## Configuración

Modificar el archivo appsettings.json:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ProyectoIngSoftware;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```
Ajustar el servidor según la instalación local.

---

### Cómo ejecutar el proyecto
1. Clonar el repositorio

```bash
git clone https://github.com/Angel-Glez1/ConsoleAppManagementProducts.git
```

2. Abrir la solucion
```
ProyectoIngSoftware.sln
```

3. Restaurar paquetes NuGet.
> Es proceso lo hace en automatico Visual Studio 2026.
En caso de que no sea así abre un shell en la raiz del proyecto (dónde se encuentra el `ProyectIngSoftware`) y ejecuta el siguente comando
```bash
dotnet restore

# luego
dotnet build

# Y después
dotnet run
```

4. Ejecutar el proyecto de consola:
```bash
Ctrl + F5
```