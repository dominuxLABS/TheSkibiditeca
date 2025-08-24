// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.AspNetCore.Mvc;
using TheSkibiditeca.Web.Models.ModelPartial;

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
        public IActionResult Book(string? searchStr, int page = 1, int pageSize = 30) {
            var allBooks = new List<BookMiniCardModel>();
            for (int i = 0; i < 500; i++){
                allBooks.Add(new BookMiniCardModel() {
                    ImageURL = "https://m.media-amazon.com/images/I/51n4B7p6cML._SY250_.jpg",
                    Title = $"Ramires al enterarse {i + 1}",
                    BookID = "1",
                });
            }

            if (!String.IsNullOrEmpty(searchStr)) {
                allBooks = allBooks.Where(b => b.Title.Contains(searchStr, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var paginatedBooks = allBooks.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.AllBooks = paginatedBooks;
            ViewData["CurrentPage"] = page;
            ViewData["CurrentFilter"] = searchStr;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = (int)Math.Ceiling(allBooks.Count / (double)pageSize);
            return this.View();
        }
    }
}
