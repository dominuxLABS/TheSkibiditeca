# TheSkibiditeca

Un sistema moderno de gestión bibliotecaria construido con ASP.NET Core para el curso de Programación Aplicada II.

> 📖 **[Versión en Español](README.es.md)** | **[English Version](README.md)**

## Miembros del Equipo:

- Díaz Rodríguez, Carlo Franco (@dom1nux)
- Mejía Irigoin, Luis Gerardo (@GalaxyM4)
- Ramos Callirgos, Harold Armando (@Jacob22wdf)
- Ruiz Loaysa, Deniss Jesus
- Vilca Ocas, María Belén


## Descripción General

TheSkibiditeca es una plataforma integral de gestión bibliotecaria que optimiza las operaciones de biblioteca incluyendo préstamos de libros, devoluciones y gestión de multas. Este proyecto demuestra prácticas modernas de desarrollo web usando tecnologías de Microsoft y sigue patrones y convenciones estándar de la industria.

## Características Principales

- 📚 **Gestión de Libros**: Administración de catálogo y control de inventario
- 👥 **Gestión de Usuarios**: Registro de miembros y gestión de perfiles
- 📋 **Sistema de Préstamos**: Funcionalidad de préstamo y reserva de libros
- 🔄 **Procesamiento de Devoluciones**: Flujo de trabajo optimizado para devolución de libros
- 💰 **Gestión de Multas**: Cálculo automático de multas por retraso y seguimiento de pagos
- 📊 **Reportes**: Estadísticas de biblioteca y análisis de uso
- 🔍 **Búsqueda y Filtros**: Capacidades avanzadas de búsqueda de libros y miembros

## Stack Tecnológico

- **Backend**: ASP.NET Core 9.0
- **Frontend**: HTML5, CSS3, JavaScript
- **Framework UI**: Bootstrap 5
- **Entorno de Desarrollo**: Visual Studio 2022
- **Contenedorización**: Docker
- **Control de Versiones**: Git y GitHub

## Características

- 🌐 Interfaz web responsiva
- 🏗️ Arquitectura MVC (Modelo-Vista-Controlador)
- 🐳 Soporte para contenedorización con Docker
- 📱 Diseño adaptable a móviles
- 🔧 Configuraciones de desarrollo y producción

## Comenzando

### Requisitos Previos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [Visual Studio Code](https://code.visualstudio.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (opcional)

### Instalación

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/dominuxLABS/TheSkibiditeca.git
   cd TheSkibiditeca
   ```

2. Restaurar dependencias:
   ```bash
   dotnet restore
   ```

3. Compilar el proyecto:
   ```bash
   dotnet build
   ```

4. Ejecutar la aplicación:
   ```bash
   dotnet run --project TheSkibiditeca.Web
   ```

La aplicación estará disponible en `https://localhost:7000` y `http://localhost:5000`.

## Scaffolding (Generación de CRUD)

**⚠️ IMPORTANTE**: Usar `dotnet scaffold` (NUEVO) en lugar de `dotnet aspnet-codegenerator` (DEPRECADO)

El proyecto soporta generación automática de CRUD para entidades usando la **nueva herramienta de scaffolding interactiva** de Microsoft. Esta herramienta es compatible con minimal hosting y funciona perfectamente con nuestra estructura de proyecto.

### Requisitos Previos para Scaffolding

Asegúrate de tener los paquetes requeridos instalados (ya incluidos en este proyecto):

```xml
<PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="8.0.7" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.8" />
```

### Usando dotnet scaffold (Herramienta Interactiva)

El **método recomendado** para generar operaciones CRUD:

```bash
# Navegar al directorio del proyecto Web
cd TheSkibiditeca.Web

# Lanzar scaffolding interactivo
dotnet scaffold
```

Esto abrirá un **menú interactivo** donde puedes:

1. **Seleccionar tipo de scaffold**: Elegir "Controller with views, using Entity Framework"
2. **Seleccionar clase modelo**: Elegir tu entidad (ej., `User`, `Book`, `Loan`)
3. **Seleccionar contexto de datos**: Elegir `DbContextSqlServer` (para desarrollo)
4. **Configurar opciones**: 
   - Nombre del controlador (auto-sugerido)
   - Ruta de carpeta de vistas
   - Uso de layout
   - Referenciar librerías de scripts

