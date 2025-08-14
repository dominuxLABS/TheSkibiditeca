// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities
{
    /// <summary>
    /// Represents a fine in the library system.
    /// </summary>
    [Table("Fines")]
    public class Fine
    {
        /// <summary>
        /// Gets or sets the fine identifier.
        /// </summary>
        [Key]
        public int FineId { get; set; }

        /// <summary>
        /// Gets or sets the loan identifier.
        /// </summary>
        public int? LoanId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the fine type identifier.
        /// </summary>
        public int FineTypeId { get; set; }

        /// <summary>
        /// Gets or sets the fine amount.
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 9999.99)]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the fine date.
        /// </summary>
        public DateTime FineDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the payment date.
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the number of days overdue.
        /// </summary>
        [Range(0, 1000)]
        public int DaysOverdue { get; set; } = 0;

        /// <summary>
        /// Gets or sets the fine description.
        /// </summary>
        [Column(TypeName = "text")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the fine has been paid.
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last update date.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the loan navigation property.
        /// </summary>
        public virtual Loan? Loan { get; set; }

        /// <summary>
        /// Gets or sets the user navigation property.
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the fine type navigation property.
        /// </summary>
        public virtual FineType FineType { get; set; } = null!;
    }
}
