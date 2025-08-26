# TheSkibiditeca

A modern library management system built with ASP.NET Core for the Applied Programming II course.

> 📖 **[Versión en Español](README.es.md)** | **[English Version](README.md)**

## Overview

TheSkibiditeca is a comprehensive library management platform that streamlines library operations including book loans, returns, and fine management. This project demonstrates modern web development practices using Microsoft technologies and follows industry-standard patterns and conventions.

## Core Features

- 📚 **Book Management**: Catalog administration and inventory control
- 👥 **User Management**: Member registration and profile management
- 📋 **Loan System**: Book borrowing and reservation functionality
- 🔄 **Return Processing**: Streamlined book return workflow
- 💰 **Fine Management**: Automated late fee calculation and payment tracking
- 📊 **Reporting**: Library statistics and usage analytics
- 🔍 **Search & Filter**: Advanced book and member search capabilities

## Technology Stack

- **Backend**: ASP.NET Core 8.0
- **Frontend**: HTML5, CSS3, JavaScript
- **UI Framework**: Bootstrap 5
- **Development Environment**: Visual Studio 2022
- **Containerization**: Docker
- **Version Control**: Git & GitHub

## Features

- 🌐 Responsive web interface
- 🏗️ MVC (Model-View-Controller) architecture
- 🐳 Docker containerization support
- 📱 Mobile-friendly design
- 🔧 Development and production configurations

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/dominuxLABS/TheSkibiditeca.git
   cd TheSkibiditeca
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run --project TheSkibiditeca.Web
   ```

The application will be available at `https://localhost:7000` and `http://localhost:5000`.

## Scaffolding (CRUD Generation)

**⚠️ IMPORTANT**: Use `dotnet scaffold` (NEW) instead of `dotnet aspnet-codegenerator` (DEPRECATED)

The project supports automatic CRUD generation for entities using Microsoft's **new interactive scaffolding tool**. This tool is compatible with minimal hosting and works seamlessly with our project structure.

### Prerequisites for Scaffolding

Ensure you have the required packages installed (already included in this project):

```xml
<PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="8.0.7" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.8" />
```

### Using dotnet scaffold (Interactive Tool)

The **recommended method** for generating CRUD operations:

```bash
# Navigate to the Web project directory
cd TheSkibiditeca.Web

# Launch interactive scaffolding
dotnet scaffold
```

This will open an **interactive menu** where you can:

1. **Select scaffold type**: Choose "Controller with views, using Entity Framework"
2. **Select model class**: Pick your entity (e.g., `User`, `Book`, `Loan`)
3. **Select data context**: Choose `DbContextSqlServer` (for development)
4. **Configure options**: 
   - Controller name (auto-suggested)
   - Views folder path
   - Layout usage
   - Reference script libraries

### Example: Scaffolding User CRUD

```bash
cd TheSkibiditeca.Web
dotnet scaffold

# Interactive menu selections:
# 1. Controller with views, using Entity Framework
# 2. Model class: User
# 3. Data context: DbContextSqlServer
# 4. Controller name: UsersController
# 5. Generate views: Yes
# 6. Reference script libraries: Yes
# 7. Use default layout: Yes
```

This generates:
- `Controllers/UsersController.cs` - Full CRUD controller
- `Views/Users/Index.cshtml` - List view
- `Views/Users/Create.cshtml` - Create form
- `Views/Users/Edit.cshtml` - Edit form
- `Views/Users/Details.cshtml` - Details view
- `Views/Users/Delete.cshtml` - Delete confirmation

### Alternative Methods

#### 1. Visual Studio IDE (Recommended for GUI users)

In Visual Studio:
1. Right-click on `Controllers` folder
2. **Add** → **New Scaffolded Item**
3. **MVC Controller with views, using Entity Framework**
4. Configure your entity and context

#### 2. Command Line (Legacy - Limited compatibility)

```bash
# ⚠️ May not work with minimal hosting - use dotnet scaffold instead
dotnet aspnet-codegenerator controller -name [Entity]Controller -m [Entity] -dc DbContextSqlServer --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlserver
```

### Troubleshooting Scaffolding

**Problem**: "Minimal hosting scenario!" error
**Solution**: Use `dotnet scaffold` instead of `dotnet aspnet-codegenerator`

**Problem**: Missing references or packages
**Solution**: Ensure all EF packages are installed and project builds successfully

**Problem**: DbContext not found
**Solution**: Verify your DbContext class name matches exactly (case-sensitive)

### Post-Scaffolding Steps

After scaffolding, remember to:

1. **Update navigation menu** (if needed):
   ```html
   <!-- Add to Views/Shared/_Layout.cshtml -->
   <li class="nav-item">
       <a class="nav-link" asp-controller="Users" asp-action="Index">Users</a>
   </li>
   ```

