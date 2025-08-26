// Copyright (c) dominuxLABS. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSkibiditeca.Web.Models.Entities;

/// <summary>
/// Represents the many-to-many relationship between books and authors.
/// </summary>
[Table("BookAuthors")]
public class BookAuthor
{
    /// <summary>
    /// Gets or sets the book identifier.
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Gets or sets the author identifier.
    /// </summary>
    public int AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the author's role for this book.
    /// </summary>
    [StringLength(50)]
    public string Role { get; set; } = "Author";

    /// <summary>
    /// Gets or sets the book navigation property.
    /// </summary>
    public virtual Book Book { get; set; } = null!;

    /// <summary>
    /// Gets or sets the author navigation property.
    /// </summary>
    public virtual Author Author { get; set; } = null!;
}
