IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [AuditLogs] (
        [AuditLogId] int NOT NULL IDENTITY,
        [TableName] nvarchar(50) NOT NULL,
        [RecordId] int NOT NULL,
        [Action] nvarchar(20) NOT NULL,
        [OldData] text NULL,
        [NewData] text NULL,
        [SystemUser] nvarchar(100) NULL,
        [Timestamp] datetime2 NOT NULL,
        [IpAddress] nvarchar(45) NULL,
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([AuditLogId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [Authors] (
        [AuthorId] int NOT NULL IDENTITY,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(100) NOT NULL,
        [Biography] text NULL,
        [BirthDate] datetime2 NULL,
        [Nationality] nvarchar(50) NULL,
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Authors] PRIMARY KEY ([AuthorId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [Categories] (
        [CategoryId] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] text NULL,
        [CreatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [UserTypes] (
        [UserTypeId] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NOT NULL,
        [MaxLoanDays] int NOT NULL,
        [MaxBooksAllowed] int NOT NULL,
        [CanRenew] bit NOT NULL,
        [DailyFineAmount] decimal(5,2) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_UserTypes] PRIMARY KEY ([UserTypeId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] int NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [Books] (
        [BookId] int NOT NULL IDENTITY,
        [Title] nvarchar(300) NOT NULL,
        [PublicationYear] int NULL,
        [CategoryId] int NULL,
        [Description] text NULL,
        [CoverImageUrl] nvarchar(500) NULL,
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        CONSTRAINT [PK_Books] PRIMARY KEY ([BookId]),
        CONSTRAINT [FK_Books_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([CategoryId]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [UserId] int NOT NULL IDENTITY,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(100) NOT NULL,
        [Email] nvarchar(150) NOT NULL,
        [Address] nvarchar(300) NULL,
        [UserTypeId] int NOT NULL,
        [CareerDepartment] nvarchar(150) NULL,
        [RegistrationDate] datetime2 NOT NULL,
        [MembershipExpirationDate] datetime2 NULL,
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [IsActive] bit NOT NULL,
        [UserCode] nvarchar(20) NOT NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(500) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [Phone] nvarchar(20) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([UserId]),
        CONSTRAINT [FK_Users_UserTypes_UserTypeId] FOREIGN KEY ([UserTypeId]) REFERENCES [UserTypes] ([UserTypeId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [BookAuthors] (
        [BookId] int NOT NULL,
        [AuthorId] int NOT NULL,
        [Role] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_BookAuthors] PRIMARY KEY ([BookId], [AuthorId]),
        CONSTRAINT [FK_BookAuthors_Authors_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Authors] ([AuthorId]) ON DELETE CASCADE,
        CONSTRAINT [FK_BookAuthors_Books_BookId] FOREIGN KEY ([BookId]) REFERENCES [Books] ([BookId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [Copies] (
        [CopyId] int NOT NULL IDENTITY,
        [BookId] int NOT NULL,
        [ISBN] nvarchar(20) NULL,
        [PublisherName] nvarchar(200) NULL,
        [PhysicalLocation] nvarchar(100) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Copies] PRIMARY KEY ([CopyId]),
        CONSTRAINT [FK_Copies_Books_BookId] FOREIGN KEY ([BookId]) REFERENCES [Books] ([BookId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] int NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] int NOT NULL,
        [RoleId] int NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] int NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [Loans] (
        [LoanId] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [Status] int NOT NULL,
        [LoanDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [ExpectedReturnDate] datetime2 NOT NULL,
        [ActualReturnDate] datetime2 NULL,
        [RenewalsCount] int NOT NULL,
        [MaxRenewals] int NOT NULL,
        [Observations] text NULL,
        [StaffMember] nvarchar(100) NULL,
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        CONSTRAINT [PK_Loans] PRIMARY KEY ([LoanId]),
        CONSTRAINT [FK_Loans_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [Reservations] (
        [ReservationId] int NOT NULL IDENTITY,
        [CopyId] int NOT NULL,
        [UserId] int NOT NULL,
        [ReservationDate] datetime2 NOT NULL,
        [ExpirationDate] datetime2 NOT NULL,
        [IsNotified] bit NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Reservations] PRIMARY KEY ([ReservationId]),
        CONSTRAINT [FK_Reservations_Copies_CopyId] FOREIGN KEY ([CopyId]) REFERENCES [Copies] ([CopyId]) ON DELETE CASCADE,
        CONSTRAINT [FK_Reservations_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [Fines] (
        [FineId] int NOT NULL IDENTITY,
        [LoanId] int NULL,
        [UserId] int NOT NULL,
        [Amount] decimal(10,2) NOT NULL,
        [FineDate] datetime2 NOT NULL,
        [PaymentDate] datetime2 NULL,
        [DaysOverdue] int NOT NULL,
        [Description] text NULL,
        [IsPaid] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Fines] PRIMARY KEY ([FineId]),
        CONSTRAINT [FK_Fines_Loans_LoanId] FOREIGN KEY ([LoanId]) REFERENCES [Loans] ([LoanId]) ON DELETE SET NULL,
        CONSTRAINT [FK_Fines_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE TABLE [LoanDetails] (
        [LoanDetailId] int NOT NULL IDENTITY,
        [LoanId] int NOT NULL,
        [CopyId] int NOT NULL,
        [Quantity] int NOT NULL,
        [DateAdded] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [Notes] text NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        CONSTRAINT [PK_LoanDetails] PRIMARY KEY ([LoanDetailId]),
        CONSTRAINT [FK_LoanDetails_Copies_CopyId] FOREIGN KEY ([CopyId]) REFERENCES [Copies] ([CopyId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_LoanDetails_Loans_LoanId] FOREIGN KEY ([LoanId]) REFERENCES [Loans] ([LoanId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_BookAuthors_AuthorId] ON [BookAuthors] ([AuthorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Books_CategoryId] ON [Books] ([CategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Categories_Name] ON [Categories] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Copies_BookId] ON [Copies] ([BookId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Copies_ISBN] ON [Copies] ([ISBN]) WHERE [ISBN] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Fines_LoanId] ON [Fines] ([LoanId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Fines_UserId] ON [Fines] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_LoanDetails_CopyId] ON [LoanDetails] ([CopyId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_LoanDetails_LoanId] ON [LoanDetails] ([LoanId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Loans_UserId] ON [Loans] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Reservations_CopyId] ON [Reservations] ([CopyId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Reservations_UserId] ON [Reservations] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [Users] ([NormalizedEmail]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_UserCode] ON [Users] ([UserCode]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Users_UserTypeId] ON [Users] ([UserTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [Users] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_UserTypes_Name] ON [UserTypes] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250824093304_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250824093304_InitialCreate', N'9.0.8');
END;

COMMIT;
GO

