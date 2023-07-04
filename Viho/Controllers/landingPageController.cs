using Microsoft.AspNetCore.Mvc;

namespace Viho.Controllers
{
    public class landingPageController : Controller
    {
        public IActionResult landingPages()
        {
            return View();
        }
    }
}
