// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.AspNetCore.Mvc;

namespace TheSkibiditeca.Web.Controllers
{
    /// <summary>
    /// Controller for listing pages (books, categories, etc.).
    /// </summary>
    public class ListController : Controller
    {
        /// <summary>
        /// Redirects to the home index page.
        /// </summary>
        /// <returns>A redirect result to Home/Index.</returns>
        public IActionResult Index()
        {
            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Shows the book list view.
        /// </summary>
        /// <returns>The Book view.</returns>
        public IActionResult Book()
        {
            return this.View();
        }
    }
}
