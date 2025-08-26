// Copyright (c) dominuxLABS. All rights reserved.

using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Models;

/// <summary>
/// Represents a shopping cart containing copies selected by the user.
/// </summary>
public class ShoppingCart
{
    /// <summary>
    /// Gets the list of copies in the cart.
    /// </summary>
    public List<Copy> Copies { get; } = new List<Copy>();
}
