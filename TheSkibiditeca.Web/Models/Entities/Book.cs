// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        /// Gets or sets the book ISBN.
        /// </summary>
        [StringLength(20)]
        public string? ISBN { get; set; }

        /// <summary>
        /// Gets or sets the publisher identifier.
        /// </summary>
        public int? PublisherId { get; set; }

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
        /// Gets or sets the number of pages.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? NumberOfPages { get; set; }

        /// <summary>
        /// Gets or sets the book language.
        /// </summary>
        [StringLength(50)]
        public string Language { get; set; } = "English";

        /// <summary>
        /// Gets or sets the physical location of the book.
        /// </summary>
        [StringLength(100)]
        public string? PhysicalLocation { get; set; }

        /// <summary>
        /// Gets or sets the total quantity of this book.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int TotalQuantity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the available quantity of this book.
        /// </summary>
        [Range(0, int.MaxValue)]
        public int AvailableQuantity { get; set; } = 1;

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
        /// Gets or sets the acquisition price.
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal? AcquisitionPrice { get; set; }

        /// <summary>
        /// Gets or sets the acquisition date.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? AcquisitionDate { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last update date.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets a value indicating whether the book is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the publisher navigation property.
        /// </summary>
        public virtual Publisher? Publisher { get; set; }

        /// <summary>
        /// Gets or sets the category navigation property.
        /// </summary>
        public virtual Category? Category { get; set; }

        /// <summary>
        /// Gets or sets the book-author relationships.
        /// </summary>
        public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

        /// <summary>
        /// Gets or sets the loans for this book.
        /// </summary>
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

        /// <summary>
        /// Gets or sets the reservations for this book.
        /// </summary>
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        /// <summary>
        /// Gets a value indicating whether the book is available for loan.
        /// </summary>
        [NotMapped]
        public bool IsAvailable => this.AvailableQuantity > 0;

        /// <summary>
        /// Gets the authors' names as a comma-separated string.
        /// </summary>
        [NotMapped]
        public string AuthorNames => string.Join(", ", this.BookAuthors.Select(ba => ba.Author?.FullName));
    }
}
