// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Controllers
{
    /// <summary>
    /// Controller responsible for displaying details about books and other entities.
    /// </summary>
    public class DetailsController : Controller
    {
        private readonly LibraryDbContext db;
        private readonly UserManager<User> _userM;

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailsController"/> class.
        /// </summary>
        /// <param name="context">The library database context.</param>
        public DetailsController(LibraryDbContext context, UserManager<User> user)
        {
            this.db = context;
            this._userM = user;
        }

        /// <summary>
        /// Redirects to the home page index.
        /// </summary>
        /// <returns>A redirect to the Home controller index action.</returns>
        public IActionResult Index()
        {
            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Shows detailed information for a book.
        /// </summary>
        /// <param name="bookId">The book identifier (as string). If null or invalid, defaults to 1.</param>
        /// <returns>The book details view, or NotFound if the book doesn't exist.</returns>
        public async Task<IActionResult> Book(string? bookId)
        {
            if (!int.TryParse(bookId, out var id))
            {
                id = 1;
            }

            var user = await _userM.GetUserAsync(HttpContext.User);
            if(user != null) { ViewBag.RoleID = user.UserTypeId; }

            var c = this.db.Books.Find(id);
            if (c == null)
            {
                return this.NotFound();
            }

            // log a serialized copy for debugging (kept intentionally simple)
            var serialized = JsonSerializer.Serialize(c);
            var authorsIds = this.db.BookAuthors.Where(e => e.BookId == c.BookId).Select(e => e.AuthorId);
            var authorNames = this.db.Authors.Where(e => authorsIds.Contains(e.AuthorId)).Select(e => e.FullName);

            this.ViewBag.Title = c.Title;
            this.ViewBag.Description = c.Description;
            this.ViewBag.Year = c.PublicationYear;
            this.ViewBag.Authors = String.Join(",", authorNames);
            this.ViewBag.Count = this.db.Copies.Where(e => e.BookId == c.BookId).Count();
            this.ViewBag.URL = c.CoverImageUrl;
            return this.View();
        }

        public IActionResult Author() {
            return this.View();
        }
    }
}
