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
using Microsoft.AspNetCore.Hosting;

namespace UniversityMiniinstagram.Web.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(IAccountService accountService, IPostService postService, IWebHostEnvironment appEnvironment)
        {
            _accountService = accountService;
            _postService = postService;
            _appEnvironment = appEnvironment;
        }

        IAccountService _accountService;
        IPostService _postService;
        IWebHostEnvironment _appEnvironment;

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

        [HttpPost]
        public async Task<IActionResult> EditProfilePost([FromForm] EditProfileViewModel vm)
        {
            vm.WebRootPath = _appEnvironment.WebRootPath;
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if(userIdClaim.Value == null)
            {
                return Unauthorized();
            }
            vm.Userid = userIdClaim.Value;
            if (ModelState.IsValid && vm.Username != null)
            {
                var result = await _accountService.EditProfile(vm);
                if(result)
                {
                    return RedirectToAction("Profile");
                }    
            }
            return RedirectToAction("EditProfilePost");
        }


        [HttpGet]
        public async Task<IActionResult> CreateRolePost()
        {
            var result = await _accountService.AddRole("Admin");

            var result1 = await _accountService.AddRole("User");
            var result2 = await _accountService.AddRole("Modarator");
            var result3 = await _accountService.AddRole("Banned");

            RegisterViewModel vm = new RegisterViewModel()
            {
                Email = "Admin@mail.ru",
                Description = "AdminAcc",
                Password = "Admin_1",
                Username = "Admin",
                Role = "Admin"
            };
            var adminUser = await _accountService.Register(vm);
            if (adminUser)
            {
                return RedirectToAction("Profile");
            }
            return BadRequest();
        }


        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            var user = await _accountService.GetUser(userIdClaim.Value);
            IsInRoleViewModel vm = new IsInRoleViewModel()
            { 
                user = user,
                roleName = "User"
            };
            var isBanned = !await _accountService.IsInRole(vm);
            if (isBanned)
            {
                return RedirectToAction("GetAllPosts", "News");
            }
            return BadRequest();
        }

    }
}