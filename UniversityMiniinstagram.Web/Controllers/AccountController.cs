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
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using UniversityMiniinstagram.Services.Interfaces;

namespace UniversityMiniinstagram.Web.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(IAccountService accountService, IPostService postService)
        {
            _accountService = accountService;
            _postService = postService;
        }

        IAccountService _accountService;
        IPostService _postService;

        [HttpGet]
        [Authorize]
        public async Task<ViewResult> Profile()
        {
            var result = HttpContext.User.IsAuthenticated();
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                var user = await _accountService.GetUser(userIdClaim.Value);
                var posts = _postService.GetUserPosts(user.Id);
                user.Posts = posts;
                ViewBag.isAdmin = true;
                return View(user);
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<ViewResult> EditProfile()
        {
            var result = HttpContext.User.IsAuthenticated();
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                var user = await _accountService.GetUser(userIdClaim.Value);
                return View(user);
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ProfileNumb(int numb)
        {
            return View(numb);
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
        public IActionResult SetLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMinutes(5) }
            );

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPost(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.Register(model);
                if(result)
                {
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Register");
        }

        [HttpPost]
        public async Task<IActionResult> LoginPost(LoginViewModel vm)
        {
            if (ModelState.IsValid && vm.Email != null && vm.Password != null)
            {
                var result = await _accountService.Login(vm);
                if (result)
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


        [HttpGet]
        public async Task<IActionResult> CreateRolePost()
        {
            var result = await _accountService.AddRole("Admin");

            var result1 = await _accountService.AddRole("User");
            if (result && result1)
            {
                return Ok();
            }
            return BadRequest(result1);
        }

    }
}