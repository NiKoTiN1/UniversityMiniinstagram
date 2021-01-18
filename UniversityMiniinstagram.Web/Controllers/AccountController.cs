﻿using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public async Task<ViewResult> Profile()
        {
            var result = HttpContext.User.IsAuthenticated();
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                ApplicationUser user = await this.AccountService.GetUser(userIdClaim.Value);
                ICollection<Post> posts = await this.PostService.GetUserPosts(user.Id);
                user.Posts = posts;
                ViewBag.isAdmin = true;
                return View(user);
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator, User")]
        [Route("profile/edit")]
        public async Task<ViewResult> EditProfile()
        {
            var result = HttpContext.User.IsAuthenticated();
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
            Microsoft.AspNetCore.Identity.ExternalLoginInfo info = await this.AccountService.GetExternalLoginInfoAsync();
            var isExist = await this.AccountService.ExternalLogin(info);
            if (isExist)
            {
                return RedirectToAction("Profile");
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                ApplicationUser user = await this.AccountService.GetUserByEmail(email);
                if (user == null)
                {
                    user = new ApplicationUser()
                    {
                        Email = email,
                        UserName = email.Split("@").First()
                    };
                    var result = await this.AccountService.Register(user);
                    if (!result)
                    {
                        RedirectToAction("Login");
                    }
                    result = await this.AccountService.AddLoginToUser(user, info);
                    if (!result)
                    {
                        RedirectToAction("Login");
                    }
                    await this.AccountService.Login(user);
                }
            }
            return RedirectToAction("Profile");
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
                var result = await this.AccountService.Register(model);
                if (result)
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
                var result = await this.AccountService.Login(vm);
                if (result)
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
            if (userIdClaim.Value == null)
            {
                return Unauthorized();
            }
            vm.UserId = userIdClaim.Value;
            if (ModelState.IsValid && vm.Username != null)
            {
                if (vm.Password != null)
                {
                    if (vm.OldPassword == null)
                    {
                        return RedirectToAction("EditProfile");
                    }
                }
                var result = await this.AccountService.EditProfile(vm);
                if (result)
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
            if (!this.AccountService.IsAdminCreated())
            {
                await this.AccountService.AddRole("Admin");
                await this.AccountService.AddRole("User");
                await this.AccountService.AddRole("Moderator");
                await this.AccountService.AddRole("Banned");

                var vm = new RegisterViewModel()
                {
                    Email = "Admin@mail.ru",
                    Description = "AdminAcc",
                    Password = "Admin_1",
                    Username = "Admin",
                };
                var adminUser = await this.AccountService.RegisterAdmin(vm);
                if (adminUser)
                {
                    return RedirectToAction("Profile");
                }
            }
            return BadRequest();
        }


        [HttpGet]
        [Authorize]
        [Route("AccessDenied")]
        public async Task<IActionResult> AccessDenied()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            ApplicationUser user = await this.AccountService.GetUser(userIdClaim.Value);
            var isBanned = !await this.AccountService.IsInRole(user, "User");
            return isBanned ? RedirectToAction("BanPage") : (IActionResult)View();
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
            ApplicationUser user = await this.AccountService.GetUser(userId);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await this.AccountService.SetBanRole(user);
            return !result ? BadRequest() : (IActionResult)Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [Route("unban")]
        public async Task<IActionResult> UnBanUser(string userId)
        {
            ApplicationUser user = await this.AccountService.GetUser(userId);
            if (user == null)
            {
                return BadRequest();
            }
            await this.AccountService.UnBanUser(user);
            return Ok();
        }
    }
}