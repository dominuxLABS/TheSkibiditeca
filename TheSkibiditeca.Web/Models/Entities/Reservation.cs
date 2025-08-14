// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities
{
    /// <summary>
    /// Represents a book reservation in the library system.
    /// </summary>
    [Table("Reservations")]
    public class Reservation
    {
        /// <summary>
        /// Gets or sets the reservation identifier.
        /// </summary>
        [Key]
        public int ReservationId { get; set; }

        /// <summary>
        /// Gets or sets the book identifier.
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the reservation date.
        /// </summary>
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the reservation expiration date.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has been notified.
        /// </summary>
        public bool IsNotified { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the reservation is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the book navigation property.
        /// </summary>
        public virtual Book Book { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user navigation property.
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// Gets a value indicating whether the reservation has expired.
        /// </summary>
        [NotMapped]
        public bool IsExpired => DateTime.UtcNow.Date > this.ExpirationDate.Date;
    }
}
