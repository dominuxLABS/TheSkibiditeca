// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities;

/// <summary>
/// Represents a book category in the library system.
/// </summary>
[Table("Categories")]
public class Category
{
    /// <summary>
    /// Gets or sets the category identifier.
    /// </summary>
    [Key]
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the category name.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category description.
    /// </summary>
    [Column(TypeName = "text")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets a value indicating whether the category is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the books in this category.
    /// </summary>
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
