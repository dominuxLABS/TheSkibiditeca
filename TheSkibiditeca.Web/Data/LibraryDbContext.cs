// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.EntityFrameworkCore;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Data
{
    /// <summary>
    /// Database context for the library management system.
    /// </summary>
    public class LibraryDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the authors table.
        /// </summary>
        public DbSet<Author> Authors { get; set; }

        /// <summary>
        /// Gets or sets the categories table.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the publishers table.
        /// </summary>
        public DbSet<Publisher> Publishers { get; set; }

        /// <summary>
        /// Gets or sets the books table.
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Gets or sets the book-authors relationship table.
        /// </summary>
        public DbSet<BookAuthor> BookAuthors { get; set; }

        /// <summary>
        /// Gets or sets the user types table.
        /// </summary>
        public DbSet<UserType> UserTypes { get; set; }

        /// <summary>
        /// Gets or sets the users table.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the loan statuses table.
        /// </summary>
        public DbSet<LoanStatus> LoanStatuses { get; set; }

        /// <summary>
        /// Gets or sets the loans table.
        /// </summary>
        public DbSet<Loan> Loans { get; set; }

        /// <summary>
        /// Gets or sets the reservations table.
        /// </summary>
        public DbSet<Reservation> Reservations { get; set; }

        /// <summary>
        /// Gets or sets the fine types table.
        /// </summary>
        public DbSet<FineType> FineTypes { get; set; }

        /// <summary>
        /// Gets or sets the fines table.
        /// </summary>
        public DbSet<Fine> Fines { get; set; }

        /// <summary>
        /// Gets or sets the audit logs table.
        /// </summary>
        public DbSet<AuditLog> AuditLogs { get; set; }

        /// <summary>
        /// Override SaveChanges to automatically update timestamps.
        /// </summary>
        /// <returns>The number of entries written to the database.</returns>
        public override int SaveChanges()
        {
            this.UpdateTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        /// Override SaveChangesAsync to automatically update timestamps.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation with the number of entries written.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Configures the entity models and relationships.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure BookAuthor composite key
            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId, ba.AuthorId });

            // Configure BookAuthor relationships
            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure unique constraints
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique()
                .HasFilter("[ISBN] IS NOT NULL");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserCode)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<UserType>()
                .HasIndex(ut => ut.Name)
                .IsUnique();

            modelBuilder.Entity<LoanStatus>()
                .HasIndex(ls => ls.Name)
                .IsUnique();

            // Configure relationships with optional foreign keys
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure User relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserType)
                .WithMany(ut => ut.Users)
                .HasForeignKey(u => u.UserTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Loan relationships
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.User)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.LoanStatus)
                .WithMany(ls => ls.Loans)
                .HasForeignKey(l => l.LoanStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Reservation relationships
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reservations)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Fine relationships
            modelBuilder.Entity<Fine>()
                .HasOne(f => f.Loan)
                .WithMany(l => l.Fines)
                .HasForeignKey(f => f.LoanId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Fine>()
                .HasOne(f => f.User)
                .WithMany(u => u.Fines)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Fine>()
                .HasOne(f => f.FineType)
                .WithMany(ft => ft.Fines)
                .HasForeignKey(f => f.FineTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure default values and constraints
            ConfigureDefaultValues(modelBuilder);
        }

        /// <summary>
        /// Configures default values for entities.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void ConfigureDefaultValues(ModelBuilder modelBuilder)
        {
            // Set default values
            modelBuilder.Entity<Author>()
                .Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Author>()
                .Property(a => a.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Book>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Book>()
                .Property(b => b.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Loan>()
                .Property(l => l.LoanDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Loan>()
                .Property(l => l.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Loan>()
                .Property(l => l.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }

        /// <summary>
        /// Updates the UpdatedAt timestamp for modified entities.
        /// </summary>
        private void UpdateTimestamps()
        {
            var entries = this.ChangeTracker.Entries()
                .Where(e => e.Entity is Author || e.Entity is Book || e.Entity is User || e.Entity is Loan)
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is Author author)
                {
                    author.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is Book book)
                {
                    book.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is User user)
                {
                    user.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is Loan loan)
                {
                    loan.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
