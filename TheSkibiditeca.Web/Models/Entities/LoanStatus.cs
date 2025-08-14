// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities
{
    /// <summary>
    /// Represents a loan status in the library system.
    /// </summary>
    [Table("LoanStatuses")]
    public class LoanStatus
    {
        /// <summary>
        /// Gets or sets the loan status identifier.
        /// </summary>
        [Key]
        public int LoanStatusId { get; set; }

        /// <summary>
        /// Gets or sets the status name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status description.
        /// </summary>
        [StringLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the loan status is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the loans with this status.
        /// </summary>
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
