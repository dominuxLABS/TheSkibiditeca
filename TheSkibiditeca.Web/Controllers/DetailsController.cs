using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Controllers {
    public class DetailsController : Controller {
        private readonly DbContextSqlServer _db;
        public DetailsController(DbContextSqlServer db) {
            _db = db;
        }

        public IActionResult Index() {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Book(string bookId) {
            var c = _db.Books.Find(int.Parse(bookId));
            ViewBag.Title = c.Title;
            ViewBag.Description = c.Description;
            ViewBag.Year = c.PublicationYear;
            ViewBag.Authors = c.AuthorNames;
            ViewBag.Count = c.AvailableCopies;
            return View();
        }
    }
}
