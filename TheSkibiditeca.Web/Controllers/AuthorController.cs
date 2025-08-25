using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models.AuthorModels;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Controllers {
    public class AuthorController : Controller {
        private readonly UserManager<User> _userM;
        private readonly LibraryDbContext _db;

        public AuthorController(UserManager<User> userM, LibraryDbContext db) {
            _userM = userM;
            _db = db;
        }

        public IActionResult Index() {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Create() {
            var user = await _userM.GetUserAsync(HttpContext.User);
            if(user == null) return RedirectToAction("Lost", "Home");
            if(user.UserTypeId < 2) return RedirectToAction("Lost", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorCreateModel model) {
            var user = await _userM.GetUserAsync(HttpContext.User);
            if(user == null) return RedirectToAction("Lost", "Home");
            if(user.UserTypeId < 2) return RedirectToAction("Lost", "Home");

            _db.Authors.Add(model.author);
            _db.SaveChanges();
            return View();
        }
    }
}
