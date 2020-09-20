using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Linq;
using System.Text;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4.Extensions;

namespace UniversityMiniinstagram.Web.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        [Authorize]
        [Route("profile")]
        public async Task<ViewResult> Profile()
        {
            var result = HttpContext.User.IsAuthenticated();
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                var userInfo = await UserInfo(userIdClaim.Value);
                if (userInfo != null)
                    return View(userInfo);
            }
            return View();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser {Email=model.Email, Avatar = model.Avatar, Description = model.Description, UserName=model.Username};

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                    return Ok(model);
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return BadRequest("Some Error");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string ReturnUrl = null)
        {
            ViewBag.ReturnUrlParameter = ReturnUrl;
            return View();
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid && vm != null)
            {
                var user = await ValidateUser(vm);
                if (user != null)
                {
                    if (vm.returnUrl != null)
                    {
                        return Redirect("https://localhost:5001" + vm.returnUrl);
                    }
                    return Redirect("https://localhost:5001/api/account/profile");
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return Redirect("https://localhost:5001/api/Account/login");
        }

        private async Task<ApplicationUser> ValidateUser(LoginViewModel vm)
        {
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user != null)
            {
                var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, vm.Password);
                if (result != PasswordVerificationResult.Failed)
                {
                    await _signInManager.SignInAsync(user, false);
                    return user;
                }
            }
            return null;
        }

        private async Task<ApplicationUser> UserInfo(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }
    }
}