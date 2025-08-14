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
        /// The base fine amount for overdue books.
        /// </summary>
        public const decimal BaseFineAmount = 5.00m;

        /// <summary>
        /// The daily increment amount for overdue fines.
        /// </summary>
        public const decimal DailyIncrementAmount = 1.50m;

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
        /// Calculates the fine amount based on the number of days overdue.
        /// </summary>
        /// <param name="daysOverdue">The number of days overdue.</param>
        /// <returns>The calculated fine amount.</returns>
        public static decimal CalculateFineAmount(int daysOverdue)
        {
            if (daysOverdue <= 0)
            {
                return 0m;
            }

            return BaseFineAmount + (DailyIncrementAmount * daysOverdue);
        }

        /// <summary>
        /// Updates the fine amount based on the current days overdue.
        /// </summary>
        public void UpdateFineAmount()
        {
            this.Amount = CalculateFineAmount(this.DaysOverdue);
        }
    }
}
