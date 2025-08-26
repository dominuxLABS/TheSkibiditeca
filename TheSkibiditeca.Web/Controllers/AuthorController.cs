// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models.AuthorModels;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Controllers;

/// <summary>
/// Controller for managing authors.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AuthorController"/> class.
/// </remarks>
/// <param name="userM">The user manager.</param>
/// <param name="db">The library database context.</param>
public class AuthorController(UserManager<User> userM, LibraryDbContext db) : Controller
{
    private readonly UserManager<User> userM = userM;
    private readonly LibraryDbContext db = db;

    /// <summary>
    /// Redirects to the Home controller's Index action.
    /// </summary>
    /// <returns>A redirect result to Home/Index.</returns>
    public IActionResult Index()
    {
        return this.RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Displays the form to create a new author if the current user is authorized.
    /// </summary>
    /// <returns>The Create view or a redirect to Home/Lost if the user is not authorized or not found.</returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var user = await this.userM.GetUserAsync(this.HttpContext.User);
        if (user == null)
        {
            return this.RedirectToAction("Lost", "Home");
        }

        if (user.UserTypeId < 2)
        {
            return this.RedirectToAction("Lost", "Home");
        }

        return this.View();
    }

    /// <summary>
    /// Handles submission of a new author creation request if the current user is authorized.
    /// </summary>
    /// <param name="model">The author creation model.</param>
    /// <returns>The Create view or a redirect to Home/Lost if the user is not authorized or not found.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(AuthorCreateModel model)
    {
        var user = await this.userM.GetUserAsync(this.HttpContext.User);
        if (user == null)
        {
            return this.RedirectToAction("Lost", "Home");
        }

        if (user.UserTypeId < 2)
        {
            return this.RedirectToAction("Lost", "Home");
        }

        this.db.Authors.Add(model.author);
        this.db.SaveChanges();
        return this.View();
    }
}
