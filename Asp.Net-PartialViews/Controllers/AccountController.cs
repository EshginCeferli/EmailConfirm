using Asp.Net_PartialViews.Models;
using Asp.Net_PartialViews.ViewModels.AccountViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace Asp.Net_PartialViews.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            AppUser appUser = new AppUser
            {
                Fullname = registerVM.Fullname,
                Email = registerVM.Email,
                UserName = registerVM.Username
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVM);
            }

            //await _signInManager.SignInAsync(appUser, false);

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

            string link = Url.Action(nameof(ConfirmEmail), "Account", new { userId = appUser.Id, token },
                Request.Scheme, Request.Host.ToString());

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("eshginij@code.edu.az"));
            email.To.Add(MailboxAddress.Parse(appUser.Email));
            email.Subject = "Test Email Subject";
            string body = $"<a href=`{link}`</a>";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("eshginij@code.edu.az", "9829535e#");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction(nameof(VerifyEmail));
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return BadRequest();

            AppUser appUser = await _userManager.FindByIdAsync(userId);

            if (appUser is null ) return NotFound();

            await _userManager.ConfirmEmailAsync(appUser, token);

            await _signInManager.SignInAsync(appUser, false);

            return RedirectToAction("Index", "Home");
            
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.EmailOrUsername);
            if (appUser is null)
            {
                appUser = await _userManager.FindByNameAsync(loginVM.EmailOrUsername);
            }

            if (appUser is null)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(loginVM);
            }


            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
