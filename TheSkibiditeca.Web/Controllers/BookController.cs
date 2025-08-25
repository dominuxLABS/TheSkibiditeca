using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models;
using TheSkibiditeca.Web.Models.BookModels;
using TheSkibiditeca.Web.Models.Entities;
using TheSkibiditeca.Web.Models.ModelPartial;

namespace TheSkibiditeca.Web.Controllers {

    public class BookController : Controller {
        private readonly LibraryDbContext db;
        private readonly UserManager<User> _userM;
        private readonly ShoppingCart carro;
        public BookController(LibraryDbContext dbo, UserManager<User> user, ShoppingCart cart) {
            db = dbo;
            _userM = user;
            carro = cart;
        }
        public IActionResult Index() {
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Create() {
            var user = await _userM.GetUserAsync(HttpContext.User);
            if(user == null) return RedirectToAction("Lost", "Home");
            if(user.UserTypeId < 2) return RedirectToAction("Lost", "Home");

            ViewBag.Categories = db.Categories.ToArray();
            ViewBag.Authors = db.Authors.ToArray();
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookCreateModel bmodel) {
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

        public async Task<IActionResult> List(string? searchStr, int page = 1, int pageSize = 30) {
            var allBooks = new List<BookMiniCardModel>();
            foreach(Book b in db.Books) {
                allBooks.Add(new BookMiniCardModel() {
                    BookID = b.BookId.ToString(),
                    Title = b.Title,
                    ImageURL = b.CoverImageUrl
                });
            }

            if(!String.IsNullOrEmpty(searchStr)) {
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

        public async Task<IActionResult> Details(string? bookId) {
            if(!int.TryParse(bookId, out var id)) {
                id = 1;
            }

            var user = await _userM.GetUserAsync(HttpContext.User);
            if(user != null) { ViewBag.RoleID = user.UserTypeId; }

            var c = this.db.Books.Find(id);
            if(c == null) {
                return this.NotFound();
            }

            // log a serialized copy for debugging (kept intentionally simple)
            var authorsIds = this.db.BookAuthors.Where(e => e.BookId == c.BookId).Select(e => e.AuthorId);
            var authorNames = this.db.Authors.Where(e => authorsIds.Contains(e.AuthorId)).Select(e => e.FullName);

            this.ViewBag.ID = bookId;
            this.ViewBag.Title = c.Title;
            this.ViewBag.Description = c.Description;
            this.ViewBag.Year = c.PublicationYear;
            this.ViewBag.Authors = string.Join(",", authorNames);
            this.ViewBag.Count = this.db.Copies.Where(e => e.BookId == c.BookId && e.IsActive == true).Count();
            this.ViewBag.URL = c.CoverImageUrl;
            return this.View();
        }

        public async Task<IActionResult> AddCart(string bookId, bool? once = false) {
            var aviableCopy = db.Copies.Where(e => e.BookId == int.Parse(bookId));
            if(carro.copies.Count >= aviableCopy.Count()) return RedirectToAction("Details", "Book", new { bookId });
            if(aviableCopy != null) {
                carro.copies.Add(aviableCopy.First());
            }

            if((bool)once) return RedirectToAction("Create", "Loan");
            return RedirectToAction("Details", "Book", new { bookId });
        }

        public async Task<IActionResult> RemoveCart(string bookId) {
            if(carro.copies.Count == 0) return RedirectToAction("Create", "Loan");
            var copiesIncar = carro.copies.Where(e => e.BookId == int.Parse(bookId)).First();
            if(copiesIncar != null) {
                carro.copies.Remove(copiesIncar);
            }

            return RedirectToAction("Create", "Loan");
        }
    }
}
