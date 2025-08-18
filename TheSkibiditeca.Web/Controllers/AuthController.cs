using Microsoft.AspNetCore.Mvc;

namespace TheSkibiditeca.Web.Controllers {
    public class AuthController : Controller {
        public IActionResult Index() {
            return View();
        }

        public IActionResult Register() {
            return this.View();
        }

        public IActionResult Login() {
            return this.View();
        }
    }
}
