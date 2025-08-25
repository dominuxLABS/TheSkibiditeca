// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models.Entities;
using TheSkibiditeca.Web.Models.ModelPartial;

namespace TheSkibiditeca.Web.Controllers
{
    /// <summary>
    /// Controller for listing pages (books, categories, etc.).
    /// </summary>
    public class ListController : Controller
    {
        private readonly UserManager<User> _userM;
        private readonly LibraryDbContext db;
        public ListController(LibraryDbContext context, UserManager<User> userM, LibraryDbContext db) {
            _userM = userM;
            this.db = db;
        }
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
        public async Task<IActionResult> Book(string? searchStr, int page = 1, int pageSize = 30) {
            var allBooks = new List<BookMiniCardModel>();
            foreach(Book b in db.Books) {
                allBooks.Add(new BookMiniCardModel() {
                    BookID = b.BookId.ToString(),
                    Title = b.Title,
                    ImageURL = b.CoverImageUrl
                });
            }

            if (!String.IsNullOrEmpty(searchStr)) {
                allBooks = allBooks.Where(b => b.Title.Contains(searchStr, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var paginatedBooks = allBooks.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.AllBooks = paginatedBooks;
            var user = await _userM.GetUserAsync(HttpContext.User);
            if(user != null) { ViewBag.RoleID = user.UserTypeId; }
            ViewData["CurrentPage"] = page;
            ViewData["CurrentFilter"] = searchStr;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = (int)Math.Ceiling(allBooks.Count / (double)pageSize);
            return this.View();
        }
    }
}
