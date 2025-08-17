// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities
{
    /// <summary>
    /// Represents a user type in the library system.
    /// </summary>
    [Table("UserTypes")]
    public class UserType
    {
        /// <summary>
        /// Gets or sets the user type identifier.
        /// </summary>
        [Key]
        public int UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the user type name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the maximum loan days for this user type.
        /// </summary>
        [Range(1, 365)]
        public int MaxLoanDays { get; set; } = 7;

        /// <summary>
        /// Gets or sets the maximum number of books that can be borrowed.
        /// </summary>
        [Range(1, 50)]
        public int MaxBooksAllowed { get; set; } = 3;

        /// <summary>
        /// Gets or sets a value indicating whether users of this type can renew loans.
        /// </summary>
        public bool CanRenew { get; set; } = true;

        /// <summary>
        /// Gets or sets the daily fine amount for overdue books.
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 999.99)]
        public decimal DailyFineAmount { get; set; } = 1.00m;

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets a value indicating whether the user type is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the users of this type.
        /// </summary>
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
