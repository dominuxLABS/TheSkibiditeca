// Copyright (c) dominuxLABS. All rights reserved.

using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Models.BookModels;

/// <summary>
/// Model used to create a new book along with its authors and copy count.
/// </summary>
public class CreateBookModel
{
    /// <summary>
    /// Gets or sets the book entity to create.
    /// </summary>
    public required Book Book { get; set; }

    /// <summary>
    /// Gets or sets the list of author names associated with the book.
    /// </summary>
    public required List<string> Authors { get; set; }

    /// <summary>
    /// Gets or sets the number of copies to create.
    /// </summary>
    public int Copies { get; set; }
}
