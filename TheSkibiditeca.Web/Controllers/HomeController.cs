// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Controllers
{
    /// <summary>
    /// The main site controller for top-level pages.
    /// </summary>
   
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userM;
        public HomeController(LibraryDbContext context, UserManager<User> userM) {
            _userM = userM;
        }

        /// <summary>
        /// Shows the home/index page.
        /// </summary>
        /// <returns>The Index view.</returns>
        public async Task<IActionResult> Index()
        {
            var user = await _userM.GetUserAsync(HttpContext.User);
            
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
