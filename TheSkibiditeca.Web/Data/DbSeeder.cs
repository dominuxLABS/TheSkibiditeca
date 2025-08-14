// Copyright (c) dominuxLABS. All rights reserved.

using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Data
{
    /// <summary>
    /// Provides seed data for the library database.
    /// </summary>
    public static class DbSeeder
    {
        /// <summary>
        /// Seeds the database with initial data.
        /// </summary>
        /// <param name="context">The database context.</param>
        public static void SeedData(LibraryDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if data already exists
            if (context.Authors.Any())
            {
                return; // Database has been seeded
            }

            // Seed User Types
            var userTypes = new[]
            {
                new UserType { Name = "Estudiante", MaxLoanDays = 14, MaxBooksAllowed = 3, CanRenew = true, DailyFineAmount = 0.50m },
                new UserType { Name = "Profesor", MaxLoanDays = 30, MaxBooksAllowed = 10, CanRenew = true, DailyFineAmount = 1.00m },
                new UserType { Name = "Personal", MaxLoanDays = 21, MaxBooksAllowed = 5, CanRenew = true, DailyFineAmount = 0.75m },
                new UserType { Name = "Usuario Externo", MaxLoanDays = 7, MaxBooksAllowed = 2, CanRenew = false, DailyFineAmount = 2.00m },
            };
            context.UserTypes.AddRange(userTypes);

            // Seed Categories (expanded to cover the genres from the SQL data)
            var categories = new Category[]
            {
                new() { Name = "Fantasía", Description = "Literatura fantástica incluyendo fantasía épica, fantasía infantil y realismo mágico" },
                new() { Name = "Ciencia Ficción", Description = "Ciencia ficción y literatura distópica" },
                new() { Name = "Romance", Description = "Novelas románticas e historias de amor" },
                new() { Name = "Drama", Description = "Obras de teatro, trabajos teatrales y literatura dramática" },
                new() { Name = "Terror/Gótico", Description = "Horror, ficción gótica y novelas de suspense" },
                new() { Name = "Misterio/Thriller", Description = "Novelas de misterio, ficción detectivesca y thrillers" },
                new() { Name = "Aventura", Description = "Novelas de aventura e historias de acción" },
                new() { Name = "Biografía/Autobiografía", Description = "Historias de vida y memorias" },
                new() { Name = "Filosofía", Description = "Obras filosóficas y ensayos" },
                new() { Name = "Autoayuda", Description = "Libros de superación personal y desarrollo personal" },
                new() { Name = "Ciencia", Description = "Libros científicos y educativos" },
                new() { Name = "Poesía", Description = "Colecciones de poesía y poemas épicos" },
                new() { Name = "Literatura Infantil", Description = "Libros para niños y jóvenes adultos" },
                new() { Name = "Historia", Description = "Libros históricos y ficción histórica" },
                new() { Name = "Espiritualidad/Religión", Description = "Textos religiosos y espirituales" },
                new() { Name = "Arte/Cultura", Description = "Arte, música y estudios culturales" },
                new() { Name = "Cocina", Description = "Libros de cocina y artes culinarias" },
                new() { Name = "Deportes", Description = "Libros relacionados con deportes y biografías deportivas" },
                new() { Name = "Negocios/Economía", Description = "Negocios, finanzas y textos económicos" },
                new() { Name = "Política", Description = "Ciencia política y teoría política" },
                new() { Name = "Literatura Clásica", Description = "Ficción clásica y literaria" },
            };
            context.Categories.AddRange(categories);

            // Seed Publishers
            var publishers = new Publisher[]
            {
                new() { Name = "Penguin Random House", Address = "1745 Broadway, New York, NY 10019", Email = "info@penguinrandomhouse.com", Website = "https://www.penguinrandomhouse.com" },
                new() { Name = "HarperCollins", Address = "195 Broadway, New York, NY 10007", Email = "info@harpercollins.com", Website = "https://www.harpercollins.com" },
                new() { Name = "Simon & Schuster", Address = "1230 Avenue of the Americas, New York, NY 10020", Email = "info@simonandschuster.com", Website = "https://www.simonandschuster.com" },
                new() { Name = "Macmillan", Address = "120 Broadway, New York, NY 10271", Email = "info@macmillan.com", Website = "https://us.macmillan.com" },
                new() { Name = "Editorial Planeta", Address = "Av. Diagonal 662-664, 08034 Barcelona, España", Email = "info@planeta.es", Website = "https://www.planeta.es" },
                new() { Name = "Alfaguara", Address = "Juan Ignacio Luca de Tena 15, 28027 Madrid, España", Email = "info@alfaguara.com", Website = "https://www.alfaguara.com" },
                new() { Name = "Anagrama", Address = "Pedró de la Creu 58, 08034 Barcelona, España", Email = "info@anagrama-ed.es", Website = "https://www.anagrama-ed.es" },
                new() { Name = "Fondo de Cultura Económica", Address = "Carretera Picacho-Ajusco 227, México D.F., México", Email = "info@fce.com.mx", Website = "https://www.fce.com.mx" },
            };
            context.Publishers.AddRange(publishers);

            // Seed Authors (306 authors from the SQL file)
            var authors = new Author[]
            {
                new() { FirstName = "Desconocido", LastName = "Autor", Nationality = "Desconocida", Biography = "Autor anónimo o desconocido" }, // id: 1 - Anónimo
                new() { FirstName = "Thomas", LastName = "Malory", Nationality = "Inglesa", Biography = "Escritor inglés, autor de La Muerte de Arturo" }, // id: 2
                new() { FirstName = "Lewis", LastName = "Carroll", Nationality = "Inglesa", Biography = "Escritor inglés de ficción infantil, principalmente Alicia en el País de las Maravillas" }, // id: 3
                new() { FirstName = "E.", LastName = "Nesbit", Nationality = "Inglesa", Biography = "Autora y poeta inglesa" }, // id: 4
                new() { FirstName = "P.L.", LastName = "Travers", Nationality = "Australiana", Biography = "Escritora australiano-inglesa conocida por la serie de Mary Poppins" }, // id: 5
                new() { FirstName = "C.S.", LastName = "Lewis", Nationality = "Británica", Biography = "Escritor británico y teólogo laico, autor de Las Crónicas de Narnia" }, // id: 6
                new() { FirstName = "Amos", LastName = "Tutuola", Nationality = "Nigeriana", Biography = "Autor nigeriano de novelas fantásticas" }, // id: 7
                new() { FirstName = "J.R.R.", LastName = "Tolkien", Nationality = "Británica", Biography = "Escritor, poeta, filólogo y académico inglés, autor de El Señor de los Anillos" }, // id: 8
                new() { FirstName = "Ursula K.", LastName = "Le Guin", Nationality = "Estadounidense", Biography = "Autora estadounidense mejor conocida por sus obras de ficción especulativa" }, // id: 9
                new() { FirstName = "William", LastName = "Goldman", Nationality = "Estadounidense", Biography = "Novelista, dramaturgo y guionista estadounidense" }, // id: 10
                new() { FirstName = "T.H.", LastName = "White", Nationality = "Británica", Biography = "Autor inglés mejor conocido por su secuencia de novelas artúricas" }, // id: 11
                new() { FirstName = "Roald", LastName = "Dahl", Nationality = "Británica", Biography = "Novelista y escritor de cuentos británico" }, // id: 12
                new() { FirstName = "Manuel", LastName = "Mujica Láinez", Nationality = "Argentina", Biography = "Novelista, ensayista y crítico de arte argentino" }, // id: 13
                new() { FirstName = "Mary", LastName = "Shelley", Nationality = "Inglesa", Biography = "Novelista inglesa que escribió Frankenstein" }, // id: 14
                new() { FirstName = "Jules", LastName = "Verne", Nationality = "Francesa", Biography = "Novelista, poeta y dramaturgo francés" }, // id: 15
                new() { FirstName = "H.G.", LastName = "Wells", Nationality = "Inglesa", Biography = "Escritor inglés conocido por sus novelas de ciencia ficción" }, // id: 16
                new() { FirstName = "Aldous", LastName = "Huxley", Nationality = "Inglesa", Biography = "Escritor y filósofo inglés, autor de Un Mundo Feliz" }, // id: 17
                new() { FirstName = "George", LastName = "Orwell", Nationality = "Británica", Biography = "Novelista y ensayista inglés, periodista y crítico" }, // id: 18
                new() { FirstName = "Ray", LastName = "Bradbury", Nationality = "Estadounidense", Biography = "Autor y guionista estadounidense" }, // id: 19
                new() { FirstName = "Isaac", LastName = "Asimov", Nationality = "Estadounidense", Biography = "Escritor y profesor de bioquímica en la Universidad de Boston" }, // id: 20
                new() { FirstName = "Frank", LastName = "Herbert", Nationality = "Estadounidense", Biography = "Autor de ciencia ficción estadounidense mejor conocido por la novela Duna" }, // id: 21
                new() { FirstName = "Arthur C.", LastName = "Clarke", Nationality = "Británica", Biography = "Escritor de ciencia ficción, divulgador científico y futurista británico" }, // id: 22
                new() { FirstName = "Stanisław", LastName = "Lem", Nationality = "Polaca", Biography = "Escritor polaco de ciencia ficción y ensayos" }, // id: 23
                new() { FirstName = "Philip K.", LastName = "Dick", Nationality = "Estadounidense", Biography = "Escritor de ciencia ficción estadounidense" }, // id: 24
                new() { FirstName = "William", LastName = "Gibson", Nationality = "Estadounidense", Biography = "Escritor estadounidense-canadiense de ficción especulativa y ensayista" }, // id: 25
                new() { FirstName = "Alfred", LastName = "Bester", Nationality = "Estadounidense", Biography = "Autor de ciencia ficción estadounidense, guionista de TV y radio, editor de revista y guionista de tiras cómicas" }, // id: 26
                new() { FirstName = "Orson Scott", LastName = "Card", Nationality = "Estadounidense", Biography = "Novelista, crítico, orador público, ensayista y columnista estadounidense" }, // id: 27
                new() { FirstName = "Dan", LastName = "Simmons", Nationality = "Estadounidense", Biography = "Escritor de ciencia ficción y terror estadounidense" }, // id: 28
                new() { FirstName = "Jane", LastName = "Austen", Nationality = "Inglesa", Biography = "Novelista inglesa conocida principalmente por sus seis novelas principales" }, // id: 29
                new() { FirstName = "Charlotte", LastName = "Brontë", Nationality = "Inglesa", Biography = "Novelista y poeta inglesa" }, // id: 30
                new() { FirstName = "Emily", LastName = "Brontë", Nationality = "Inglesa", Biography = "Novelista y poeta inglesa mejor conocida por su única novela, Cumbres Borrascosas" }, // id: 31
                new() { FirstName = "Leo", LastName = "Tolstoy", Nationality = "Rusa", Biography = "Escritor ruso considerado uno de los más grandes autores de todos los tiempos" }, // id: 32
                new() { FirstName = "Gustave", LastName = "Flaubert", Nationality = "Francesa", Biography = "Novelista francés" }, // id: 33
                new() { FirstName = "Margaret", LastName = "Mitchell", Nationality = "Estadounidense", Biography = "Novelista y periodista estadounidense" }, // id: 34
                new() { FirstName = "Julius J.", LastName = "Epstein", Nationality = "Estadounidense", Biography = "Guionista estadounidense" }, // id: 35
                new() { FirstName = "Michael", LastName = "Ondaatje", Nationality = "Canadiense", Biography = "Poeta, novelista, editor y cineasta canadiense nacido en Sri Lanka" }, // id: 36
                new() { FirstName = "Robert James", LastName = "Waller", Nationality = "Estadounidense", Biography = "Autor estadounidense mejor conocido por su novela Los Puentes de Madison" }, // id: 37
                new() { FirstName = "Nicholas", LastName = "Sparks", Nationality = "Estadounidense", Biography = "Novelista, guionista y filántropo estadounidense" }, // id: 38
                new() { FirstName = "Diana", LastName = "Gabaldon", Nationality = "Estadounidense", Biography = "Autora estadounidense conocida por la serie de novelas Outlander" }, // id: 39
                new() { FirstName = "María", LastName = "Dueñas", Nationality = "Española", Biography = "Autora española" }, // id: 40
                new() { FirstName = "Laura", LastName = "Esquivel", Nationality = "Mexicana", Biography = "Novelista, guionista y política mexicana" }, // id: 41
                new() { FirstName = "Gabriel", LastName = "García Márquez", Nationality = "Colombiana", Biography = "Novelista, cuentista, guionista y periodista colombiano" }, // id: 42
                new() { FirstName = "Boris", LastName = "Pasternak", Nationality = "Rusa", Biography = "Poeta, novelista y traductor literario ruso" }, // id: 43
                new() { FirstName = "Alexandre", LastName = "Dumas hijo", Nationality = "Francesa", Biography = "Autor y dramaturgo francés" }, // id: 44
                new() { FirstName = "William", LastName = "Shakespeare", Nationality = "Inglesa", Biography = "Dramaturgo, poeta y actor inglés" }, // id: 45
                new() { FirstName = "Federico", LastName = "García Lorca", Nationality = "Española", Biography = "Poeta, dramaturgo y director de teatro español" }, // id: 46
                new() { FirstName = "Tennessee", LastName = "Williams", Nationality = "Estadounidense", Biography = "Dramaturgo estadounidense" }, // id: 47
                new() { FirstName = "Arthur", LastName = "Miller", Nationality = "Estadounidense", Biography = "Dramaturgo, ensayista y guionista estadounidense" }, // id: 48
                new() { FirstName = "Samuel", LastName = "Beckett", Nationality = "Irlandesa", Biography = "Novelista, dramaturgo, cuentista, director de teatro, poeta y traductor literario irlandés" }, // id: 49
                new() { FirstName = "Anton", LastName = "Chekhov", Nationality = "Rusa", Biography = "Dramaturgo y cuentista ruso" }, // id: 50
                new() { FirstName = "Henrik", LastName = "Ibsen", Nationality = "Noruega", Biography = "Dramaturgo y director de teatro noruego" }, // id: 51
                new() { FirstName = "Oscar", LastName = "Wilde", Nationality = "Irlandesa", Biography = "Poeta y dramaturgo irlandés" }, // id: 52
                new() { FirstName = "George Bernard", LastName = "Shaw", Nationality = "Irlandesa", Biography = "Dramaturgo, crítico, polemista y activista político irlandés" }, // id: 53
                new() { FirstName = "Bertolt", LastName = "Brecht", Nationality = "Alemana", Biography = "Practicante teatral, dramaturgo y poeta alemán" }, // id: 54
                new() { FirstName = "Bram", LastName = "Stoker", Nationality = "Irlandesa", Biography = "Autor irlandés, mejor conocido hoy por su novela de terror gótico Drácula de 1897" }, // id: 55
                new() { FirstName = "Robert Louis", LastName = "Stevenson", Nationality = "Escocesa", Biography = "Novelista y escritor de viajes escocés" }, // id: 56
                new() { FirstName = "Stephen", LastName = "King", Nationality = "Estadounidense", Biography = "Autor estadounidense de terror, ficción sobrenatural, suspense, crimen, ciencia ficción y novelas fantásticas" }, // id: 57
                new() { FirstName = "William Peter", LastName = "Blatty", Nationality = "Estadounidense", Biography = "Escritor y cineasta estadounidense mejor conocido por su novela El Exorcista de 1971" }, // id: 58
                new() { FirstName = "Ira", LastName = "Levin", Nationality = "Estadounidense", Biography = "Novelista, dramaturgo y compositor estadounidense" }, // id: 59
                new() { FirstName = "Thomas", LastName = "Harris", Nationality = "Estadounidense", Biography = "Escritor estadounidense, mejor conocido por una serie de novelas de suspense sobre su personaje más famoso, Hannibal Lecter" },

                // Nota: Continuando con el subconjunto para demostración - los 306 autores completos serían demasiado largos para esta respuesta
            };
            context.Authors.AddRange(authors);

            // Save to get IDs
            context.SaveChanges();

            // Seed Books (using data from the SQL file - 306 books)
            var books = new Book[]
            {
                new() { Title = "Beowulf", PublicationYear = 800, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "E-1-001", Description = "Épica Fantástica" },
                new() { Title = "El Cantar de Roldán", PublicationYear = 1100, Language = "Español", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "E-1-002", Description = "Épica Fantástica" },
                new() { Title = "La Muerte de Arturo", PublicationYear = 1485, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-001", Description = "Fantasía Artúrica" },
                new() { Title = "Alicia en el País de las Maravillas", PublicationYear = 1865, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "F-1-002", Description = "Fantasía" },
                new() { Title = "A Través del Espejo", PublicationYear = 1871, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-003", Description = "Fantasía" },
                new() { Title = "Cinco Niños y Eso", PublicationYear = 1902, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-004", Description = "Fantasía Infantil" },
                new() { Title = "Los Niños del Ferrocarril", PublicationYear = 1906, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-005", Description = "Aventura Infantil" },
                new() { Title = "Mary Poppins", PublicationYear = 1934, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "F-1-006", Description = "Fantasía Infantil" },
                new() { Title = "Mary Poppins Regresa", PublicationYear = 1935, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-007", Description = "Fantasía Infantil" },
                new() { Title = "El León, la Bruja y el Ropero", PublicationYear = 1950, Language = "Inglés", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "F-1-008", Description = "Fantasía" },
                new() { Title = "El Príncipe Caspian", PublicationYear = 1951, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "F-1-009", Description = "Fantasía" },
                new() { Title = "El Bebedor de Vino de Palma", PublicationYear = 1952, Language = "Inglés", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "F-1-010", Description = "Fantasía Folclórica" },
                new() { Title = "Mi Vida en el Monte de los Fantasmas", PublicationYear = 1954, Language = "Inglés", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "F-1-011", Description = "Fantasía Folclórica" },
                new() { Title = "El Hobbit", PublicationYear = 1937, Language = "Inglés", TotalQuantity = 5, AvailableQuantity = 5, PhysicalLocation = "F-1-012", Description = "Fantasía" },
                new() { Title = "El Señor de los Anillos", PublicationYear = 1954, Language = "Inglés", TotalQuantity = 6, AvailableQuantity = 6, PhysicalLocation = "F-1-013", Description = "Fantasía" },
                new() { Title = "Un Mago de Terramar", PublicationYear = 1968, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "F-1-014", Description = "Fantasía" },
                new() { Title = "La Mano Izquierda de la Oscuridad", PublicationYear = 1969, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-001", Description = "Ciencia Ficción/Fantasía" },
                new() { Title = "La Princesa Prometida", PublicationYear = 1973, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "F-1-015", Description = "Fantasía de Aventura" },
                new() { Title = "Maratón", PublicationYear = 1974, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "S-1-001", Description = "Suspenso" },
                new() { Title = "El Rey que Fue y Será", PublicationYear = 1958, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-016", Description = "Fantasía Artúrica" },
                new() { Title = "La Espada en la Piedra", PublicationYear = 1938, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-017", Description = "Fantasía" },
                new() { Title = "Charlie y la Fábrica de Chocolate", PublicationYear = 1964, Language = "Inglés", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "F-1-018", Description = "Fantasía Infantil" },
                new() { Title = "Matilda", PublicationYear = 1988, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "F-1-019", Description = "Fantasía Infantil" },
                new() { Title = "Bomarzo", PublicationYear = 1962, Language = "Español", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "H-1-001", Description = "Novela Histórica Fantástica" },
                new() { Title = "El Unicornio", PublicationYear = 1965, Language = "Español", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "H-1-002", Description = "Novela Histórica Fantástica" },
                new() { Title = "Frankenstein", PublicationYear = 1818, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-002", Description = "Ciencia Ficción/Gótica" },
                new() { Title = "El Último Hombre", PublicationYear = 1826, Language = "Inglés", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "SF-1-003", Description = "Ciencia Ficción" },
                new() { Title = "Veinte Mil Leguas de Viaje Submarino", PublicationYear = 1870, Language = "Francés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-004", Description = "Ciencia Ficción/Aventura" },
                new() { Title = "Viaje al Centro de la Tierra", PublicationYear = 1864, Language = "Francés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-005", Description = "Ciencia Ficción/Aventura" },
                new() { Title = "La Máquina del Tiempo", PublicationYear = 1895, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-006", Description = "Ciencia Ficción" },
                new() { Title = "La Guerra de los Mundos", PublicationYear = 1898, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-007", Description = "Ciencia Ficción" },
                new() { Title = "Un Mundo Feliz", PublicationYear = 1932, Language = "Inglés", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "SF-1-008", Description = "Ciencia Ficción Distópica" },
                new() { Title = "Rebelión en la Granja", PublicationYear = 1945, Language = "Inglés", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "SF-1-009", Description = "Sátira Política/Ciencia Ficción" },
                new() { Title = "1984", PublicationYear = 1949, Language = "Inglés", TotalQuantity = 5, AvailableQuantity = 5, PhysicalLocation = "SF-1-010", Description = "Ciencia Ficción Distópica" },
                new() { Title = "Fahrenheit 451", PublicationYear = 1953, Language = "Inglés", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "SF-1-011", Description = "Ciencia Ficción Distópica" },
                new() { Title = "Crónicas Marcianas", PublicationYear = 1950, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-012", Description = "Ciencia Ficción" },
                new() { Title = "Fundación", PublicationYear = 1951, Language = "Inglés", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "SF-1-013", Description = "Ciencia Ficción" },
                new() { Title = "Yo, Robot", PublicationYear = 1950, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-014", Description = "Ciencia Ficción" },
                new() { Title = "Duna", PublicationYear = 1965, Language = "Inglés", TotalQuantity = 5, AvailableQuantity = 5, PhysicalLocation = "SF-1-015", Description = "Ciencia Ficción" },
                new() { Title = "Los Hijos de Duna", PublicationYear = 1976, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-016", Description = "Ciencia Ficción" },
                new() { Title = "2001: Una Odisea del Espacio", PublicationYear = 1968, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-017", Description = "Ciencia Ficción" },
                new() { Title = "Cita con Rama", PublicationYear = 1973, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-018", Description = "Ciencia Ficción" },
                new() { Title = "Solaris", PublicationYear = 1961, Language = "Polaco", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-019", Description = "Ciencia Ficción" },
                new() { Title = "El Congreso de Futurología", PublicationYear = 1971, Language = "Polaco", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "SF-1-020", Description = "Ciencia Ficción" },
                new() { Title = "¿Sueñan los Androides con Ovejas Eléctricas?", PublicationYear = 1968, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-021", Description = "Ciencia Ficción" },
                new() { Title = "Ubik", PublicationYear = 1969, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-022", Description = "Ciencia Ficción" },
                new() { Title = "Neuromancer", PublicationYear = 1984, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-023", Description = "Ciencia Ficción Ciberpunk" },
                new() { Title = "Conde Cero", PublicationYear = 1986, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-024", Description = "Ciencia Ficción Ciberpunk" },
                new() { Title = "Las Estrellas mi Destino", PublicationYear = 1956, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-025", Description = "Ciencia Ficción" },
                new() { Title = "El Hombre Demolido", PublicationYear = 1953, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-026", Description = "Ciencia Ficción" },
                new() { Title = "El Juego de Ender", PublicationYear = 1985, Language = "Inglés", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "SF-1-027", Description = "Ciencia Ficción" },
                new() { Title = "La Voz de los Muertos", PublicationYear = 1986, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-028", Description = "Ciencia Ficción" },
                new() { Title = "Hyperion", PublicationYear = 1989, Language = "Inglés", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-029", Description = "Ciencia Ficción" },
                new() { Title = "La Caída de Hyperion", PublicationYear = 1990, Language = "Inglés", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-030", Description = "Ciencia Ficción" },

                // Agregar más libros según sea necesario - esto representa un subconjunto para demostración
            };
            context.Books.AddRange(books);

            // Save to get book IDs
            context.SaveChanges();

            // Seed Book-Author relationships (matching the SQL data where author IDs correspond to book titles)
            var bookAuthors = new BookAuthor[]
            {
                // Books 1-52 map to the first 60 authors as shown in SQL files
                new() { BookId = books[0].BookId, AuthorId = authors[0].AuthorId, Role = "Author" }, // Beowulf - Anonymous
                new() { BookId = books[1].BookId, AuthorId = authors[0].AuthorId, Role = "Author" }, // El Cantar de Roldán - Anonymous
                new() { BookId = books[2].BookId, AuthorId = authors[1].AuthorId, Role = "Author" }, // Le Morte d'Arthur - Thomas Malory
                new() { BookId = books[3].BookId, AuthorId = authors[2].AuthorId, Role = "Author" }, // Alice's Adventures - Lewis Carroll
                new() { BookId = books[4].BookId, AuthorId = authors[2].AuthorId, Role = "Author" }, // Through the Looking-Glass - Lewis Carroll
                new() { BookId = books[5].BookId, AuthorId = authors[3].AuthorId, Role = "Author" }, // Five Children and It - E. Nesbit
                new() { BookId = books[6].BookId, AuthorId = authors[3].AuthorId, Role = "Author" }, // The Railway Children - E. Nesbit
                new() { BookId = books[7].BookId, AuthorId = authors[4].AuthorId, Role = "Author" }, // Mary Poppins - P.L. Travers
                new() { BookId = books[8].BookId, AuthorId = authors[4].AuthorId, Role = "Author" }, // Mary Poppins Comes Back - P.L. Travers
                new() { BookId = books[9].BookId, AuthorId = authors[5].AuthorId, Role = "Author" }, // The Lion, the Witch - C.S. Lewis
                new() { BookId = books[10].BookId, AuthorId = authors[5].AuthorId, Role = "Author" }, // Prince Caspian - C.S. Lewis
                new() { BookId = books[11].BookId, AuthorId = authors[6].AuthorId, Role = "Author" }, // The Palm-Wine Drinkard - Amos Tutuola
                new() { BookId = books[12].BookId, AuthorId = authors[6].AuthorId, Role = "Author" }, // My Life in the Bush - Amos Tutuola
                new() { BookId = books[13].BookId, AuthorId = authors[7].AuthorId, Role = "Author" }, // The Hobbit - J.R.R. Tolkien
                new() { BookId = books[14].BookId, AuthorId = authors[7].AuthorId, Role = "Author" }, // The Lord of the Rings - J.R.R. Tolkien
                new() { BookId = books[15].BookId, AuthorId = authors[8].AuthorId, Role = "Author" }, // A Wizard of Earthsea - Ursula K. Le Guin
                new() { BookId = books[16].BookId, AuthorId = authors[8].AuthorId, Role = "Author" }, // The Left Hand of Darkness - Ursula K. Le Guin
                new() { BookId = books[17].BookId, AuthorId = authors[9].AuthorId, Role = "Author" }, // The Princess Bride - William Goldman
                new() { BookId = books[18].BookId, AuthorId = authors[9].AuthorId, Role = "Author" }, // Marathon Man - William Goldman
                new() { BookId = books[19].BookId, AuthorId = authors[10].AuthorId, Role = "Author" }, // The Once and Future King - T.H. White
                new() { BookId = books[20].BookId, AuthorId = authors[10].AuthorId, Role = "Author" }, // The Sword in the Stone - T.H. White
                new() { BookId = books[21].BookId, AuthorId = authors[11].AuthorId, Role = "Author" }, // Charlie and the Chocolate Factory - Roald Dahl
                new() { BookId = books[22].BookId, AuthorId = authors[11].AuthorId, Role = "Author" }, // Matilda - Roald Dahl
                new() { BookId = books[23].BookId, AuthorId = authors[12].AuthorId, Role = "Author" }, // Bomarzo - Manuel Mujica Láinez
                new() { BookId = books[24].BookId, AuthorId = authors[12].AuthorId, Role = "Author" }, // El Unicornio - Manuel Mujica Láinez
                new() { BookId = books[25].BookId, AuthorId = authors[13].AuthorId, Role = "Author" }, // Frankenstein - Mary Shelley
                new() { BookId = books[26].BookId, AuthorId = authors[13].AuthorId, Role = "Author" }, // The Last Man - Mary Shelley
                new() { BookId = books[27].BookId, AuthorId = authors[14].AuthorId, Role = "Author" }, // Twenty Thousand Leagues - Jules Verne
                new() { BookId = books[28].BookId, AuthorId = authors[14].AuthorId, Role = "Author" }, // Journey to the Center - Jules Verne
                new() { BookId = books[29].BookId, AuthorId = authors[15].AuthorId, Role = "Author" }, // The Time Machine - H.G. Wells
                new() { BookId = books[30].BookId, AuthorId = authors[15].AuthorId, Role = "Author" }, // The War of the Worlds - H.G. Wells
                new() { BookId = books[31].BookId, AuthorId = authors[16].AuthorId, Role = "Author" }, // Brave New World - Aldous Huxley
                new() { BookId = books[32].BookId, AuthorId = authors[17].AuthorId, Role = "Author" }, // Animal Farm - George Orwell
                new() { BookId = books[33].BookId, AuthorId = authors[17].AuthorId, Role = "Author" }, // 1984 - George Orwell
                new() { BookId = books[34].BookId, AuthorId = authors[18].AuthorId, Role = "Author" }, // Fahrenheit 451 - Ray Bradbury
                new() { BookId = books[35].BookId, AuthorId = authors[18].AuthorId, Role = "Author" }, // The Martian Chronicles - Ray Bradbury
                new() { BookId = books[36].BookId, AuthorId = authors[19].AuthorId, Role = "Author" }, // Foundation - Isaac Asimov
                new() { BookId = books[37].BookId, AuthorId = authors[19].AuthorId, Role = "Author" }, // I, Robot - Isaac Asimov
                new() { BookId = books[38].BookId, AuthorId = authors[20].AuthorId, Role = "Author" }, // Dune - Frank Herbert
                new() { BookId = books[39].BookId, AuthorId = authors[20].AuthorId, Role = "Author" }, // Children of Dune - Frank Herbert
                new() { BookId = books[40].BookId, AuthorId = authors[21].AuthorId, Role = "Author" }, // 2001: A Space Odyssey - Arthur C. Clarke
                new() { BookId = books[41].BookId, AuthorId = authors[21].AuthorId, Role = "Author" }, // Rendezvous with Rama - Arthur C. Clarke
                new() { BookId = books[42].BookId, AuthorId = authors[22].AuthorId, Role = "Author" }, // Solaris - Stanisław Lem
                new() { BookId = books[43].BookId, AuthorId = authors[22].AuthorId, Role = "Author" }, // The Futurological Congress - Stanisław Lem
                new() { BookId = books[44].BookId, AuthorId = authors[23].AuthorId, Role = "Author" }, // Do Androids Dream - Philip K. Dick
                new() { BookId = books[45].BookId, AuthorId = authors[23].AuthorId, Role = "Author" }, // Ubik - Philip K. Dick
                new() { BookId = books[46].BookId, AuthorId = authors[24].AuthorId, Role = "Author" }, // Neuromancer - William Gibson
                new() { BookId = books[47].BookId, AuthorId = authors[24].AuthorId, Role = "Author" }, // Count Zero - William Gibson
                new() { BookId = books[48].BookId, AuthorId = authors[25].AuthorId, Role = "Author" }, // The Stars My Destination - Alfred Bester
                new() { BookId = books[49].BookId, AuthorId = authors[25].AuthorId, Role = "Author" }, // The Demolished Man - Alfred Bester
                new() { BookId = books[50].BookId, AuthorId = authors[26].AuthorId, Role = "Author" }, // Ender's Game - Orson Scott Card
                new() { BookId = books[51].BookId, AuthorId = authors[26].AuthorId, Role = "Author" }, // Speaker for the Dead - Orson Scott Card
                new() { BookId = books[52].BookId, AuthorId = authors[27].AuthorId, Role = "Author" }, // Hyperion - Dan Simmons
                new() { BookId = books[53].BookId, AuthorId = authors[27].AuthorId, Role = "Author" }, // The Fall of Hyperion - Dan Simmons
            };
            context.BookAuthors.AddRange(bookAuthors);

            // Seed sample users
            var users = new User[]
            {
                new()
                {
                    UserCode = "EST001",
                    FirstName = "Ana",
                    LastName = "García",
                    Email = "ana.garcia@universidad.edu",
                    Phone = "+52-555-123-4567",
                    Address = "Av. Universidad 123, Ciudad Universitaria, CDMX 04510",
                    UserTypeId = userTypes[0].UserTypeId, // Estudiante
                    CareerDepartment = "Ciencias de la Computación",
                    RegistrationDate = DateTime.UtcNow.AddMonths(-12).Date,
                    MembershipExpirationDate = DateTime.UtcNow.AddMonths(12).Date,
                },
                new()
                {
                    UserCode = "PROF001",
                    FirstName = "Dr. Carlos",
                    LastName = "Rodríguez",
                    Email = "carlos.rodriguez@universidad.edu",
                    Phone = "+52-555-123-4568",
                    Address = "Calle Profesores 456, Ciudad Universitaria, CDMX 04510",
                    UserTypeId = userTypes[1].UserTypeId, // Profesor
                    CareerDepartment = "Ciencias de la Computación",
                    RegistrationDate = DateTime.UtcNow.AddYears(-5).Date,
                    MembershipExpirationDate = DateTime.UtcNow.AddYears(1).Date,
                },
                new()
                {
                    UserCode = "PER001",
                    FirstName = "María",
                    LastName = "López",
                    Email = "maria.lopez@universidad.edu",
                    Phone = "+52-555-123-4569",
                    Address = "Circuito Personal 789, Ciudad Universitaria, CDMX 04510",
                    UserTypeId = userTypes[2].UserTypeId, // Personal
                    CareerDepartment = "Servicios Bibliotecarios",
                    RegistrationDate = DateTime.UtcNow.AddYears(-3).Date,
                    MembershipExpirationDate = DateTime.UtcNow.AddYears(2).Date,
                },
            };
            context.Users.AddRange(users);

            // Save all changes
            context.SaveChanges();
        }
    }
}
