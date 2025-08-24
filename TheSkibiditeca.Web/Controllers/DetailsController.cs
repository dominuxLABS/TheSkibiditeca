// Copyright (c) dominuxLABS. All rights reserved.

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TheSkibiditeca.Web.Data;

namespace TheSkibiditeca.Web.Controllers
{
    /// <summary>
    /// Controller responsible for displaying details about books and other entities.
    /// </summary>
    public class DetailsController : Controller
    {
        private readonly LibraryDbContext db;

    /// <summary>
    /// Initializes a new instance of the <see cref="DetailsController"/> class.
    /// </summary>
        /// <param name="context">The library database context.</param>
        public DetailsController(LibraryDbContext context)
        {
            this.db = context;
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
        public IActionResult Book(string? bookId)
        {
            if (!int.TryParse(bookId, out var id))
            {
                id = 1;
            }

            var c = this.db.Books.Find(id);
            if (c == null)
            {
                return this.NotFound();
            }

            // log a serialized copy for debugging (kept intentionally simple)
            var serialized = JsonSerializer.Serialize(c);
            Console.WriteLine(serialized);

            this.ViewBag.Title = c.Title;
            this.ViewBag.Description = c.Description;
            this.ViewBag.Year = c.PublicationYear;
            this.ViewBag.Authors = c.AuthorNames;
            this.ViewBag.Count = c.AvailableCopies;
            return this.View();
        }
    }
}
