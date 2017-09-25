using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Services;
using WebMVC.ViewModels.Account;

namespace WebMVC.Controllers
{
    public class AccountController : Controller
    {
        #region | Fields

        private readonly IIdentityService _identityService;

        #endregion

        #region | Constructors

        public AccountController(
            IIdentityService identityService)
        {
            _identityService = identityService;
        }

        #endregion

        #region | Public Methods

        #region | GET: /Login

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var vm = new LoginViewModel();
            ViewData["ReturnUrl"] = returnUrl;

            return View(vm);
        }

        #endregion

        #region | POST: /Login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tokenResponse = await _identityService.Token(model.Email, model.Password);

                if (tokenResponse == null)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
                else
                {
                    var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenResponse.Token);
                    var principal = new ClaimsPrincipal();
                    principal.AddIdentity(new ClaimsIdentity(
                        jwtToken.Claims.Union(
                        new List<Claim>() { new Claim("access_token", tokenResponse.Token) })
                    ));

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                        new AuthenticationProperties(new Dictionary<string, string>(){{"token_expires", tokenResponse.Expiration.ToString("O")}}));
                }
            }

            // something went wrong, show form with error
            var vm = new LoginViewModel() { Email = model.Email };
            ViewData["ReturnUrl"] = model.ReturnUrl;
            return View(vm);
        }

        #endregion

        #region | POST: /Signout

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        #endregion

        #endregion
    }
}