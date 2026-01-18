# 📑 Sistema de Gestión de Facturación (NCS.Prueba)
Una aplicación web diseñada para la gestión integral de clientes y procesos de facturación. Este proyecto implementa prácticas modernas de desarrollo con .NET 8, enfocándose en la integridad de los datos, la separación de responsabilidades y una arquitectura limpia y escalable.

## 🚀 Funcionalidades
- **Gestión de Clientes:** CRUD completo para la administración de clientes y facturas.
- **Interfaz de Usuario:** Interfaz responsive para su utilización.

## 🛠️ Tecnologías y Herramientas
- **Backend:** .NET 8 (ASP.NET Core MVC).
- **Persistencia:** Entity Framework Core con SQL Server.
- **Frontend:** Bootstrap, Razor Pages y JavaScript.
- **DevOps:** Integración Continua (CI/CD) en Azure.

## 🏗️ Arquitectura e Implementaciones
El proyecto sigue patrones de diseño para asegurar que el código sea mantenible y escalable:

- **Repository Pattern & Unit of Work:** Desacoplamiento de la lógica de datos y gestión de transacciones centralizada 📂.
- **Entity Framework Core (Code First):** Uso de migraciones para un control de versiones del esquema de la base de datos.
- **Lógica en Base de Datos:** Uso de **Procedimientos Almacenados** para el cálculo de totales.
- **Validación Avanzada:** Integridad de la información en el cliente (**jquery-validation**) y el servidor (**DataAnnotations** y **CustomValidation**).
- **Dependency Injection:** Uso del contenedor nativo de .NET para gestionar el ciclo de vida de los servicios 💉.
- **Service Layer:** La lógica de negocio está centralizada en servicios específicos (**ClienteService**, **FacturaService**).

## 🌐 Demo en Vivo
Puedes probar la aplicación desplegada en Azure aquí: https://ncs-prueba.azurewebsites.net/

## 💻 Configuración Local
Si deseas ejecutar el proyecto localmente, sigue estos pasos:

1. Clonar el repositorio:

```git
git clone https://github.com/EmilioAngu2003/NCS.Prueba
```

2. Configurar la Base de Datos: Crea un archivo llamado `appsettings.json` en la raíz del proyecto y añade tu `ConnectionString` de SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=NCS_Prueba;Trusted_Connection=True;"
  }
}
```

3. Iniciar la aplicación: *(Nota: Las migraciones de la base de datos se aplicarán automáticamente al iniciar el servidor).* 

```
dotnet run
```