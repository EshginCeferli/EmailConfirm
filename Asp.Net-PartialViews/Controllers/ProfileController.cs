using Asp.Net_PartialViews.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Asp.Net_PartialViews.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)

        {
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var appUser = _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (appUser is null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(appUser);
        }

        public async Task<IActionResult> Edit()
        {
            return View();
        }
    }
}