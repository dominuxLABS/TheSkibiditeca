// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.AspNetCore.Identity;
using TheSkibiditeca.Web.Logging;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Data
{
    /// <summary>
    /// Provides seed data for the library database.
    /// </summary>
    public static class DbSeeder
    {
        private static readonly string[] PublisherNames = new[]
        {
            "Penguin", "HarperCollins", "Random House", "Oxford", "Cambridge", "Hachette", "Simon & Schuster",
        };

    /// <summary>
    /// Seeds the database with initial data using Identity APIs for users and roles.
    /// </summary>
    /// <param name="services">Service provider (will be used to resolve scoped services).</param>
    /// <param name="logger">Logger to report seeding errors and progress.</param>
    /// <returns>A task that completes once seeding finishes.</returns>
        public static async Task SeedDataAsync(IServiceProvider services, ILogger logger)
        {
            var context = services.GetRequiredService<LibraryDbContext>();

            // Ensure database is created (safe during development)
            context.Database.EnsureCreated();

            // If authors exist we assume the DB was seeded already
            if (context.Authors.Any())
            {
                return;
            }

            // Resolve Identity managers
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

            // Seed User Types
            var userTypes = new[]
            {
                new UserType { Name = "Estudiante", MaxLoanDays = 14, MaxBooksAllowed = 3, CanRenew = true, DailyFineAmount = 0.50m },
                new UserType { Name = "Profesor", MaxLoanDays = 30, MaxBooksAllowed = 10, CanRenew = true, DailyFineAmount = 1.00m },
                new UserType { Name = "Personal", MaxLoanDays = 21, MaxBooksAllowed = 5, CanRenew = true, DailyFineAmount = 0.75m },
                new UserType { Name = "Usuario Externo", MaxLoanDays = 7, MaxBooksAllowed = 2, CanRenew = false, DailyFineAmount = 2.00m },
            };
            context.UserTypes.AddRange(userTypes);

            // Seed Categories (small subset for speed)
            var categories = new Category[]
            {
                new() { Name = "Fantasía", Description = "Literatura fantástica" },
                new() { Name = "Ciencia Ficción", Description = "Ciencia ficción y distopía" },
                new() { Name = "Romance", Description = "Novelas románticas" },
                new() { Name = "Historia", Description = "Libros y ficción histórica" },
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            // Seed Authors (small subset)
            var authors = new Author[]
            {
                new() { FirstName = "Desconocido", LastName = "Autor", Nationality = "Desconocida", Biography = "Autor anónimo" },
                new() { FirstName = "J.R.R.", LastName = "Tolkien", Nationality = "Británica", Biography = "Autor de El Señor de los Anillos" },
                new() { FirstName = "Lewis", LastName = "Carroll", Nationality = "Inglesa", Biography = "Autor de Alicia en el País de las Maravillas" },
            };
            context.Authors.AddRange(authors);
            context.SaveChanges();

            // Seed Books (small subset)
            var books = new Book[]
            {
                new() { Title = "Beowulf", PublicationYear = 800, CategoryId = categories[0].CategoryId, Description = "Épica Fantástica" },
                new() { Title = "El Señor de los Anillos", PublicationYear = 1954, CategoryId = categories[0].CategoryId, Description = "Fantasía" },
                new() { Title = "Alicia en el País de las Maravillas", PublicationYear = 1865, CategoryId = categories[1].CategoryId, Description = "Fantasía" },
            };
            context.Books.AddRange(books);
            context.SaveChanges();

            // Seed Copies (few copies for demo)
            var usedIsbns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                usedIsbns.UnionWith(context.Copies.Where(c => !string.IsNullOrEmpty(c.ISBN)).Select(c => c.ISBN!));
            }
            catch (Exception ex)
            {
                // Table may not exist yet; log and continue
                DatabaseSetupLoggers.DatabaseSetupError(logger, ex);
            }

            var copies = new List<Copy>
            {
                new() { BookId = books[0].BookId, ISBN = GenerateIsbn(), PublisherName = PublisherNames[0], PhysicalLocation = "Estante A-1", IsActive = true },
                new() { BookId = books[1].BookId, ISBN = GenerateIsbn(), PublisherName = PublisherNames[1], PhysicalLocation = "Estante A-2", IsActive = true },
            };
            context.Copies.AddRange(copies);
            context.SaveChanges();

            // Ensure default roles
            const string adminRole = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(adminRole));
            }

            // Seed users using UserManager
            var seedUsers = new[]
            {
                new User
                {
                    UserCode = "EST001",
                    FirstName = "Ana",
                    LastName = "García",
                    Email = "ana.garcia@universidad.edu",
                    Phone = "+52-555-123-4567",
                    Address = "Av. Universidad 123",
                    UserTypeId = userTypes[0].UserTypeId,
                    CareerDepartment = "Ciencias de la Computación",
                    RegistrationDate = DateTime.UtcNow.AddMonths(-12).Date,
                    MembershipExpirationDate = DateTime.UtcNow.AddMonths(12).Date,
                },
                new User
                {
                    UserCode = "ADMIN",
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@localhost",
                    Phone = "+00-000-000-0000",
                    Address = "Localhost",
                    UserTypeId = userTypes[1].UserTypeId,
                    CareerDepartment = "IT",
                    RegistrationDate = DateTime.UtcNow.Date,
                    MembershipExpirationDate = DateTime.UtcNow.Date.AddYears(1),
                },
            };

            const string defaultPassword = "ChangeMe123!";

            foreach (var u in seedUsers)
            {
                var existsByName = await userManager.FindByNameAsync(u.UserCode);
                var existsByEmail = await userManager.FindByEmailAsync(u.Email!);
                if (existsByName != null || existsByEmail != null)
                {
                    continue;
                }

                var createResult = await userManager.CreateAsync(u, defaultPassword);
                if (!createResult.Succeeded)
                {
                    DatabaseSetupLoggers.DatabaseSetupError(logger, new InvalidOperationException(string.Join("; ", createResult.Errors.Select(e => e.Description))));
                    continue;
                }

                if (u.UserCode == "ADMIN")
                {
                    await userManager.AddToRoleAsync(u, adminRole);
                }
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Generates a pseudo-ISBN-13 string using the 978 prefix for seeding/demo purposes.
        /// </summary>
        /// <returns>A 13-digit ISBN-like string with a computed check digit.</returns>
        private static string GenerateIsbn()
        {
            // Build 12-digit payload: 978 + 9 random digits
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            Span<byte> bytes = stackalloc byte[6]; // 6 bytes -> will be used to derive digits
            rng.GetBytes(bytes);

            var sb = new System.Text.StringBuilder("978");
            for (int i = 0; i < 9; i++)
            {
                int digit = bytes[i % bytes.Length] % 10;
                sb.Append((char)('0' + digit));
            }

            var payload = sb.ToString(); // 12 digits

            // compute check digit for ISBN-13
            int sum = 0;
            for (int i = 0; i < payload.Length; i++)
            {
                int d = payload[i] - '0';
                sum += (i % 2 == 0) ? d : d * 3;
            }

            int remainder = sum % 10;
            int check = remainder == 0 ? 0 : 10 - remainder;

            return payload + check.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
