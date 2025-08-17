// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheSkibiditeca.Web.Models.Enums;

namespace TheSkibiditeca.Web.Models.Entities
{
    /// <summary>
    /// Represents a book in the library system.
    /// </summary>
    [Table("Books")]
    public class Book
    {
        /// <summary>
        /// Gets or sets the book identifier.
        /// </summary>
        [Key]
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the book title.
        /// </summary>
        [Required]
        [StringLength(300)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the publication year.
        /// </summary>
        [Range(1000, 9999)]
        public int? PublicationYear { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the book description.
        /// </summary>
        [Column(TypeName = "text")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the cover image URL.
        /// </summary>
        [StringLength(500)]
        [Url]
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last update date.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the category navigation property.
        /// </summary>
        public virtual Category? Category { get; set; }

        /// <summary>
        /// Gets or sets the book-author relationships.
        /// </summary>
        public virtual ICollection<BookAuthor> BookAuthors { get; set; } = [];

        /// <summary>
        /// Gets or sets the loans for this book.
        /// </summary>
        public virtual ICollection<Loan> Loans { get; set; } = [];

        /// <summary>
        /// Gets or sets the reservations for this book.
        /// </summary>
        public virtual ICollection<Reservation> Reservations { get; set; } = [];

        /// <summary>
        /// Gets or sets the physical copies (ejemplares) for this book.
        /// </summary>
        public virtual ICollection<Copy> Copies { get; set; } = [];

        /// <summary>
        /// Gets the total number of copies for this book.
        /// </summary>
        [NotMapped]
        public int TotalCopies => this.Copies?.Count ?? 0;

        /// <summary>
        /// Gets the number of available copies (active and not currently loaned).
        /// </summary>
        [NotMapped]
        public int AvailableCopies => this.Copies?.Count(c => c.IsActive && (c.Loans == null || !c.Loans.Any(l => l.ActualReturnDate == null && l.Status != LoanStatusType.Returned))) ?? 0;

        /// <summary>
        /// Gets a value indicating whether the book is available for loan.
        /// </summary>
        [NotMapped]
        public bool IsAvailable => this.AvailableCopies > 0;

        /// <summary>
        /// Gets the authors' names as a comma-separated string.
        /// </summary>
        [NotMapped]
        public string AuthorNames => string.Join(", ", this.BookAuthors.Select(ba => ba.Author?.FullName));

        /// <summary>
        /// Gets all authors associated with this book.
        /// </summary>
        /// <returns>A collection of authors for this book.</returns>
        public IEnumerable<Author> GetAuthors()
        {
            return this.BookAuthors
                .Where(ba => ba.Author != null)
                .Select(ba => ba.Author!)
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName);
        }

        /// <summary>
        /// Gets authors by role (e.g., "Author", "Editor", "Translator").
        /// </summary>
        /// <param name="role">The role to filter by.</param>
        /// <returns>A collection of authors with the specified role.</returns>
        public IEnumerable<Author> GetAuthorsByRole(string role)
        {
            return this.BookAuthors
                .Where(ba => ba.Role.Equals(role, StringComparison.OrdinalIgnoreCase) && ba.Author != null)
                .Select(ba => ba.Author!)
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName);
        }

        /// <summary>
        /// Gets the primary authors (those with "Author" role).
        /// </summary>
        /// <returns>A collection of primary authors.</returns>
        public IEnumerable<Author> GetPrimaryAuthors()
        {
            return this.GetAuthorsByRole("Author");
        }

        /// <summary>
        /// Gets authors' full names as a formatted string.
        /// </summary>
        /// <param name="separator">The separator to use between names. Default is ", ".</param>
        /// <returns>A formatted string of author names.</returns>
        public string GetAuthorsDisplayString(string separator = ", ")
        {
            var authors = this.GetAuthors();
            return string.Join(separator, authors.Select(a => a.FullName));
        }

        /// <summary>
        /// Checks if a specific author is associated with this book.
        /// </summary>
        /// <param name="authorId">The author identifier to check.</param>
        /// <returns>True if the author is associated with this book; otherwise, false.</returns>
        public bool HasAuthor(int authorId)
        {
            return this.BookAuthors.Any(ba => ba.AuthorId == authorId);
        }

        /// <summary>
        /// Gets the count of authors associated with this book.
        /// </summary>
        /// <returns>The number of authors.</returns>
        public int GetAuthorCount()
        {
            return this.BookAuthors.Count(ba => ba.Author != null);
        }
    }
}
