// Copyright (c) dominuxLABS. All rights reserved.

using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Models.AuthorModels;

/// <summary>
/// Model used when creating an author.
/// </summary>
public class AuthorCreateModel
{
    /// <summary>
    /// Gets or sets the author to create.
    /// </summary>
    public required Author Author { get; set; }
}
