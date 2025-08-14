# TheSkibiditeca - Database Models Documentation

## Overview

This document describes the database models implemented for the TheSkibiditeca library management system using Entity Framework Core with Code First approach.

## Database Schema

### Core Entities

#### Authors
Stores information about book authors.
- **AuthorId** (PK): Unique identifier
- **FirstName**: Author's first name
- **LastName**: Author's last name
- **Biography**: Optional biography
- **BirthDate**: Optional birth date
- **Nationality**: Optional nationality
- **CreatedAt/UpdatedAt**: Audit timestamps
- **IsActive**: Soft delete flag

#### Categories
Book categories for organization.
- **CategoryId** (PK): Unique identifier
- **Name**: Category name (unique)
- **Description**: Optional description
- **CreatedAt**: Creation timestamp
- **IsActive**: Soft delete flag

#### Publishers
Book publishers information.
- **PublisherId** (PK): Unique identifier
- **Name**: Publisher name
- **Address/Phone/Email/Website**: Contact information
- **CreatedAt**: Creation timestamp
- **IsActive**: Soft delete flag

#### Books
Core book information.
- **BookId** (PK): Unique identifier
- **Title**: Book title
- **ISBN**: Unique ISBN (optional)
- **PublisherId** (FK): Reference to publisher
- **PublicationYear**: Year of publication
- **CategoryId** (FK): Reference to category
- **NumberOfPages**: Page count
- **Language**: Book language (default: English)
- **PhysicalLocation**: Physical location in library
- **TotalQuantity**: Total copies owned
- **AvailableQuantity**: Currently available copies
- **Description**: Book description
- **CoverImageUrl**: URL to cover image
- **AcquisitionPrice/AcquisitionDate**: Purchase information
- **CreatedAt/UpdatedAt**: Audit timestamps
- **IsActive**: Soft delete flag

#### BookAuthors
Many-to-many relationship between Books and Authors.
- **BookId** (PK, FK): Reference to book
- **AuthorId** (PK, FK): Reference to author
- **Role**: Author's role (Author, Co-author, Editor, etc.)

### User Management

#### UserTypes
Different types of library users with different privileges.
- **UserTypeId** (PK): Unique identifier
- **Name**: Type name (Student, Professor, Staff, External)
- **MaxLoanDays**: Maximum loan duration
- **MaxBooksAllowed**: Maximum books that can be borrowed
- **CanRenew**: Whether renewals are allowed
- **DailyFineAmount**: Daily fine amount for overdue books
- **CreatedAt**: Creation timestamp
- **IsActive**: Soft delete flag

#### Users
Library users (students, faculty, staff, external users).
- **UserId** (PK): Unique identifier
- **UserCode**: Unique user code (student ID, employee ID)
- **FirstName/LastName**: User's name
- **Email**: Unique email address
- **Phone/Address**: Contact information
- **UserTypeId** (FK): Reference to user type
- **CareerDepartment**: Academic department or career
- **RegistrationDate**: When user registered
- **MembershipExpirationDate**: When membership expires
- **CreatedAt/UpdatedAt**: Audit timestamps
- **IsActive**: Soft delete flag

### Loan Management

#### LoanStatuses
Possible states of a loan.
- **LoanStatusId** (PK): Unique identifier
- **Name**: Status name (Active, Returned, Overdue, Renewed, Lost)
- **Description**: Status description
- **IsActive**: Soft delete flag

#### Loans
Book loan transactions.
- **LoanId** (PK): Unique identifier
- **BookId** (FK): Reference to borrowed book
- **UserId** (FK): Reference to borrowing user
- **LoanStatusId** (FK): Current loan status
- **LoanDate**: When book was borrowed
- **ExpectedReturnDate**: When book should be returned
- **ActualReturnDate**: When book was actually returned (optional)
- **RenewalsCount**: Number of times renewed
- **MaxRenewals**: Maximum renewals allowed
- **Observations**: Additional notes
- **StaffMember**: Staff member who processed the loan
- **CreatedAt/UpdatedAt**: Audit timestamps

#### Reservations
Book reservations for when books are not available.
- **ReservationId** (PK): Unique identifier
- **BookId** (FK): Reference to reserved book
- **UserId** (FK): Reference to user making reservation
- **ReservationDate**: When reservation was made
- **ExpirationDate**: When reservation expires
- **IsNotified**: Whether user has been notified of availability
- **IsActive**: Whether reservation is still active
- **CreatedAt**: Creation timestamp

### Fine Management

