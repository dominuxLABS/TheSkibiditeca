// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.AspNetCore.Mvc;

namespace TheSkibiditeca.Web.Controllers
{
    /// <summary>
    /// The main site controller for top-level pages.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Shows the home/index page.
        /// </summary>
        /// <returns>The Index view.</returns>
        public IActionResult Index()
        {
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
        /// Shows the credits page.
        /// </summary>
        /// <returns>The Credits view.</returns>
        public IActionResult Credits()
        {
            return this.View();
        }
    }
}
