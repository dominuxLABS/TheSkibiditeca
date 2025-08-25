using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheSkibiditeca.Web.Models.AuthorModels;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Controllers {
    public class AuthorController : Controller {
        private readonly UserManager<User> _userM;

        public AuthorController(UserManager<User> userM) {
            _userM = userM;
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


            return View();
        }
    }
}