#### FineTypes
Different types of fines that can be applied.
- **FineTypeId** (PK): Unique identifier
- **Name**: Fine type name (Late Return, Damage, Lost Book, etc.)
- **Description**: Fine description
- **BaseAmount**: Base fine amount
- **IsActive**: Soft delete flag

#### Fines
Individual fine instances.
- **FineId** (PK): Unique identifier
- **LoanId** (FK): Reference to associated loan (optional)
- **UserId** (FK): Reference to user who owes fine
- **FineTypeId** (FK): Reference to fine type
- **Amount**: Fine amount
- **FineDate**: When fine was assessed
- **PaymentDate**: When fine was paid (optional)
- **DaysOverdue**: Number of days overdue (for late returns)
- **Description**: Additional description
- **IsPaid**: Whether fine has been paid
- **CreatedAt/UpdatedAt**: Audit timestamps

### Audit Trail

#### AuditLogs
System audit trail for tracking changes.
- **AuditLogId** (PK): Unique identifier
- **TableName**: Name of affected table
- **RecordId**: ID of affected record
- **Action**: Action performed (INSERT, UPDATE, DELETE)
- **OldData/NewData**: Before/after data (JSON)
- **SystemUser**: User who made the change
- **Timestamp**: When change occurred
- **IpAddress**: IP address of user

## Relationships

### One-to-Many Relationships
- **Publisher** → **Books** (One publisher can publish many books)
- **Category** → **Books** (One category can contain many books)
- **UserType** → **Users** (One user type can have many users)
- **User** → **Loans** (One user can have many loans)
- **User** → **Reservations** (One user can have many reservations)
- **User** → **Fines** (One user can have many fines)
- **Book** → **Loans** (One book can have many loans)
- **Book** → **Reservations** (One book can have many reservations)
- **LoanStatus** → **Loans** (One status can apply to many loans)
- **FineType** → **Fines** (One fine type can have many instances)
- **Loan** → **Fines** (One loan can generate many fines)

### Many-to-Many Relationships
- **Books** ↔ **Authors** (through BookAuthors table)

## Key Features

### Data Integrity
- Foreign key constraints ensure referential integrity
- Unique constraints on critical fields (ISBN, UserCode, Email)
- Check constraints for valid data ranges
- Cascade and restrict delete behaviors appropriately configured

### Audit Trail
- Automatic timestamp management (CreatedAt, UpdatedAt)
- Soft deletes using IsActive flags
- Comprehensive audit logging system

### Business Logic
- Computed properties for common calculations (IsAvailable, IsOverdue, etc.)
- Flexible user type system with configurable limits
- Support for book renewals and reservations
- Comprehensive fine management system

### Performance Considerations
- Indexed fields for frequently queried data
- Optimized relationships with appropriate delete behaviors
- Efficient many-to-many relationship handling

## Usage Examples

### Adding a New Book
```csharp
var book = new Book
{
    Title = "Sample Book",
    ISBN = "9781234567890",
    PublisherId = 1,
    CategoryId = 1,
    TotalQuantity = 3,
    AvailableQuantity = 3
};
context.Books.Add(book);
context.SaveChanges();
```

### Creating a Loan
```csharp
var loan = new Loan
{
    BookId = 1,
    UserId = 1,
    LoanStatusId = 1, // Active
    ExpectedReturnDate = DateTime.UtcNow.AddDays(14)
};
context.Loans.Add(loan);

// Update book availability
var book = context.Books.Find(1);
book.AvailableQuantity--;
context.SaveChanges();
```

### Querying Overdue Loans
```csharp
var overdueLoans = context.Loans
    .Include(l => l.Book)
    .Include(l => l.User)
    .Where(l => l.ActualReturnDate == null && 
                l.ExpectedReturnDate < DateTime.UtcNow.Date)
    .ToList();
```

## Migration Commands

### Create New Migration
```bash
dotnet ef migrations add [MigrationName] --project ./TheSkibiditeca.Web/TheSkibiditeca.Web.csproj
```

### Update Database
```bash
dotnet ef database update --project ./TheSkibiditeca.Web/TheSkibiditeca.Web.csproj
```

### Remove Last Migration
```bash
dotnet ef migrations remove --project ./TheSkibiditeca.Web/TheSkibiditeca.Web.csproj
```

## Configuration

The database models are configured in `LibraryDbContext.cs` with:
- Entity relationships
- Index definitions
- Unique constraints
- Default values
- Delete behaviors

Connection string is configured in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TheSkibiditecaDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```