### Ejemplo: Scaffolding CRUD de Usuario

```bash
cd TheSkibiditeca.Web
dotnet scaffold

# Selecciones del menú interactivo:
# 1. Controller with views, using Entity Framework
# 2. Clase modelo: User
# 3. Contexto de datos: DbContextSqlServer
# 4. Nombre del controlador: UsersController
# 5. Generar vistas: Sí
# 6. Referenciar librerías de scripts: Sí
# 7. Usar layout por defecto: Sí
```

Esto genera:
- `Controllers/UsersController.cs` - Controlador CRUD completo
- `Views/Users/Index.cshtml` - Vista de lista
- `Views/Users/Create.cshtml` - Formulario de creación
- `Views/Users/Edit.cshtml` - Formulario de edición
- `Views/Users/Details.cshtml` - Vista de detalles
- `Views/Users/Delete.cshtml` - Confirmación de eliminación

### Métodos Alternativos

#### 1. Visual Studio IDE (Recomendado para usuarios de interfaz gráfica)

En Visual Studio:
1. Clic derecho en la carpeta `Controllers`
2. **Agregar** → **Nuevo elemento con scaffolding**
3. **Controlador MVC con vistas, usando Entity Framework**
4. Configurar tu entidad y contexto

#### 2. Línea de Comandos (Legacy - Compatibilidad limitada)

```bash
# ⚠️ Puede no funcionar con minimal hosting - usar dotnet scaffold en su lugar
dotnet aspnet-codegenerator controller -name [Entidad]Controller -m [Entidad] -dc DbContextSqlServer --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlserver
```

### Solución de Problemas de Scaffolding

**Problema**: Error "Minimal hosting scenario!"
**Solución**: Usar `dotnet scaffold` en lugar de `dotnet aspnet-codegenerator`

**Problema**: Referencias o paquetes faltantes
**Solución**: Asegurar que todos los paquetes EF estén instalados y el proyecto compile exitosamente

**Problema**: DbContext no encontrado
**Solución**: Verificar que el nombre de tu clase DbContext coincida exactamente (sensible a mayúsculas)

### Pasos Post-Scaffolding

Después del scaffolding, recuerda:

1. **Actualizar menú de navegación** (si es necesario):
   ```html
   <!-- Agregar a Views/Shared/_Layout.cshtml -->
   <li class="nav-item">
       <a class="nav-link" asp-controller="Users" asp-action="Index">Usuarios</a>
   </li>
   ```

2. **Ejecutar migraciones de base de datos** (si hay nuevas entidades):
   ```bash
   dotnet ef migrations add Add[Entidad]Entity --context DbContextSqlServer
   dotnet ef database update --context DbContextSqlServer
   ```

3. **Probar las operaciones CRUD generadas** en tu navegador

### Soporte para Docker

Para ejecutar la aplicación usando Docker:

```bash
docker build -t theskibiditeca .
docker run -p 8080:8080 theskibiditeca
```

La aplicación estará disponible en `http://localhost:8080`.

## Estructura del Proyecto

La solución sigue una arquitectura modular con una clara separación de responsabilidades usando la convención de nomenclatura: `<NombreProyecto>.<Componente>`

### Estructura Actual

```
TheSkibiditeca/
├── TheSkibiditeca.Web/          # Aplicación web principal (MVC)
│   ├── Controllers/             # Controladores MVC
│   ├── Models/                  # Modelos de datos y modelos de vista
│   ├── Views/                   # Vistas Razor y layouts
│   ├── wwwroot/                 # Archivos estáticos (CSS, JS, imágenes)
│   └── Program.cs               # Punto de entrada de la aplicación
├── TheSkibiditeca.sln           # Archivo de solución de Visual Studio
├── LICENSE.txt                  # Licencia del proyecto
└── README.md                    # Documentación del proyecto
```

### Arquitectura Planificada

A medida que el proyecto evolucione, se agregarán componentes adicionales siguiendo la convención de nomenclatura establecida:

