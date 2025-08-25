using Microsoft.AspNetCore.Mvc;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Controllers {
    public class CreateController : Controller {
        private readonly LibraryDbContext db;
        public CreateController(LibraryDbContext db ) {
            this.db = db;
        }

        public IActionResult Index() {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Book() {
            ViewBag.Categories = db.Categories.ToArray();
            ViewBag.Authors = db.Authors.ToArray();
            return View();
        }
    }
}
