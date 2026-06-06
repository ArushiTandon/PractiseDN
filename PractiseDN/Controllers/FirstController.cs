using Microsoft.AspNetCore.Mvc;

namespace PractiseDN.Controllers
{
    public class FirstController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
