// Copyright (c) dominuxLABS. All rights reserved.

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TheSkibiditeca.Web.Models;

namespace TheSkibiditeca.Web.Controllers
{
    /// <summary>
    /// Controller for handling home page requests and basic application functionality.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for this controller.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Displays the home page.
        /// </summary>
        /// <returns>The index view.</returns>
        public IActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Displays the privacy policy page.
        /// </summary>
        /// <returns>The privacy view.</returns>
        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult Credits() {
            return this.View();
        }

        /// <summary>
        /// Displays the error page with diagnostic information.
        /// </summary>
        /// <returns>The error view with error details.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
