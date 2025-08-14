// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities
{
    /// <summary>
    /// Represents an author in the library system.
    /// </summary>
    [Table("Authors")]
    public class Author
    {
        /// <summary>
        /// Gets or sets the author identifier.
        /// </summary>
        [Key]
        public int AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the author's first name.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the author's last name.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the author's biography.
        /// </summary>
        [Column(TypeName = "text")]
        public string? Biography { get; set; }

        /// <summary>
        /// Gets or sets the author's birth date.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the author's nationality.
        /// </summary>
        [StringLength(50)]
        public string? Nationality { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last update date.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets a value indicating whether the author is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the book-author relationships.
        /// </summary>
        public virtual ICollection<BookAuthor> BookAuthors { get; set; } = [];

        /// <summary>
        /// Gets the author's full name.
        /// </summary>
        [NotMapped]
        public string FullName => $"{this.FirstName} {this.LastName}";
    }
}