- **TheSkibiditeca.Web** - Aplicación web principal con controladores y vistas MVC (actual)
- **TheSkibiditeca.Services** - Lógica de negocio (procesamiento de préstamos, cálculo de multas, gestión de inventario)
- **TheSkibiditeca.Data** - Capa de acceso a datos y patrones de repositorio
- **TheSkibiditeca.Models** - Entidades de dominio (Book, Member, Loan, Fine, etc.)
- **TheSkibiditeca.Tests** - Pruebas unitarias e de integración para todos los componentes
- **TheSkibiditeca.API** - API RESTful para aplicaciones móviles o integraciones de terceros

#### Componentes de Dominio
El sistema manejará entidades principales de biblioteca:
- **Libros**: Gestión de catálogo, seguimiento de disponibilidad
- **Miembros**: Perfiles de usuario, estado de membresía, historial de préstamos
- **Préstamos**: Préstamos activos, fechas de vencimiento, solicitudes de renovación
- **Multas**: Multas por retraso, seguimiento de pagos, cálculos de penalizaciones
- **Reservas**: Retención de libros y listas de espera

Este enfoque modular asegura:
- 🏗️ **Separación de Responsabilidades**: Cada proyecto tiene una responsabilidad específica
- 🔄 **Reutilización**: Los componentes pueden ser referenciados entre proyectos
- 🧪 **Testabilidad**: Más fácil realizar pruebas unitarias de componentes individuales
- 📦 **Mantenibilidad**: Límites claros entre diferentes capas

## Directrices de Desarrollo

### Convenciones de Nomenclatura

El proyecto sigue convenciones estrictas de nomenclatura para asegurar consistencia y mantenibilidad:

#### **Controladores**
- Formato: `[Nombre]Controller.cs`
- Ejemplos: `BooksController.cs`, `LoansController.cs`, `MembersController.cs`, `FinesController.cs`

#### **Vistas**
- Formato: `[NombreAccion].cshtml`
- Ejemplos: `Index.cshtml`, `Create.cshtml`, `Edit.cshtml`, `Details.cshtml`, `Return.cshtml`
- Las vistas se organizan en carpetas que coinciden con los nombres de sus controladores

#### **Modelos**
- **Entidades de Dominio**: `[Nombre].cs` (ej., `Book.cs`, `Member.cs`, `Loan.cs`, `Fine.cs`)
- **ViewModels**: `[Nombre]ViewModel.cs` (ej., `BookDetailsViewModel.cs`, `LoanProcessViewModel.cs`)
- **DTOs**: `[Nombre]Dto.cs` (ej., `BookDto.cs`, `MemberDto.cs`)
- **Modelos API**: `[Nombre]Request.cs` / `[Nombre]Response.cs` (ej., `CreateLoanRequest.cs`, `FineCalculationResponse.cs`)

#### **Evitando Colisiones de Nombres**
Para prevenir conflictos de nomenclatura, el proyecto usa:
- **Namespaces**: Diferentes tipos en namespaces separados (`TheSkibiditeca.Models`, `TheSkibiditeca.ViewModels`)
- **Sufijos contextuales**: `ViewModel`, `Dto`, `Request`, `Response` cuando sea necesario
- **Organización de carpetas**: Separación lógica por propósito y capa

#### **Directrices Generales**
- **Idioma**: Todo el código, comentarios y mensajes de commit deben estar en inglés
- **Convenciones C#**: Seguir los estándares oficiales de codificación C# de Microsoft
- **Pascal Case**: Clases, métodos, propiedades (`BookController`, `GetBookById`)
- **Camel Case**: Variables locales, parámetros (`bookId`, `userName`)
- **Commits**: Usar formato de commit convencional (`feat:`, `fix:`, `docs:`, etc.)

## Contribución

Este es un proyecto educativo para el curso de Programación Aplicada II. Las contribuciones son gestionadas por el equipo de desarrollo.

## Licencia

Este proyecto está licenciado bajo la Licencia Pública General Affero de GNU v3.0 - ver el archivo [LICENSE.txt](LICENSE.txt) para detalles.

## Contexto Académico

**Curso**: Programación Aplicada II (11Q238)  
**Institución**: Universidad Nacional de Cajamarca  
**Año Académico**: 2025  

---

*Construido con ❤️ por el equipo dominuxLABS*
