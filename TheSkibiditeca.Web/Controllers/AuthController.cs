using Microsoft.AspNetCore.Mvc;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Controllers {
    public class AuthController : Controller {
        private readonly DbContextSqlServer _db;
        public AuthController(DbContextSqlServer db) {
            _db = db;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Register() {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult>Register(RegisterModel model) {
            return this.View(model); 
        }

        public IActionResult Login() {
            return this.View();
        }
    }
}
