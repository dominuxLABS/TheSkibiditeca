# TheSkibiditeca

A modern library management system built with ASP.NET Core for the Applied Programming II course.

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