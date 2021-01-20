using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Web.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        public AccountController(IAccountService accountService, IPostService postService, IWebHostEnvironment appEnvironment)
        {
            this.AccountService = accountService;
            this.PostService = postService;
            this.AppEnvironment = appEnvironment;
        }

        private readonly IAccountService AccountService;
        private readonly IPostService PostService;
        private readonly IWebHostEnvironment AppEnvironment;

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator, User")]
        [Route("profile")]
        public async Task<IActionResult> Profile()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                ApplicationUser user = await this.PostService.GetUserPosts(userIdClaim.Value);
                return View(user);
            }
            return RedirectToAction("GetAllPosts", "News");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator, User")]
        [Route("profile/edit")]
        public async Task<ViewResult> EditProfile()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                ApplicationUser user = await this.AccountService.GetUser(userIdClaim.Value);
                return View(user);
            }
            return View();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string ReturnUrl = null)
        {
            ViewBag.ReturnUrlParameter = ReturnUrl;
            return View();
        }

        [HttpGet]
        [Route("google-login")]
        public IActionResult GoogleLogin()
        {
            AuthenticationProperties proptities = this.AccountService.GoogleLogin(Url.Action("GoogleResponse"));
            return new ChallengeResult("Google", proptities);
        }

        [Route("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await this.AccountService.GetExternalLoginInfoAsync();
            return await this.AccountService.ExternalLogin(info) ? RedirectToAction("Profile") : RedirectToAction("Login");
        }

        [HttpGet]
        [Route("registration")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await this.AccountService.Logout();
            await HttpContext.SignOutAsync();
            HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return RedirectToAction("Login");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator, User")]
        [Route("set-language")]
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
        [Route("registered")]
        public async Task<IActionResult> RegistrationPost(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await this.AccountService.Register(model))
                {
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Register");
        }

        [HttpPost]
        [Route("authorized")]
        public async Task<IActionResult> LoginPost(LoginViewModel vm)
        {
            if (ModelState.IsValid && vm.Email != null && vm.Password != null)
            {
                if (await this.AccountService.Login(vm))
                {
                    return vm.ReturnUrl != null ? Redirect(vm.ReturnUrl) : (IActionResult)RedirectToAction("Profile");
                }
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator, User")]
        [Route("profile/edited")]
        public async Task<IActionResult> EditProfilePost([FromForm] EditProfileViewModel vm)
        {
            vm.WebRootPath = this.AppEnvironment.WebRootPath;
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            vm.UserId = userIdClaim.Value;
            if (ModelState.IsValid && vm.Username != null)
            {
                if (vm.Password != null && vm.OldPassword == null)
                {
                    return RedirectToAction("EditProfile");
                }
                if (await this.AccountService.EditProfile(vm))
                {
                    return RedirectToAction("Profile");
                }
            }
            return RedirectToAction("EditProfile");
        }


        [HttpGet]
        [Route("add-role")]
        public async Task<IActionResult> CreateRolePost()
        {
            return await this.AccountService.CreateAdmin() ? BadRequest() : (IActionResult)RedirectToAction("Profile");
        }


        [HttpGet]
        [Authorize]
        [Route("AccessDenied")]
        public async Task<IActionResult> AccessDenied()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            return await this.AccountService.IsInRole(userIdClaim.Value, "User") ? (IActionResult)View() : RedirectToAction("BanPage");
        }

        [HttpGet]
        [Authorize(Roles = "Banned")]
        [Route("banned")]
        public IActionResult BanPage()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [Route("ban")]
        public async Task<IActionResult> BanUser(string userId)
        {
            return await this.AccountService.SetBanRole(userId) ? (IActionResult)Ok() : BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [Route("unban")]
        public async Task<IActionResult> UnBanUser(string userId)
        {
            return await this.AccountService.UnBanUser(userId) ? Ok() : (IActionResult)BadRequest();
        }
    }
}