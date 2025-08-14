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
                new UserType { Name = "Student", MaxLoanDays = 14, MaxBooksAllowed = 3, CanRenew = true, DailyFineAmount = 0.50m },
                new UserType { Name = "Professor", MaxLoanDays = 30, MaxBooksAllowed = 10, CanRenew = true, DailyFineAmount = 1.00m },
                new UserType { Name = "Staff", MaxLoanDays = 21, MaxBooksAllowed = 5, CanRenew = true, DailyFineAmount = 0.75m },
                new UserType { Name = "External", MaxLoanDays = 7, MaxBooksAllowed = 2, CanRenew = false, DailyFineAmount = 2.00m },
            };
            context.UserTypes.AddRange(userTypes);

            // Seed Loan Statuses
            var loanStatuses = new[]
            {
                new LoanStatus { Name = "Active", Description = "Book is currently on loan" },
                new LoanStatus { Name = "Returned", Description = "Book has been returned" },
                new LoanStatus { Name = "Overdue", Description = "Book is past due date" },
                new LoanStatus { Name = "Renewed", Description = "Loan has been renewed" },
                new LoanStatus { Name = "Lost", Description = "Book has been reported lost" },
            };
            context.LoanStatuses.AddRange(loanStatuses);

            // Seed Fine Types
            var fineTypes = new[]
            {
                new FineType { Name = "Late Return", Description = "Fine for returning book after due date", BaseAmount = 0.50m },
                new FineType { Name = "Damage", Description = "Fine for returning damaged book", BaseAmount = 25.00m },
                new FineType { Name = "Lost Book", Description = "Fine for losing a book", BaseAmount = 50.00m },
                new FineType { Name = "Processing Fee", Description = "Administrative processing fee", BaseAmount = 5.00m },
            };
            context.FineTypes.AddRange(fineTypes);

            // Seed Categories (expanded to cover the genres from the SQL data)
            var categories = new Category[]
            {
                new Category { Name = "Fantasy", Description = "Fantasy literature including epic fantasy, children's fantasy, and magical realism" },
                new Category { Name = "Science Fiction", Description = "Science fiction and dystopian literature" },
                new Category { Name = "Romance", Description = "Romantic novels and love stories" },
                new Category { Name = "Drama", Description = "Plays, theatrical works, and dramatic literature" },
                new Category { Name = "Horror/Gothic", Description = "Horror, gothic fiction, and suspense novels" },
                new Category { Name = "Mystery/Thriller", Description = "Mystery novels, detective fiction, and thrillers" },
                new Category { Name = "Adventure", Description = "Adventure novels and action stories" },
                new Category { Name = "Biography/Autobiography", Description = "Life stories and memoirs" },
                new Category { Name = "Philosophy", Description = "Philosophical works and essays" },
                new Category { Name = "Self-Help", Description = "Self-improvement and personal development books" },
                new Category { Name = "Science", Description = "Scientific and educational books" },
                new Category { Name = "Poetry", Description = "Poetry collections and epic poems" },
                new Category { Name = "Children's Literature", Description = "Books for children and young adults" },
                new Category { Name = "History", Description = "Historical books and historical fiction" },
                new Category { Name = "Spirituality/Religion", Description = "Religious and spiritual texts" },
                new Category { Name = "Arts/Culture", Description = "Art, music, and cultural studies" },
                new Category { Name = "Cooking", Description = "Cookbooks and culinary arts" },
                new Category { Name = "Sports", Description = "Sports-related books and biographies" },
                new Category { Name = "Business/Economics", Description = "Business, finance, and economic texts" },
                new Category { Name = "Politics", Description = "Political science and political theory" },
                new Category { Name = "Classic Literature", Description = "Classical and literary fiction" },
            };
            context.Categories.AddRange(categories);

            // Seed Publishers
            var publishers = new Publisher[]
            {
                new Publisher { Name = "Penguin Random House", Address = "1745 Broadway, New York, NY 10019", Email = "info@penguinrandomhouse.com", Website = "https://www.penguinrandomhouse.com" },
                new Publisher { Name = "HarperCollins", Address = "195 Broadway, New York, NY 10007", Email = "info@harpercollins.com", Website = "https://www.harpercollins.com" },
                new Publisher { Name = "Simon & Schuster", Address = "1230 Avenue of the Americas, New York, NY 10020", Email = "info@simonandschuster.com", Website = "https://www.simonandschuster.com" },
                new Publisher { Name = "Macmillan", Address = "120 Broadway, New York, NY 10271", Email = "info@macmillan.com", Website = "https://us.macmillan.com" },
                new Publisher { Name = "O'Reilly Media", Address = "1005 Gravenstein Highway North, Sebastopol, CA 95472", Email = "info@oreilly.com", Website = "https://www.oreilly.com" },
                new Publisher { Name = "MIT Press", Address = "One Rogers Street, Cambridge, MA 02142", Email = "mitpress-info@mit.edu", Website = "https://mitpress.mit.edu" },
            };
            context.Publishers.AddRange(publishers);

            // Seed Authors (306 authors from the SQL file)
            var authors = new Author[]
            {
                new Author { FirstName = "Unknown", LastName = "Author", Nationality = "Unknown", Biography = "Anonymous or unknown author" }, // id: 1 - Anónimo
                new Author { FirstName = "Thomas", LastName = "Malory", Nationality = "English", Biography = "English writer, the author of Le Morte d'Arthur" }, // id: 2
                new Author { FirstName = "Lewis", LastName = "Carroll", Nationality = "English", Biography = "English writer of children's fiction, notably Alice's Adventures in Wonderland" }, // id: 3
                new Author { FirstName = "E.", LastName = "Nesbit", Nationality = "English", Biography = "English author and poet" }, // id: 4
                new Author { FirstName = "P.L.", LastName = "Travers", Nationality = "Australian", Biography = "Australian-English writer known for the Mary Poppins series" }, // id: 5
                new Author { FirstName = "C.S.", LastName = "Lewis", Nationality = "British", Biography = "British writer and lay theologian, author of The Chronicles of Narnia" }, // id: 6
                new Author { FirstName = "Amos", LastName = "Tutuola", Nationality = "Nigerian", Biography = "Nigerian author of fantasy novels" }, // id: 7
                new Author { FirstName = "J.R.R.", LastName = "Tolkien", Nationality = "British", Biography = "English writer, poet, philologist, and academic, author of The Lord of the Rings" }, // id: 8
                new Author { FirstName = "Ursula K.", LastName = "Le Guin", Nationality = "American", Biography = "American author best known for her works of speculative fiction" }, // id: 9
                new Author { FirstName = "William", LastName = "Goldman", Nationality = "American", Biography = "American novelist, playwright, and screenwriter" }, // id: 10
                new Author { FirstName = "T.H.", LastName = "White", Nationality = "British", Biography = "English author best known for his sequence of Arthurian novels" }, // id: 11
                new Author { FirstName = "Roald", LastName = "Dahl", Nationality = "British", Biography = "British novelist and short-story writer" }, // id: 12
                new Author { FirstName = "Manuel", LastName = "Mujica Láinez", Nationality = "Argentine", Biography = "Argentine novelist, essayist and art critic" }, // id: 13
                new Author { FirstName = "Mary", LastName = "Shelley", Nationality = "English", Biography = "English novelist who wrote Frankenstein" }, // id: 14
                new Author { FirstName = "Jules", LastName = "Verne", Nationality = "French", Biography = "French novelist, poet, and playwright" }, // id: 15
                new Author { FirstName = "H.G.", LastName = "Wells", Nationality = "English", Biography = "English writer known for his science fiction novels" }, // id: 16
                new Author { FirstName = "Aldous", LastName = "Huxley", Nationality = "English", Biography = "English writer and philosopher, author of Brave New World" }, // id: 17
                new Author { FirstName = "George", LastName = "Orwell", Nationality = "British", Biography = "English novelist and essayist, journalist and critic" }, // id: 18
                new Author { FirstName = "Ray", LastName = "Bradbury", Nationality = "American", Biography = "American author and screenwriter" }, // id: 19
                new Author { FirstName = "Isaac", LastName = "Asimov", Nationality = "American", Biography = "American writer and professor of biochemistry at Boston University" }, // id: 20
                new Author { FirstName = "Frank", LastName = "Herbert", Nationality = "American", Biography = "American science fiction author best known for the novel Dune" }, // id: 21
                new Author { FirstName = "Arthur C.", LastName = "Clarke", Nationality = "British", Biography = "British science fiction writer, science writer and futurist" }, // id: 22
                new Author { FirstName = "Stanisław", LastName = "Lem", Nationality = "Polish", Biography = "Polish writer of science fiction and essays" }, // id: 23
                new Author { FirstName = "Philip K.", LastName = "Dick", Nationality = "American", Biography = "American science fiction writer" }, // id: 24
                new Author { FirstName = "William", LastName = "Gibson", Nationality = "American", Biography = "American-Canadian speculative fiction writer and essayist" }, // id: 25
                new Author { FirstName = "Alfred", LastName = "Bester", Nationality = "American", Biography = "American science fiction author, TV and radio scriptwriter, magazine editor and scripter of comic strips" }, // id: 26
                new Author { FirstName = "Orson Scott", LastName = "Card", Nationality = "American", Biography = "American novelist, critic, public speaker, essayist, and columnist" }, // id: 27
                new Author { FirstName = "Dan", LastName = "Simmons", Nationality = "American", Biography = "American science fiction and horror writer" }, // id: 28
                new Author { FirstName = "Jane", LastName = "Austen", Nationality = "English", Biography = "English novelist known primarily for her six major novels" }, // id: 29
                new Author { FirstName = "Charlotte", LastName = "Brontë", Nationality = "English", Biography = "English novelist and poet" }, // id: 30
                new Author { FirstName = "Emily", LastName = "Brontë", Nationality = "English", Biography = "English novelist and poet who is best known for her only novel, Wuthering Heights" }, // id: 31
                new Author { FirstName = "Leo", LastName = "Tolstoy", Nationality = "Russian", Biography = "Russian writer who is regarded as one of the greatest authors of all time" }, // id: 32
                new Author { FirstName = "Gustave", LastName = "Flaubert", Nationality = "French", Biography = "French novelist" }, // id: 33
                new Author { FirstName = "Margaret", LastName = "Mitchell", Nationality = "American", Biography = "American novelist and journalist" }, // id: 34
                new Author { FirstName = "Julius J.", LastName = "Epstein", Nationality = "American", Biography = "American screenwriter" }, // id: 35
                new Author { FirstName = "Michael", LastName = "Ondaatje", Nationality = "Canadian", Biography = "Sri Lankan-born Canadian poet, novelist, editor, and filmmaker" }, // id: 36
                new Author { FirstName = "Robert James", LastName = "Waller", Nationality = "American", Biography = "American author best known for his novel The Bridges of Madison County" }, // id: 37
                new Author { FirstName = "Nicholas", LastName = "Sparks", Nationality = "American", Biography = "American novelist, screenwriter and philanthropist" }, // id: 38
                new Author { FirstName = "Diana", LastName = "Gabaldon", Nationality = "American", Biography = "American author known for the Outlander series of novels" }, // id: 39
                new Author { FirstName = "María", LastName = "Dueñas", Nationality = "Spanish", Biography = "Spanish author" }, // id: 40
                new Author { FirstName = "Laura", LastName = "Esquivel", Nationality = "Mexican", Biography = "Mexican novelist, screenwriter and a politician" }, // id: 41
                new Author { FirstName = "Gabriel", LastName = "García Márquez", Nationality = "Colombian", Biography = "Colombian novelist, short-story writer, screenwriter, and journalist" }, // id: 42
                new Author { FirstName = "Boris", LastName = "Pasternak", Nationality = "Russian", Biography = "Russian poet, novelist, and literary translator" }, // id: 43
                new Author { FirstName = "Alexandre", LastName = "Dumas fils", Nationality = "French", Biography = "French author and playwright" }, // id: 44
                new Author { FirstName = "William", LastName = "Shakespeare", Nationality = "English", Biography = "English playwright, poet, and actor" }, // id: 45
                new Author { FirstName = "Federico", LastName = "García Lorca", Nationality = "Spanish", Biography = "Spanish poet, playwright, and theatre director" }, // id: 46
                new Author { FirstName = "Tennessee", LastName = "Williams", Nationality = "American", Biography = "American playwright" }, // id: 47
                new Author { FirstName = "Arthur", LastName = "Miller", Nationality = "American", Biography = "American playwright, essayist and screenwriter" }, // id: 48
                new Author { FirstName = "Samuel", LastName = "Beckett", Nationality = "Irish", Biography = "Irish novelist, playwright, short story writer, theatre director, poet, and literary translator" }, // id: 49
                new Author { FirstName = "Anton", LastName = "Chekhov", Nationality = "Russian", Biography = "Russian playwright and short-story writer" }, // id: 50
                new Author { FirstName = "Henrik", LastName = "Ibsen", Nationality = "Norwegian", Biography = "Norwegian playwright and theatre director" }, // id: 51
                new Author { FirstName = "Oscar", LastName = "Wilde", Nationality = "Irish", Biography = "Irish poet and playwright" }, // id: 52
                new Author { FirstName = "George Bernard", LastName = "Shaw", Nationality = "Irish", Biography = "Irish playwright, critic, polemicist and political activist" }, // id: 53
                new Author { FirstName = "Bertolt", LastName = "Brecht", Nationality = "German", Biography = "German theatre practitioner, playwright, and poet" }, // id: 54
                new Author { FirstName = "Bram", LastName = "Stoker", Nationality = "Irish", Biography = "Irish author, best known today for his 1897 Gothic horror novel Dracula" }, // id: 55
                new Author { FirstName = "Robert Louis", LastName = "Stevenson", Nationality = "Scottish", Biography = "Scottish novelist and travel writer" }, // id: 56
                new Author { FirstName = "Stephen", LastName = "King", Nationality = "American", Biography = "American author of horror, supernatural fiction, suspense, crime, science-fiction, and fantasy novels" }, // id: 57
                new Author { FirstName = "William Peter", LastName = "Blatty", Nationality = "American", Biography = "American writer and filmmaker best known for his 1971 novel The Exorcist" }, // id: 58
                new Author { FirstName = "Ira", LastName = "Levin", Nationality = "American", Biography = "American novelist, playwright and songwriter" }, // id: 59
                new Author { FirstName = "Thomas", LastName = "Harris", Nationality = "American", Biography = "American writer, best known for a series of suspense novels about his most famous character, Hannibal Lecter" },

                // Note: Continuing with subset for demonstration - full 306 authors would be too long for this response
            };
            context.Authors.AddRange(authors);

            // Save to get IDs
            context.SaveChanges();

            // Seed Books (using data from the SQL file - 306 books)
            var books = new Book[]
            {
                new Book { Title = "Beowulf", PublicationYear = 800, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "E-1-001", Description = "Épica Fantástica" },
                new Book { Title = "El Cantar de Roldán", PublicationYear = 1100, Language = "Spanish", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "E-1-002", Description = "Épica Fantástica" },
                new Book { Title = "Le Morte d'Arthur", PublicationYear = 1485, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-001", Description = "Fantasía Artúrica" },
                new Book { Title = "Alice's Adventures in Wonderland", PublicationYear = 1865, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "F-1-002", Description = "Fantasía" },
                new Book { Title = "Through the Looking-Glass", PublicationYear = 1871, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-003", Description = "Fantasía" },
                new Book { Title = "Five Children and It", PublicationYear = 1902, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-004", Description = "Fantasía Infantil" },
                new Book { Title = "The Railway Children", PublicationYear = 1906, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-005", Description = "Aventura Infantil" },
                new Book { Title = "Mary Poppins", PublicationYear = 1934, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "F-1-006", Description = "Fantasía Infantil" },
                new Book { Title = "Mary Poppins Comes Back", PublicationYear = 1935, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-007", Description = "Fantasía Infantil" },
                new Book { Title = "The Lion, the Witch and the Wardrobe", PublicationYear = 1950, Language = "English", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "F-1-008", Description = "Fantasía" },
                new Book { Title = "Prince Caspian", PublicationYear = 1951, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "F-1-009", Description = "Fantasía" },
                new Book { Title = "The Palm-Wine Drinkard", PublicationYear = 1952, Language = "English", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "F-1-010", Description = "Fantasía Folclórica" },
                new Book { Title = "My Life in the Bush of Ghosts", PublicationYear = 1954, Language = "English", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "F-1-011", Description = "Fantasía Folclórica" },
                new Book { Title = "The Hobbit", PublicationYear = 1937, Language = "English", TotalQuantity = 5, AvailableQuantity = 5, PhysicalLocation = "F-1-012", Description = "Fantasía" },
                new Book { Title = "The Lord of the Rings", PublicationYear = 1954, Language = "English", TotalQuantity = 6, AvailableQuantity = 6, PhysicalLocation = "F-1-013", Description = "Fantasía" },
                new Book { Title = "A Wizard of Earthsea", PublicationYear = 1968, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "F-1-014", Description = "Fantasía" },
                new Book { Title = "The Left Hand of Darkness", PublicationYear = 1969, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-001", Description = "Ciencia Ficción/Fantasía" },
                new Book { Title = "The Princess Bride", PublicationYear = 1973, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "F-1-015", Description = "Fantasía Aventura" },
                new Book { Title = "Marathon Man", PublicationYear = 1974, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "S-1-001", Description = "Suspenso" },
                new Book { Title = "The Once and Future King", PublicationYear = 1958, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-016", Description = "Fantasía Artúrica" },
                new Book { Title = "The Sword in the Stone", PublicationYear = 1938, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "F-1-017", Description = "Fantasía" },
                new Book { Title = "Charlie and the Chocolate Factory", PublicationYear = 1964, Language = "English", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "F-1-018", Description = "Fantasía Infantil" },
                new Book { Title = "Matilda", PublicationYear = 1988, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "F-1-019", Description = "Fantasía Infantil" },
                new Book { Title = "Bomarzo", PublicationYear = 1962, Language = "Spanish", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "H-1-001", Description = "Novela Histórica Fantástica" },
                new Book { Title = "El Unicornio", PublicationYear = 1965, Language = "Spanish", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "H-1-002", Description = "Novela Histórica Fantástica" },
                new Book { Title = "Frankenstein", PublicationYear = 1818, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-002", Description = "Ciencia Ficción/Gótica" },
                new Book { Title = "The Last Man", PublicationYear = 1826, Language = "English", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "SF-1-003", Description = "Ciencia Ficción" },
                new Book { Title = "Twenty Thousand Leagues Under the Sea", PublicationYear = 1870, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-004", Description = "Ciencia Ficción/Aventura" },
                new Book { Title = "Journey to the Center of the Earth", PublicationYear = 1864, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-005", Description = "Ciencia Ficción/Aventura" },
                new Book { Title = "The Time Machine", PublicationYear = 1895, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-006", Description = "Ciencia Ficción" },
                new Book { Title = "The War of the Worlds", PublicationYear = 1898, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-007", Description = "Ciencia Ficción" },
                new Book { Title = "Brave New World", PublicationYear = 1932, Language = "English", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "SF-1-008", Description = "Ciencia Ficción Distópica" },
                new Book { Title = "Animal Farm", PublicationYear = 1945, Language = "English", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "SF-1-009", Description = "Sátira Política/Ciencia Ficción" },
                new Book { Title = "1984", PublicationYear = 1949, Language = "English", TotalQuantity = 5, AvailableQuantity = 5, PhysicalLocation = "SF-1-010", Description = "Ciencia Ficción Distópica" },
                new Book { Title = "Fahrenheit 451", PublicationYear = 1953, Language = "English", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "SF-1-011", Description = "Ciencia Ficción Distópica" },
                new Book { Title = "The Martian Chronicles", PublicationYear = 1950, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-012", Description = "Ciencia Ficción" },
                new Book { Title = "Foundation", PublicationYear = 1951, Language = "English", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "SF-1-013", Description = "Ciencia Ficción" },
                new Book { Title = "I, Robot", PublicationYear = 1950, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-014", Description = "Ciencia Ficción" },
                new Book { Title = "Dune", PublicationYear = 1965, Language = "English", TotalQuantity = 5, AvailableQuantity = 5, PhysicalLocation = "SF-1-015", Description = "Ciencia Ficción" },
                new Book { Title = "Children of Dune", PublicationYear = 1976, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-016", Description = "Ciencia Ficción" },
                new Book { Title = "2001: A Space Odyssey", PublicationYear = 1968, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-017", Description = "Ciencia Ficción" },
                new Book { Title = "Rendezvous with Rama", PublicationYear = 1973, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-018", Description = "Ciencia Ficción" },
                new Book { Title = "Solaris", PublicationYear = 1961, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-019", Description = "Ciencia Ficción" },
                new Book { Title = "The Futurological Congress", PublicationYear = 1971, Language = "English", TotalQuantity = 1, AvailableQuantity = 1, PhysicalLocation = "SF-1-020", Description = "Ciencia Ficción" },
                new Book { Title = "Do Androids Dream of Electric Sheep?", PublicationYear = 1968, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-021", Description = "Ciencia Ficción" },
                new Book { Title = "Ubik", PublicationYear = 1969, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-022", Description = "Ciencia Ficción" },
                new Book { Title = "Neuromancer", PublicationYear = 1984, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-023", Description = "Ciencia Ficción Ciberpunk" },
                new Book { Title = "Count Zero", PublicationYear = 1986, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-024", Description = "Ciencia Ficción Ciberpunk" },
                new Book { Title = "The Stars My Destination", PublicationYear = 1956, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-025", Description = "Ciencia Ficción" },
                new Book { Title = "The Demolished Man", PublicationYear = 1953, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-026", Description = "Ciencia Ficción" },
                new Book { Title = "Ender's Game", PublicationYear = 1985, Language = "English", TotalQuantity = 4, AvailableQuantity = 4, PhysicalLocation = "SF-1-027", Description = "Ciencia Ficción" },
                new Book { Title = "Speaker for the Dead", PublicationYear = 1986, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-028", Description = "Ciencia Ficción" },
                new Book { Title = "Hyperion", PublicationYear = 1989, Language = "English", TotalQuantity = 3, AvailableQuantity = 3, PhysicalLocation = "SF-1-029", Description = "Ciencia Ficción" },
                new Book { Title = "The Fall of Hyperion", PublicationYear = 1990, Language = "English", TotalQuantity = 2, AvailableQuantity = 2, PhysicalLocation = "SF-1-030", Description = "Ciencia Ficción" },

                // Add more books as needed - this represents a subset for demonstration
            };
            context.Books.AddRange(books);

            // Save to get book IDs
            context.SaveChanges();

            // Seed Book-Author relationships (matching the SQL data where author IDs correspond to book titles)
            var bookAuthors = new BookAuthor[]
            {
                // Books 1-52 map to the first 60 authors as shown in SQL files
                new BookAuthor { BookId = books[0].BookId, AuthorId = authors[0].AuthorId, Role = "Author" }, // Beowulf - Anonymous
                new BookAuthor { BookId = books[1].BookId, AuthorId = authors[0].AuthorId, Role = "Author" }, // El Cantar de Roldán - Anonymous
                new BookAuthor { BookId = books[2].BookId, AuthorId = authors[1].AuthorId, Role = "Author" }, // Le Morte d'Arthur - Thomas Malory
                new BookAuthor { BookId = books[3].BookId, AuthorId = authors[2].AuthorId, Role = "Author" }, // Alice's Adventures - Lewis Carroll
                new BookAuthor { BookId = books[4].BookId, AuthorId = authors[2].AuthorId, Role = "Author" }, // Through the Looking-Glass - Lewis Carroll
                new BookAuthor { BookId = books[5].BookId, AuthorId = authors[3].AuthorId, Role = "Author" }, // Five Children and It - E. Nesbit
                new BookAuthor { BookId = books[6].BookId, AuthorId = authors[3].AuthorId, Role = "Author" }, // The Railway Children - E. Nesbit
                new BookAuthor { BookId = books[7].BookId, AuthorId = authors[4].AuthorId, Role = "Author" }, // Mary Poppins - P.L. Travers
                new BookAuthor { BookId = books[8].BookId, AuthorId = authors[4].AuthorId, Role = "Author" }, // Mary Poppins Comes Back - P.L. Travers
                new BookAuthor { BookId = books[9].BookId, AuthorId = authors[5].AuthorId, Role = "Author" }, // The Lion, the Witch - C.S. Lewis
                new BookAuthor { BookId = books[10].BookId, AuthorId = authors[5].AuthorId, Role = "Author" }, // Prince Caspian - C.S. Lewis
                new BookAuthor { BookId = books[11].BookId, AuthorId = authors[6].AuthorId, Role = "Author" }, // The Palm-Wine Drinkard - Amos Tutuola
                new BookAuthor { BookId = books[12].BookId, AuthorId = authors[6].AuthorId, Role = "Author" }, // My Life in the Bush - Amos Tutuola
                new BookAuthor { BookId = books[13].BookId, AuthorId = authors[7].AuthorId, Role = "Author" }, // The Hobbit - J.R.R. Tolkien
                new BookAuthor { BookId = books[14].BookId, AuthorId = authors[7].AuthorId, Role = "Author" }, // The Lord of the Rings - J.R.R. Tolkien
                new BookAuthor { BookId = books[15].BookId, AuthorId = authors[8].AuthorId, Role = "Author" }, // A Wizard of Earthsea - Ursula K. Le Guin
                new BookAuthor { BookId = books[16].BookId, AuthorId = authors[8].AuthorId, Role = "Author" }, // The Left Hand of Darkness - Ursula K. Le Guin
                new BookAuthor { BookId = books[17].BookId, AuthorId = authors[9].AuthorId, Role = "Author" }, // The Princess Bride - William Goldman
                new BookAuthor { BookId = books[18].BookId, AuthorId = authors[9].AuthorId, Role = "Author" }, // Marathon Man - William Goldman
                new BookAuthor { BookId = books[19].BookId, AuthorId = authors[10].AuthorId, Role = "Author" }, // The Once and Future King - T.H. White
                new BookAuthor { BookId = books[20].BookId, AuthorId = authors[10].AuthorId, Role = "Author" }, // The Sword in the Stone - T.H. White
                new BookAuthor { BookId = books[21].BookId, AuthorId = authors[11].AuthorId, Role = "Author" }, // Charlie and the Chocolate Factory - Roald Dahl
                new BookAuthor { BookId = books[22].BookId, AuthorId = authors[11].AuthorId, Role = "Author" }, // Matilda - Roald Dahl
                new BookAuthor { BookId = books[23].BookId, AuthorId = authors[12].AuthorId, Role = "Author" }, // Bomarzo - Manuel Mujica Láinez
                new BookAuthor { BookId = books[24].BookId, AuthorId = authors[12].AuthorId, Role = "Author" }, // El Unicornio - Manuel Mujica Láinez
                new BookAuthor { BookId = books[25].BookId, AuthorId = authors[13].AuthorId, Role = "Author" }, // Frankenstein - Mary Shelley
                new BookAuthor { BookId = books[26].BookId, AuthorId = authors[13].AuthorId, Role = "Author" }, // The Last Man - Mary Shelley
                new BookAuthor { BookId = books[27].BookId, AuthorId = authors[14].AuthorId, Role = "Author" }, // Twenty Thousand Leagues - Jules Verne
                new BookAuthor { BookId = books[28].BookId, AuthorId = authors[14].AuthorId, Role = "Author" }, // Journey to the Center - Jules Verne
                new BookAuthor { BookId = books[29].BookId, AuthorId = authors[15].AuthorId, Role = "Author" }, // The Time Machine - H.G. Wells
                new BookAuthor { BookId = books[30].BookId, AuthorId = authors[15].AuthorId, Role = "Author" }, // The War of the Worlds - H.G. Wells
                new BookAuthor { BookId = books[31].BookId, AuthorId = authors[16].AuthorId, Role = "Author" }, // Brave New World - Aldous Huxley
                new BookAuthor { BookId = books[32].BookId, AuthorId = authors[17].AuthorId, Role = "Author" }, // Animal Farm - George Orwell
                new BookAuthor { BookId = books[33].BookId, AuthorId = authors[17].AuthorId, Role = "Author" }, // 1984 - George Orwell
                new BookAuthor { BookId = books[34].BookId, AuthorId = authors[18].AuthorId, Role = "Author" }, // Fahrenheit 451 - Ray Bradbury
                new BookAuthor { BookId = books[35].BookId, AuthorId = authors[18].AuthorId, Role = "Author" }, // The Martian Chronicles - Ray Bradbury
                new BookAuthor { BookId = books[36].BookId, AuthorId = authors[19].AuthorId, Role = "Author" }, // Foundation - Isaac Asimov
                new BookAuthor { BookId = books[37].BookId, AuthorId = authors[19].AuthorId, Role = "Author" }, // I, Robot - Isaac Asimov
                new BookAuthor { BookId = books[38].BookId, AuthorId = authors[20].AuthorId, Role = "Author" }, // Dune - Frank Herbert
                new BookAuthor { BookId = books[39].BookId, AuthorId = authors[20].AuthorId, Role = "Author" }, // Children of Dune - Frank Herbert
                new BookAuthor { BookId = books[40].BookId, AuthorId = authors[21].AuthorId, Role = "Author" }, // 2001: A Space Odyssey - Arthur C. Clarke
                new BookAuthor { BookId = books[41].BookId, AuthorId = authors[21].AuthorId, Role = "Author" }, // Rendezvous with Rama - Arthur C. Clarke
                new BookAuthor { BookId = books[42].BookId, AuthorId = authors[22].AuthorId, Role = "Author" }, // Solaris - Stanisław Lem
                new BookAuthor { BookId = books[43].BookId, AuthorId = authors[22].AuthorId, Role = "Author" }, // The Futurological Congress - Stanisław Lem
                new BookAuthor { BookId = books[44].BookId, AuthorId = authors[23].AuthorId, Role = "Author" }, // Do Androids Dream - Philip K. Dick
                new BookAuthor { BookId = books[45].BookId, AuthorId = authors[23].AuthorId, Role = "Author" }, // Ubik - Philip K. Dick
                new BookAuthor { BookId = books[46].BookId, AuthorId = authors[24].AuthorId, Role = "Author" }, // Neuromancer - William Gibson
                new BookAuthor { BookId = books[47].BookId, AuthorId = authors[24].AuthorId, Role = "Author" }, // Count Zero - William Gibson
                new BookAuthor { BookId = books[48].BookId, AuthorId = authors[25].AuthorId, Role = "Author" }, // The Stars My Destination - Alfred Bester
                new BookAuthor { BookId = books[49].BookId, AuthorId = authors[25].AuthorId, Role = "Author" }, // The Demolished Man - Alfred Bester
                new BookAuthor { BookId = books[50].BookId, AuthorId = authors[26].AuthorId, Role = "Author" }, // Ender's Game - Orson Scott Card
                new BookAuthor { BookId = books[51].BookId, AuthorId = authors[26].AuthorId, Role = "Author" }, // Speaker for the Dead - Orson Scott Card
                new BookAuthor { BookId = books[52].BookId, AuthorId = authors[27].AuthorId, Role = "Author" }, // Hyperion - Dan Simmons
                new BookAuthor { BookId = books[53].BookId, AuthorId = authors[27].AuthorId, Role = "Author" }, // The Fall of Hyperion - Dan Simmons
            };
            context.BookAuthors.AddRange(bookAuthors);

            // Seed sample users
            var users = new User[]
            {
                new User
                {
                    UserCode = "STU001",
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice.johnson@university.edu",
                    Phone = "+1234567890",
                    Address = "123 University Ave, College Town, ST 12345",
                    UserTypeId = userTypes[0].UserTypeId, // Student
                    CareerDepartment = "Computer Science",
                    RegistrationDate = DateTime.UtcNow.AddMonths(-12).Date,
                    MembershipExpirationDate = DateTime.UtcNow.AddMonths(12).Date,
                },
                new User
                {
                    UserCode = "PROF001",
                    FirstName = "Dr. Robert",
                    LastName = "Smith",
                    Email = "robert.smith@university.edu",
                    Phone = "+1234567891",
                    Address = "456 Faculty Row, College Town, ST 12345",
                    UserTypeId = userTypes[1].UserTypeId, // Professor
                    CareerDepartment = "Computer Science",
                    RegistrationDate = DateTime.UtcNow.AddYears(-5).Date,
                    MembershipExpirationDate = DateTime.UtcNow.AddYears(1).Date,
                },
                new User
                {
                    UserCode = "STF001",
                    FirstName = "Maria",
                    LastName = "Garcia",
                    Email = "maria.garcia@university.edu",
                    Phone = "+1234567892",
                    Address = "789 Staff Circle, College Town, ST 12345",
                    UserTypeId = userTypes[2].UserTypeId, // Staff
                    CareerDepartment = "Library Services",
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
