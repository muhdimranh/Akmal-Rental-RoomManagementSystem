using Microsoft.AspNetCore.Mvc;

namespace Cuba.Controllers
{
    public class Page_LayoutController : Controller
    {
        public IActionResult BoxLayout()
        {
            ViewBag.FullName = HttpContext.Session.GetString("FullName");
            return View();
        }
        public IActionResult RTL()
        {
            return View();
        }
        public IActionResult Dark()
        {
            return View();
        }
        public IActionResult FooterLight()
        {
            return View();
        }
        public IActionResult FooterDark()
        {
            return View();
        }
        public IActionResult FooterFixed()
        {
            return View();
        }
    }
}
