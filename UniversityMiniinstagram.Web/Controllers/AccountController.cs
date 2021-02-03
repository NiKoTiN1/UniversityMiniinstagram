using AutoMapper;
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
using UniversityMiniinstagram.Services.Attributes;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Web.Controllers
{
    [Route("account")]
    public partial class AccountController : Controller
    {
        public AccountController(IAccountService accountService, IPostService postService, IWebHostEnvironment appEnvironment, IMapper mapper)
        {
            this.accountService = accountService;
            this.postService = postService;
            this.appEnvironment = appEnvironment;
            this.mapper = mapper;
        }
        private readonly IAccountService accountService;
        private readonly IPostService postService;
        private readonly IWebHostEnvironment appEnvironment;
        private readonly IMapper mapper;

        [HttpGet]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator, Roles.User)]
        [Route("profile")]
        public virtual async Task<IActionResult> Profile()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                ApplicationUser user = await this.postService.GetUserPosts(userIdClaim.Value);
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
                EditProfileViewModel model = this.mapper.Map<EditProfileViewModel>(await this.accountService.GetUser(userIdClaim.Value));
                return View(model);
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
            AuthenticationProperties proptities = this.accountService.GoogleLogin(Url.Action("GoogleResponse"));
            return Challenge(proptities, "Google");
        }

        [Route("google-response")]
        public virtual async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await this.accountService.GetExternalLoginInfoAsync();
            return await this.accountService.ExternalLogin(info) ? RedirectToAction(MVC.Account.Profile()) : RedirectToAction(MVC.Account.Login());
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
            await this.accountService.Logout();
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
        [Route("registration")]
        public virtual async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!await this.accountService.Register(model.Email, model.Description, model.Username, model.Password))
            {
                return View();
            }
            return RedirectToAction(MVC.Account.Login());
        }

        [HttpPost]
        [Route("login")]
        public virtual async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await this.accountService.Login(model.Email, model.Password))
                {
                    return model.ReturnUrl != null ? Redirect(model.ReturnUrl) : (IActionResult)RedirectToAction(MVC.Account.Profile());
                }
            }
            return View(model);
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator, Roles.User)]
        [Route("profile/edit")]
        public virtual async Task<IActionResult> EditProfile([FromForm] EditProfileViewModel model)
        {
            string webRootPath = this.appEnvironment.WebRootPath;
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            model.UserId = userIdClaim.Value;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.Password != null && model.OldPassword == null)
            {
                return RedirectToAction(MVC.Account.EditProfile());
            }
            if (await this.accountService.EditProfile(model.Username, model.Description, model.Password, model.UserId, model.OldPassword, model.File, webRootPath))
            {
                return RedirectToAction(MVC.Account.Profile());
            }
            return RedirectToAction(MVC.Account.EditProfile());
        }

        [HttpGet]
        [Authorize]
        [Route("AccessDenied")]
        public virtual async Task<IActionResult> AccessDenied()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            return await this.accountService.IsInRole(userIdClaim.Value, Enum.GetName(typeof(Roles), Roles.User)) ? (IActionResult)View() : RedirectToAction(MVC.Account.BanPage());
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
            return await this.accountService.SetBanRole(userId) ? (IActionResult)Ok() : BadRequest();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("unban")]
        public virtual async Task<IActionResult> UnBanUser(string userId)
        {
            return await this.accountService.UnBanUser(userId) ? Ok() : (IActionResult)BadRequest();
        }
    }
}