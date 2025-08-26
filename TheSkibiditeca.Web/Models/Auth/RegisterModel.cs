// Copyright (c) dominuxLABS. All rights reserved.

using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Models.Auth;

/// <summary>
/// Represents the model for user registration data.
/// </summary>
public class RegisterModel
{
    /// <summary>
    /// Gets or sets the user entity for the registering account.
    /// </summary>
    public User User { get; set; } = new User();

    /// <summary>
    /// Gets or sets the password provided by the user.
    /// </summary>
    public string PasswordStr { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password confirmation provided by the user.
    /// </summary>
    public string PasswordConf { get; set; } = string.Empty;
}
