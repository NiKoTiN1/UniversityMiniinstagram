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
using System.Security.Principal;

namespace UniversityMiniinstagram.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }

        [HttpGet]
        [Authorize]
        public async Task<ViewResult> Profile()
        {
            var result = HttpContext.User.IsAuthenticated();
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                var userInfo = await UserInfo(userIdClaim.Value);
                if (userInfo != null)
                {
                    ViewBag.isAdmin = await _userManager.IsInRoleAsync(userInfo, "Admin");
                    return View(userInfo);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ProfileNumb(int numb)
        {
            return Ok(numb);
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl = null)
        {
            ViewBag.ReturnUrlParameter = ReturnUrl;
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPost(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser {Email=model.Email, Avatar = model.Avatar, Description = model.Description, UserName=model.Username};
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, model.Role);
                if (result.Succeeded)
                    return RedirectToAction("Login");
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return BadRequest("Some Error");
        }

        [HttpPost]
        public async Task<IActionResult> LoginPost(LoginViewModel vm)
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
                    return RedirectToAction("Profile");
                }
            }
            return RedirectToAction("Login");
        }


        [HttpPost]
        [Route("addrole")]
        public async Task<IActionResult> CreateRolePost()
        {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole("Admin"));

            IdentityResult result1 = await _roleManager.CreateAsync(new IdentityRole("User"));
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                foreach (var error in result1.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return BadRequest(result1);
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