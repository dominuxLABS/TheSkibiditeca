// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities
{
    /// <summary>
    /// Represents a fine type in the library system.
    /// </summary>
    [Table("FineTypes")]
    public class FineType
    {
        /// <summary>
        /// Gets or sets the fine type identifier.
        /// </summary>
        [Key]
        public int FineTypeId { get; set; }

        /// <summary>
        /// Gets or sets the fine type name.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the fine type description.
        /// </summary>
        [StringLength(300)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the base amount for this fine type.
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 9999.99)]
        public decimal BaseAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the fine type is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the fines of this type.
        /// </summary>
        public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();
    }
}
