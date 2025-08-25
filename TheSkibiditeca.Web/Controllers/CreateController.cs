using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models.Create;
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

        [HttpGet]
        public IActionResult Book() {
            ViewBag.Categories = db.Categories.ToArray();
            ViewBag.Authors = db.Authors.ToArray();
            return View();
        }

        [HttpPost]
        public IActionResult Book(BookCreateModel bmodel) {
            Book nbook = bmodel.book;
            db.Books.Add(nbook);
            db.SaveChanges();
            Book lastAdded = db.Books.OrderBy(e => e.BookId).LastOrDefault();
            for(int i = 0; i < bmodel.Copies; i++) {
                db.Copies.Add(new Copy() {
                    BookId = lastAdded.BookId,
                    ISBN = DbSeeder.GenerateIsbn(),
                    PublisherName = "GenericPublisher",
                    PhysicalLocation = "GenericPosition",
                    IsActive = true
                });
            }

            foreach(string authorId in bmodel.authors) {
                db.BookAuthors.Add(new BookAuthor() {
                    BookId = lastAdded.BookId,
                    AuthorId = int.Parse(authorId),
                    Role = "Writer"
                });
            }

            db.SaveChanges();
            
            return RedirectToAction("Book", "List");
        }
    }
}
