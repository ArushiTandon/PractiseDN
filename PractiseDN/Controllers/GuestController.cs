using Microsoft.AspNetCore.Mvc;

namespace PractiseDN.Controllers
{
    public class GuestController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Aboutus()
        {
            return View();
        }

        public IActionResult Contactus()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Gallery()
        {
            return View();
        }
    }
}
