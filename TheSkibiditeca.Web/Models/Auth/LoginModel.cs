// Copyright (c) dominuxLABS. All rights reserved.

namespace TheSkibiditeca.Web.Models.Auth;

/// <summary>
/// Model representing login credentials.
/// </summary>
public class LoginModel
{
    /// <summary>
    /// Gets or sets the user email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
