
using Microsoft.AspNetCore.Mvc;

namespace Asp.Net_PartialViews.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

    }
}
