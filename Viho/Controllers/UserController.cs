using Microsoft.AspNetCore.Mvc;

namespace Cuba.Controllers
{
    public class UserController : Controller
    {
        public IActionResult UserProfile()
        {
            return View();
        }
        public IActionResult UserEdit()
        {
            return View();
        }
        public IActionResult UserCards()
        {
            return View();
        }
    }
}
