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
using UniversityMiniinstagram.Database.Constants;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Attrebutes;
using UniversityMiniinstagram.Services.Interfaces;

namespace UniversityMiniinstagram.Web.Controllers
{
    [Route("account")]
    public partial class AccountController : Controller
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
        [AuthorizeEnum(Roles.Admin, Roles.Moderator, Roles.User)]
        [Route("profile")]
        public virtual async Task<IActionResult> Profile()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                ApplicationUser user = await this.PostService.GetUserPosts(userIdClaim.Value);
                return View(user);
            }
            return RedirectToAction(MVC.News.GetAllPosts());
        }

        [HttpGet]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator, Roles.User)]
        [Route("profile/edit")]
        public virtual async Task<IActionResult> EditProfile()
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
        public virtual IActionResult Login(string ReturnUrl = null)
        {
            ViewBag.ReturnUrlParameter = ReturnUrl;
            return View();
        }

        [HttpGet]
        [Route("google-login")]
        public virtual IActionResult GoogleLogin()
        {
            AuthenticationProperties proptities = this.AccountService.GoogleLogin(Url.Action("GoogleResponse"));
            return new ChallengeResult("Google", proptities);
        }

        [Route("google-response")]
        public virtual async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await this.AccountService.GetExternalLoginInfoAsync();
            return await this.AccountService.ExternalLogin(info) ? RedirectToAction(MVC.Account.Profile()) : RedirectToAction(MVC.Account.Login());
        }

        [HttpGet]
        [Route("registration")]
        public virtual IActionResult Registration()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [Route("logout")]
        public virtual async Task<IActionResult> Logout()
        {
            await this.AccountService.Logout();
            await HttpContext.SignOutAsync();
            HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return RedirectToAction(MVC.Account.Login());
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator, Roles.User)]
        [Route("set-language")]
        public virtual IActionResult SetLanguage(string culture)
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
        public virtual async Task<IActionResult> RegistrationPost(string email, string username, string password, string description = null)
        {
            if (ModelState.IsValid)
            {
                if (await this.AccountService.Register(email, description, username, password))
                {
                    return RedirectToAction(MVC.Account.Login());
                }
            }
            return RedirectToAction("Registration");
        }

        [HttpPost]
        [Route("authorized")]
        public virtual async Task<IActionResult> LoginPost(string email, string password, string returnUrl = null)
        {
            if (ModelState.IsValid && email != null && password != null)
            {
                if (await this.AccountService.Login(email, password))
                {
                    return returnUrl != null ? Redirect(returnUrl) : (IActionResult)RedirectToAction(MVC.Account.Profile());
                }
            }
            return RedirectToAction(MVC.Account.Login());
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator, Roles.User)]
        [Route("profile/edited")]
        public virtual async Task<IActionResult> EditProfilePost([FromForm] string username, string userId, string description = null, string password = null, string oldPassword = null, IFormFile file = null)
        {
            var webRootPath = this.AppEnvironment.WebRootPath;
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            userId = userIdClaim.Value;
            if (ModelState.IsValid && username != null)
            {
                if (password != null && oldPassword == null)
                {
                    return RedirectToAction(MVC.Account.EditProfile());
                }
                if (await this.AccountService.EditProfile(username, description, password, userId, oldPassword, file, webRootPath))
                {
                    return RedirectToAction(MVC.Account.Profile());
                }
            }
            return RedirectToAction(MVC.Account.EditProfile());
        }

        [HttpGet]
        [Authorize]
        [Route("AccessDenied")]
        public virtual async Task<IActionResult> AccessDenied()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            return await this.AccountService.IsInRole(userIdClaim.Value, Enum.GetName(typeof(Roles), Roles.User)) ? (IActionResult)View() : RedirectToAction(MVC.Account.BanPage());
        }

        [HttpGet]
        [AuthorizeEnum(Roles.Banned)]
        [Route("banned")]
        public virtual IActionResult BanPage()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("ban")]
        public virtual async Task<IActionResult> BanUser(string userId)
        {
            return await this.AccountService.SetBanRole(userId) ? (IActionResult)Ok() : BadRequest();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("unban")]
        public virtual async Task<IActionResult> UnBanUser(string userId)
        {
            return await this.AccountService.UnBanUser(userId) ? Ok() : (IActionResult)BadRequest();
        }
    }
}