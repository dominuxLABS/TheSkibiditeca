using Microsoft.AspNetCore.Mvc;

namespace TheSkibiditeca.Web.Controllers {
    public class ListController : Controller {
        public IActionResult Index() {
            return RedirectToAction("Home", "Index");
        }

        public IActionResult Book() {
            return View();
        }
    }
}
