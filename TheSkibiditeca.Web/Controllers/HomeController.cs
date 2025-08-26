// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Controllers;

/// <summary>
/// The main site controller for top-level pages.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HomeController"/> class.
/// </remarks>
/// <param name="context">The library database context.</param>
/// <param name="userM">User manager for application users.</param>
public class HomeController(LibraryDbContext context, UserManager<User> userM) : Controller
{
    private readonly UserManager<User> userMgr = userM;
    private readonly LibraryDbContext dbContext = context;

    /// <summary>
    /// Shows the home/index page.
    /// </summary>
    /// <returns>The Index view.</returns>
    public async Task<IActionResult> Index()
    {
        var user = await this.userMgr.GetUserAsync(this.HttpContext.User);
        if (user != null)
        {
            this.ViewBag.RoleID = user.UserTypeId;
        }

        var random = new Random();
        int index = random.Next(this.dbContext.Books.Count());
        this.ViewBag.Newest = this.dbContext.Books.ToList().OrderByDescending(b => b.CreatedAt).FirstOrDefault();
        this.ViewBag.Random = this.dbContext.Books.ToArray()[index];
        this.ViewBag.Popular = this.dbContext.Books.ToList().OrderByDescending(b => b.TotalCopies).FirstOrDefault();

        return this.View();
    }

    /// <summary>
    /// Shows the privacy page.
    /// </summary>
    /// <returns>The Privacy view.</returns>
    public IActionResult Privacy()
    {
        return this.View();
    }

    /// <summary>
    /// Shows the lost page.
    /// </summary>
    /// <returns>The Lost view.</returns>
    public IActionResult Lost()
    {
        return this.View();
    }

    /// <summary>
    /// Shows the credits page.
    /// </summary>
    /// <returns>The Credits view.</returns>
    public IActionResult Credits()
    {
        return this.View();
    }
}
