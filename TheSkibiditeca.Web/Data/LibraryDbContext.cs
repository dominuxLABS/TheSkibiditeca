// Copyright (c) dominuxLABS. All rights reserved.
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Data
{
    /// <summary>
    /// Database context for the library management system. Integrates ASP.NET Core Identity
    /// using the existing Users table (mapped to User entity).
    /// </summary>
    public class LibraryDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to configure the database context.</param>
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the authors table.
        /// </summary>
        public DbSet<Author> Authors { get; set; } = null!;

        /// <summary>
        /// Gets or sets the categories table.
        /// </summary>
        public DbSet<Category> Categories { get; set; } = null!;

        /// <summary>
        /// Gets or sets the books table.
        /// </summary>
        public DbSet<Book> Books { get; set; } = null!;

        /// <summary>
        /// Gets or sets the physical copies (ejemplares) table.
        /// </summary>
        public DbSet<Copy> Copies { get; set; } = null!;

        /// <summary>
        /// Gets or sets the book-author relationship table.
        /// </summary>
        public DbSet<BookAuthor> BookAuthors { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user types table.
        /// </summary>
        public DbSet<UserType> UserTypes { get; set; } = null!;

        // Users DbSet is provided by IdentityDbContext<User, ...>

        /// <summary>
        /// Gets or sets the loans table.
        /// </summary>
        public DbSet<Loan> Loans { get; set; } = null!;

        /// <summary>
        /// Gets or sets the loan details table.
        /// </summary>
        public DbSet<LoanDetails> LoanDetails { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reservations table.
        /// </summary>
        public DbSet<Reservation> Reservations { get; set; } = null!;

        /// <summary>
        /// Gets or sets the fines table.
        /// </summary>
        public DbSet<Fine> Fines { get; set; } = null!;

        /// <summary>
        /// Gets or sets the audit logs table.
        /// </summary>
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

        /// <summary>
        /// Saves changes to the database and updates timestamps for modified entities.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public override int SaveChanges()
        {
            this.UpdateTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves changes to the database and updates timestamps for modified entities.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result is the number of state entries written to the database.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Configures the entity models and relationships for the database.
        /// </summary>
        /// <param name="builder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Map User to existing Users table and columns
            builder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.HasKey(u => u.Id);
                b.Property(u => u.Id).HasColumnName("UserId");
                b.Property(u => u.UserName).HasColumnName("UserCode").HasMaxLength(20).IsRequired();
                b.Property(u => u.PhoneNumber).HasColumnName("Phone").HasMaxLength(20);
                b.Property(u => u.PasswordHash).HasColumnName("PasswordHash").HasMaxLength(500);
                b.Property(u => u.Email).HasMaxLength(150);
            });

            // Configure BookAuthor composite key
            builder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId, ba.AuthorId });

            // BookAuthor relationships
            builder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraints and indexes
            builder.Entity<Copy>()
                .HasIndex(c => c.ISBN)
                .IsUnique()
                .HasFilter("[ISBN] IS NOT NULL");

            builder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<UserType>()
                .HasIndex(ut => ut.Name)
                .IsUnique();

            // Relationships
            builder.Entity<Copy>()
                .HasOne(c => c.Book)
                .WithMany(b => b.Copies)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<User>()
                .HasOne(u => u.UserType)
                .WithMany(ut => ut.Users)
                .HasForeignKey(u => u.UserTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Loan>()
                .HasOne(l => l.User)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LoanDetails>()
                .HasOne(ld => ld.Loan)
                .WithMany(l => l.LoanDetails)
                .HasForeignKey(ld => ld.LoanId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LoanDetails>()
                .HasOne(ld => ld.Copy)
                .WithMany(c => c.LoanDetails)
                .HasForeignKey(ld => ld.CopyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Reservation>()
                .HasOne(r => r.Copy)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CopyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Fine>()
                .HasOne(f => f.Loan)
                .WithMany(l => l.Fines)
                .HasForeignKey(f => f.LoanId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Fine>()
                .HasOne(f => f.User)
                .WithMany(u => u.Fines)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure default values and constraints for SQL Server
            ConfigureForSqlServer(builder);

            ConfigureUniqueConstraints(builder);
        }

        private static void ConfigureForSqlServer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Author>()
                .Property(a => a.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Book>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Copy>()
                .Property(c => c.CopyId)
                .ValueGeneratedOnAdd();

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

            modelBuilder.Entity<LoanDetails>()
                .Property(ld => ld.DateAdded)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<LoanDetails>()
                .Property(ld => ld.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<LoanDetails>()
                .Property(ld => ld.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }

        private static void ConfigureUniqueConstraints(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        private void UpdateTimestamps()
        {
            var entries = this.ChangeTracker.Entries()
                .Where(e => e.Entity is Author || e.Entity is Book || e.Entity is User || e.Entity is Loan || e.Entity is LoanDetails)
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
                else if (entry.Entity is LoanDetails loanDetails)
                {
                    loanDetails.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
