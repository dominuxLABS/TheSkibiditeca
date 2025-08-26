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
                 new() { Name = "No Ficción", Description = "Libros de Autoayuda"},
                 new() { Name = "Música", Description = "Libros sobre música"},
                 new() { Name = "Acción", Description = "libros de Accion"},
             };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            // Seed Authors (small subset)
            var authors = new Author[]
            {
             new() { FirstName = "Autor", LastName = "Desconocido", Nationality = "Desconocida", Biography = "Autor anónimo" },
             new() { FirstName = "J.R.R.", LastName = "Tolkien", Nationality = "Británica", Biography = "Autor de El Señor de los Anillos" },
             new() { FirstName = "Lewis", LastName = "Carroll", Nationality = "Inglesa", Biography = "Autor de Alicia en el País de las Maravillas" },
             new() { FirstName = "Antonio", LastName = "Ezquerro Esteban", Nationality = "Epañola", Biography = "Autor de Teoria Musical"},
             new() { FirstName = "Oriol", LastName = "Brugarolas Bonet", Nationality = "Epañola", Biography = "Autor de Teoria Musical"},
             new() { FirstName = "Javier", LastName = "Artigas Pina", Nationality = "Epañola", Biography = "Autor de Teoria Musical"},
             new() { FirstName = "Osamu", LastName = "Dazai", Nationality = "Japones", Biography = "Autor de Indigno de ser Humano"},
             new() { FirstName = "Mark", LastName = "Manson", Nationality = "EstadoUnidense", Biography = "Autor de El sutil arte de que te importe un carajo"},
             new() { FirstName = "Fernando", LastName = "Villarán", Nationality = "Peruano", Biography = "Autor de El regreso del Huáscar"},
             new() { FirstName = "Mípham", LastName = "Sakyong", Nationality = "Indio", Biography = "Autor de "},
             new() { FirstName = "Walter", LastName = "Isaacson", Nationality = "Estadounidense", Biography = "Autor de la biografia de Elon Musk"},
             new() { FirstName = "Jaime", LastName = "Bayly", Nationality = "Peruano", Biography = "Autor de"},
             new() { FirstName = "Mel", LastName = "Robbins", Nationality = "Estadounidense", Biography = "Autor de"},
             new() { FirstName = "Pablo", LastName = "Vierci", Nationality = "Uruguayo", Biography = "Autor de"},
             new() { FirstName = "Giacomo", LastName = "Bocchio", Nationality = "Peruano", Biography = "Autor de"},
             new() { FirstName = "Lucio Antonio", LastName = "Vásquez", Nationality = "Peruano", Biography = "Autor de"},
             new() { FirstName = "Roque", LastName = "Benavides Ganoza", Nationality = "Peruano", Biography = "Autor de"},
            };
            context.Authors.AddRange(authors);
            context.SaveChanges();

            // Seed Books (small subset)
            var books = new Book[]
            {
             new() { Title = "Beowulf", CoverImageUrl = "https://www.crisol.com.pe/media/catalog/product/cache/cf84e6047db2ba7f2d5c381080c69ffe/9/7/9788445009871_c0nxmf9mcsrbokqu.jpg", PublicationYear = 800, CategoryId = categories[0].CategoryId, Description = "Beowulf, adaptado al español como Beovulfo, es un poema épico anglosajón anónimo que fue escrito en inglés antiguo en verso aliterativo. Cuenta con 3182 versos." },
             new() { Title = "El Señor de los Anillos", CoverImageUrl = "https://www.crisol.com.pe/media/catalog/product/cache/f6d2c62455a42b0d712f6c919e880845/9/7/9788445011119_hfv2hzneyamrzdod.jpg", PublicationYear = 1954, CategoryId = categories[0].CategoryId, Description = "El Señor de los Anillos es una novela de fantasía épica escrita por el filólogo y escritor británico J. R. R. Tolkien." },
             new() { Title = "Alicia en el País de las Maravillas", CoverImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTw5JN2j8YUEfm_YkoqOaCVRqnIv3dEKlu09g&s", PublicationYear = 1865, CategoryId = categories[1].CategoryId, Description = "Las aventuras de Alicia en el país de las maravillas, comúnmente abreviado como Alicia en el país de las maravillas, es una novela de fantasía escrita por el matemático, lógico, fotógrafo y escritor británico Charles Lutwidge Dodgson, bajo el seudónimo de Lewis Carroll, publicada en 1865." },
             new() { Title = "Repertorio inédito para tecla en la Barcelona de comienzos del siglo XIX: de la ópera al salón y el convento", CoverImageUrl = "https://m.media-amazon.com/images/I/51MTlTM50nL._SY425_.jpg", PublicationYear = 2025, CategoryId = categories[5].CategoryId, Description = "Este volumen recoge la edición de música para tecla (piano y órgano) de finales del siglo XVIII e inicios del siglo XIX del Ms. 1378 de la Biblioteca de Fondo Antiguo de la Universitat de Barcelona, en formato de partitura moderna y con dos estudios críticos."},
             new() { Title = "Indigno de Ser Humano", CoverImageUrl = "https://www.crisol.com.pe/media/catalog/product/cache/cf84e6047db2ba7f2d5c381080c69ffe/9/7/9786129901107_ujgxzb0yrgpdwxfd.png", PublicationYear = 1948, CategoryId = categories[2].CategoryId, Description = "Indigno de ser humano es narrado en forma de varios cuadernos dejados por Yōzō Ōba (大庭 葉蔵), un joven hombre afligido e incapaz de revelar su verdadero ser a los demás, y que, en cambio se ve obligado a mantener una fachada de jocosidad vacía."},
             new() { Title = "El Regreso del Huáscar", CoverImageUrl = "https://www.crisol.com.pe/media/catalog/product/cache/cf84e6047db2ba7f2d5c381080c69ffe/9/7/9786124898310_jwr3byeh0h6jenoh.jpg", PublicationYear = 2024, CategoryId = categories[1].CategoryId, Description = "En un inminente 2029, Perú y Chile vuelven a encontrarse en un conflicto que marcará la historia. La maquinaria bélica de ambos países se pone en marcha ya no por yacimientos de salitre sino por cuantiosos yacimientos de oro."},
             new() { Title = "Convertir La Mente En Nuestra Aliada", CoverImageUrl = "https://m.media-amazon.com/images/I/31QVcnZDt+L.jpg", PublicationYear = 2003, CategoryId = categories[4].CategoryId, Description = "La mente es como un caballo salvaje. Intentamos centrarla pero no se queda quieta en un sitio, sino que se va dando brincos y galopando de una cosa a la siguiente. La seguimos impotentes dondequiera que vaya, incapaces de conseguir lo que nos hemos propuesto en el trabajo, en la vida familiar o en un camino espiritual."},
             new() { Title = "Elon Musk", CoverImageUrl = "https://www.crisol.com.pe/media/catalog/product/cache/cf84e6047db2ba7f2d5c381080c69ffe/9/7/9786125068293_rjwf0x4a2mke6ltp.jpg", PublicationYear = 2023, CategoryId = categories[4].CategoryId, Description = "Del autor de Steve Jobs y otras grandes biografías, todas ellas éxitos internacionales de ventas, esta es la historia asombrosamente íntima del innovador más fascinante y polémico del mundo, un visionario que ha roto todos los moldes y ha conducido al mundo a la era de los vehículos eléctricos, la exploración espacial privada y la inteligencia artificial. Ah, y el mismo que compró Twitter."},
             new() { Title = "No se lo digas a nadie", CoverImageUrl = "https://www.crisol.com.pe/media/catalog/product/cache/cf84e6047db2ba7f2d5c381080c69ffe/9/7/9786125044471_4c3jb00eg8cbcnsx.jpg", PublicationYear = 1994, CategoryId = categories[4].CategoryId, Description = "Entre desencuentros y excesos, Joaquín busca la aceptación de su sexualidad dentro de una sociedad conservadora y desencantada, pero fracasa al no encontrar su lugar en el mundo. "},
             new() { Title = "Eleva tu juego culinario", CoverImageUrl = "https://www.crisol.com.pe/media/catalog/product/cache/cf84e6047db2ba7f2d5c381080c69ffe/9/7/9786123198695_hpr20if0s01sl1u5.jpg", PublicationYear = 2024, CategoryId = categories[4].CategoryId, Description = "Giacomo Bocchio es uno de los chefs más reconocidos y talentosos de nuestro país. A los veinticinco años se convirtió en el chef más joven del mundo en ingresar a la Academia Culinaria de Francia."},
             new() { Title = "La sociedad de la nieve", CoverImageUrl = "https://www.crisol.com.pe/media/catalog/product/cache/cf84e6047db2ba7f2d5c381080c69ffe/9/7/9786123199371_sflyja7ypcnsl4vl.jpg", PublicationYear = 2008, CategoryId = categories[6].CategoryId, Description = "Con alrededor de veinte años, a cuatro mil metros de altura y treinta grados bajo cero, sin abrigo ni comida, la relación entre víctimas y sobrevivientes se invirtió y solo regresaron con vida dieciséis."},
             new() { Title = "El poder de los 5 segundos", CoverImageUrl = "https://www.crisol.com.pe/media/catalog/product/cache/cf84e6047db2ba7f2d5c381080c69ffe/9/7/9786287596153.jpg", PublicationYear = 2018, CategoryId = categories[4].CategoryId, Description = "El libro que ha transformado la vida de millones de lectores. En El Poder de los 5 segundos descubrirás que son sólo cinco los segundos que necesitas para confiar, romper el hábito de la procrastinación y la duda, vencer el miedo, dejar de preocuparte y sentirte más feliz."},
             new() { Title = "Pack Víctor Raúl Haya de la Torre", CoverImageUrl = "https://www.crisol.com.pe/media/catalog/product/cache/cf84e6047db2ba7f2d5c381080c69ffe/7/7/7759022000118_30tuobf3zdudspxn.jpg", PublicationYear = 2024, CategoryId = categories[3].CategoryId, Description = "En estos dos volúmenes Roque Benavides y Lucio Antonio Vásquez logran combinar anécdotas personales y recuerdos familiares con una hábil utilización de fuentes y biografías. "},
             new() { Title = "El sutil arte de que te importe un carajo", CoverImageUrl = "https://www.crisol.com.pe/media/catalog/product/cache/cf84e6047db2ba7f2d5c381080c69ffe/9/7/9781404117150.jpg", PublicationYear = 2018, CategoryId = categories[4].CategoryId, Description = "Durante los últimos años, Mark Manson -en su popular blog- se ha afanado en corregir nuestras delirantes expectativas sobre nosotros mismos y el mundo. Ahora nos ofrece su toda su intrépida sabiduría en este libro pionero.Manson nos recuerda que los seres humanos somos falibles y limitados:no todos podemos ser extraordinarios: hay ganadores y perdedores en la sociedad, y esto no siempre es justo o es tu"},
            };
            context.Books.AddRange(books);
            context.SaveChanges();

            // Seed BookAuthors
            var authorBooks = new BookAuthor[] {
             new() {BookId = 1, AuthorId = 1, Role = "Writer" },
             new() {BookId = 2, AuthorId = 2, Role = "Writer" },
             new() {BookId = 3, AuthorId = 3, Role = "Writer" },
             new() {BookId = 4, AuthorId = 4, Role = "Writer" },
             new() {BookId = 4, AuthorId = 5, Role = "Writer" },
             new() {BookId = 4, AuthorId = 6, Role = "Writer" },
             new() {BookId = 5, AuthorId = 7, Role = "Writer" },
             new() {BookId = 6, AuthorId = 9, Role = "Writer" },
             new() {BookId = 7, AuthorId = 10, Role = "Writer" },
             new() {BookId = 8, AuthorId = 11, Role = "Writer" },
             new() {BookId = 9, AuthorId = 12, Role = "Writer" },
             new() {BookId = 10, AuthorId = 13, Role = "Writer" },
             new() {BookId = 11, AuthorId = 14, Role = "Writer" },
             new() {BookId = 12, AuthorId = 15, Role = "Writer" },
             new() {BookId = 13, AuthorId = 16, Role = "Writer" },
             new() {BookId = 13, AuthorId = 17, Role = "Writer" },
             new() {BookId = 14, AuthorId = 8, Role = "Writer" },

             };

            context.BookAuthors.AddRange(authorBooks);
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
                new() { BookId = books[2].BookId, ISBN = GenerateIsbn(), PublisherName = PublisherNames[1], PhysicalLocation = "Estante A-3", IsActive = true },
                new() { BookId = books[3].BookId, ISBN = GenerateIsbn(), PublisherName = PublisherNames[1], PhysicalLocation = "Estante A-4", IsActive = true },
                new() { BookId = books[4].BookId, ISBN = GenerateIsbn(), PublisherName = PublisherNames[0], PhysicalLocation = "Estante A-5", IsActive = true },
                new() { BookId = books[5].BookId, ISBN = GenerateIsbn(), PublisherName = PublisherNames[1], PhysicalLocation = "Estante A-6", IsActive = true },
                new() { BookId = books[6].BookId, ISBN = GenerateIsbn(), PublisherName = PublisherNames[2], PhysicalLocation = "Estante A-7", IsActive = true },
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
                    UserTypeId = userTypes[3].UserTypeId,
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
        public static string GenerateIsbn()
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
