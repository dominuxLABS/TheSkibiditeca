// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities
{
    /// <summary>
    /// Represents a publisher in the library system.
    /// </summary>
    [Table("Publishers")]
    public class Publisher
    {
        /// <summary>
        /// Gets or sets the publisher identifier.
        /// </summary>
        [Key]
        public int PublisherId { get; set; }

        /// <summary>
        /// Gets or sets the publisher name.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the publisher address.
        /// </summary>
        [StringLength(500)]
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the publisher phone number.
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the publisher email.
        /// </summary>
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the publisher website URL.
        /// </summary>
        [StringLength(200)]
        [Url]
        public string? Website { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets a value indicating whether the publisher is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the books published by this publisher.
        /// </summary>
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
