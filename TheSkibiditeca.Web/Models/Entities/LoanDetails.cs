// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities
{
    /// <summary>
    /// Represents a loan detail entry that acts as an intermediate entity between Copy and Loan.
    /// This allows for implementing shopping cart-like functionality where users can select
    /// multiple copies before creating actual loans.
    /// </summary>
    [Table("LoanDetails")]
    public class LoanDetails
    {
        /// <summary>
        /// Gets or sets the loan detail identifier.
        /// </summary>
        [Key]
        public int LoanDetailId { get; set; }

        /// <summary>
        /// Gets or sets the loan identifier.
        /// </summary>
        public int LoanId { get; set; }

        /// <summary>
        /// Gets or sets the copy (ejemplar) identifier.
        /// </summary>
        public int CopyId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of copies (usually 1 for library books).
        /// </summary>
        [Range(1, 10)]
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the date when this item was added to the loan.
        /// </summary>
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets additional notes for this specific loan detail.
        /// </summary>
        [Column(TypeName = "text")]
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this loan detail is active.
        /// This can be used to soft-delete items from the cart without removing them completely.
        /// </summary>
        public bool IsActive { get; set; } = true;

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
        public virtual Loan Loan { get; set; } = null!;

        /// <summary>
        /// Gets or sets the copy navigation property.
        /// </summary>
        public virtual Copy Copy { get; set; } = null!;
    }
}
