// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities
{
    /// <summary>
    /// Represents a book loan in the library system.
    /// </summary>
    [Table("Loans")]
    public class Loan
    {
        /// <summary>
        /// Gets or sets the loan identifier.
        /// </summary>
        [Key]
        public int LoanId { get; set; }

        /// <summary>
        /// Gets or sets the book identifier.
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the loan status identifier.
        /// </summary>
        public int LoanStatusId { get; set; } = 1; // Active by default

        /// <summary>
        /// Gets or sets the loan date.
        /// </summary>
        public DateTime LoanDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the expected return date.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime ExpectedReturnDate { get; set; }

        /// <summary>
        /// Gets or sets the actual return date.
        /// </summary>
        public DateTime? ActualReturnDate { get; set; }

        /// <summary>
        /// Gets or sets the number of renewals made.
        /// </summary>
        [Range(0, 10)]
        public int RenewalsCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum number of renewals allowed.
        /// </summary>
        [Range(0, 10)]
        public int MaxRenewals { get; set; } = 2;

        /// <summary>
        /// Gets or sets loan observations.
        /// </summary>
        [Column(TypeName = "text")]
        public string? Observations { get; set; }

        /// <summary>
        /// Gets or sets the staff member who processed the loan.
        /// </summary>
        [StringLength(100)]
        public string? StaffMember { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last update date.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the book navigation property.
        /// </summary>
        public virtual Book Book { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user navigation property.
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the loan status navigation property.
        /// </summary>
        public virtual LoanStatus LoanStatus { get; set; } = null!;

        /// <summary>
        /// Gets or sets the fines associated with this loan.
        /// </summary>
        public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

        /// <summary>
        /// Gets a value indicating whether the loan is overdue.
        /// </summary>
        [NotMapped]
        public bool IsOverdue => this.ActualReturnDate == null && DateTime.UtcNow.Date > this.ExpectedReturnDate.Date;

        /// <summary>
        /// Gets the number of days overdue.
        /// </summary>
        [NotMapped]
        public int DaysOverdue => this.IsOverdue ? (int)(DateTime.UtcNow.Date - this.ExpectedReturnDate.Date).TotalDays : 0;

        /// <summary>
        /// Gets a value indicating whether the loan can be renewed.
        /// </summary>
        [NotMapped]
        public bool CanRenew => this.RenewalsCount < this.MaxRenewals && this.ActualReturnDate == null;
    }
}
