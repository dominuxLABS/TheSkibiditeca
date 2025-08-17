// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities
{
    /// <summary>
    /// Represents a physical copy (ejemplar) of a book. Each copy can have its own ISBN and publisher string.
    /// </summary>
    [Table("Copies")]
    public class Copy
    {
    /// <summary>
    /// Gets or sets the primary key for this physical copy (ejemplar).
    /// </summary>
        [Key]
        public int CopyId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the book metadata.
    /// </summary>
        public int BookId { get; set; }

    /// <summary>
    /// Gets or sets the ISBN of this physical copy (optional).
    /// </summary>
        [StringLength(20)]
        public string? ISBN { get; set; }

    /// <summary>
    /// Gets or sets the publisher name stored as a simple string to simplify schema.
    /// </summary>
        [StringLength(200)]
        public string? PublisherName { get; set; }

    /// <summary>
    /// Gets or sets the location or shelf identifier for this copy.
    /// </summary>
        [StringLength(100)]
        public string? PhysicalLocation { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this copy is active/usable in inventory.
    /// </summary>
        public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the navigation to the book metadata.
    /// </summary>
        public virtual Book? Book { get; set; }

    /// <summary>
    /// Gets or sets the loans associated with this copy through loan details.
    /// </summary>
        public virtual ICollection<LoanDetails> LoanDetails { get; set; } = new List<LoanDetails>();

    /// <summary>
    /// Gets or sets the reservations associated with this copy.
    /// </summary>
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    // GenerateIsbn removed — generation logic moved to DbSeeder (placeholder data only)
    }
}