2. **Run database migrations** (if new entities):
   ```bash
   dotnet ef migrations add Add[Entity]Entity --context DbContextSqlServer
   dotnet ef database update --context DbContextSqlServer
   ```

3. **Test the generated CRUD operations** in your browser

### Docker Support

To run the application using Docker:

```bash
docker build -t theskibiditeca .
docker run -p 8080:8080 theskibiditeca
```

The application will be available at `http://localhost:8080`.

## Project Structure

The solution follows a modular architecture with a clear separation of concerns using the naming convention: `<ProjectName>.<Component>`

### Current Structure

```
TheSkibiditeca/
├── TheSkibiditeca.Web/          # Main web application (MVC)
│   ├── Controllers/             # MVC Controllers
│   ├── Models/                  # Data models and view models
│   ├── Views/                   # Razor views and layouts
│   ├── wwwroot/                 # Static files (CSS, JS, images)
│   └── Program.cs               # Application entry point
├── TheSkibiditeca.sln           # Visual Studio solution file
├── LICENSE.txt                  # Project license
└── README.md                    # Project documentation
```

### Planned Architecture

As the project evolves, additional components will be added following the established naming convention:

- **TheSkibiditeca.Web** - Main web application with MVC controllers and views (current)
- **TheSkibiditeca.Services** - Business logic (loan processing, fine calculation, inventory management)
- **TheSkibiditeca.Data** - Data access layer and repository patterns
- **TheSkibiditeca.Models** - Domain entities (Book, Member, Loan, Fine, etc.)
- **TheSkibiditeca.Tests** - Unit and integration tests for all components
- **TheSkibiditeca.API** - RESTful API for mobile apps or third-party integrations

#### Domain Components
The system will handle core library entities:
- **Books**: Catalog management, availability tracking
- **Members**: User profiles, membership status, borrowing history
- **Loans**: Active borrowings, due dates, renewal requests
- **Fines**: Late fees, payment tracking, penalty calculations
- **Reservations**: Book holds and waitlists

This modular approach ensures:
- 🏗️ **Separation of Concerns**: Each project has a specific responsibility
- 🔄 **Reusability**: Components can be referenced across projects
- 🧪 **Testability**: Easier to unit test individual components
- 📦 **Maintainability**: Clear boundaries between different layers

## Development Guidelines

### Naming Conventions

The project follows strict naming conventions to ensure consistency and maintainability:

#### **Controllers**
- Format: `[Name]Controller.cs`
- Examples: `BooksController.cs`, `LoansController.cs`, `MembersController.cs`, `FinesController.cs`

#### **Views**
- Format: `[ActionName].cshtml`
- Examples: `Index.cshtml`, `Create.cshtml`, `Edit.cshtml`, `Details.cshtml`, `Return.cshtml`
- Views are organized in folders matching their controller names

#### **Models**
- **Domain Entities**: `[Name].cs` (e.g., `Book.cs`, `Member.cs`, `Loan.cs`, `Fine.cs`)
- **ViewModels**: `[Name]ViewModel.cs` (e.g., `BookDetailsViewModel.cs`, `LoanProcessViewModel.cs`)
- **DTOs**: `[Name]Dto.cs` (e.g., `BookDto.cs`, `MemberDto.cs`)
- **API Models**: `[Name]Request.cs` / `[Name]Response.cs` (e.g., `CreateLoanRequest.cs`, `FineCalculationResponse.cs`)

#### **Avoiding Name Collisions**
To prevent naming conflicts, the project uses:
- **Namespaces**: Different types in separate namespaces (`TheSkibiditeca.Models`, `TheSkibiditeca.ViewModels`)
- **Contextual suffixes**: `ViewModel`, `Dto`, `Request`, `Response` when needed
- **Folder organization**: Logical separation by purpose and layer

#### **General Guidelines**
- **Language**: All code, comments, and commit messages should be in English
- **C# Conventions**: Follow Microsoft's official C# coding standards
- **Pascal Case**: Classes, methods, properties (`BookController`, `GetBookById`)
- **Camel Case**: Local variables, parameters (`bookId`, `userName`)
- **Commits**: Use conventional commit format (`feat:`, `fix:`, `docs:`, etc.)

## Contributing

This is an educational project for the Applied Programming II course. Contributions are managed by the development team.

## License

This project is licensed under the GNU Affero General Public License v3.0 - see the [LICENSE.txt](LICENSE.txt) file for details.

## Academic Context

**Course**: Programación Aplicada II (11Q238)  
**Institution**: Universidad Nacional de Cajamarca  
**Academic Year**: 2025  

---

*Built with ❤️ by the dominuxLABS team*