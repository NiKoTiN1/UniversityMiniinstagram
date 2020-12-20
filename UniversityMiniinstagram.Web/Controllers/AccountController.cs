using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Linq;
using UniversityMiniinstagram.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using UniversityMiniinstagram.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using UniversityMiniinstagram.Database.Models;

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
        [Authorize(Roles = "Admin, Modarator, User")]
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
        [Authorize(Roles = "Admin, Modarator, User")]
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
        [Authorize(Roles = "Admin, Modarator, User")]
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
        public IActionResult GoogleLogin()
        {
            var proptities = _accountService.GoogleLogin(Url.Action("GoogleResponse")); 
            return new ChallengeResult("Google", proptities);
        }
            
        public async Task<IActionResult> GoogleResponse(string returnUrl = null, string remoteError = null)
        {
            var info = await _accountService.GetExternalLoginInfoAsync();
            var isExist = await _accountService.ExternalLogin(info);
            if(isExist)
            {
                return RedirectToAction("Profile");
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = await _accountService.GetUserByEmail(email);
                if(user == null)
                {
                    user = new ApplicationUser()
                    {
                        Email = email,
                        UserName = email.Split("@").First()
                    };
                    var result = await _accountService.Register(user);
                    if (!result)
                    {
                        RedirectToAction("Login");
                    }
                    result = await _accountService.AddLoginToUser(user, info);
                    if (!result)
                    {
                        RedirectToAction("Login");
                    }
                    await _accountService.Login(user);
                }
            }
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            await HttpContext.SignOutAsync();
            HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return RedirectToAction("Login");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Modarator, User")]
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
                        return Redirect(vm.returnUrl);
                    }
                    return RedirectToAction("Profile");
                }
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Modarator, User")]
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
                if(vm.Password != null)
                {
                    if(vm.OldPassword == null)
                    {
                        return RedirectToAction("EditProfile");
                    }
                }
                var result = await _accountService.EditProfile(vm);
                if(result)
                {
                    return RedirectToAction("Profile");
                }    
            }
            return RedirectToAction("EditProfile");
        }


        [HttpGet]
        public async Task<IActionResult> CreateRolePost()
        {
            if(!_accountService.IsAdminCreated())
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
                };
                var adminUser = await _accountService.RegisterAdmin(vm);
                if (adminUser)
                {
                    return RedirectToAction("Profile");
                }
            }
            return BadRequest();
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AccessDenied(string ReturnUrl = null)
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
                return RedirectToAction("BanPage");
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Banned")]
        public IActionResult BanPage()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Modarator")]
        public async Task<IActionResult> BanUser(string userId)
        {
            var user = await _accountService.GetUser(userId);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _accountService.SetBanRole(user);
            if(!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Modarator")]
        public async Task<IActionResult> UnBanUser(string userId)
        {
            var user = await _accountService.GetUser(userId);
            if (user == null)
            {
                return BadRequest();
            }
            await _accountService.UnBanUser(user);
            return Ok();
        }
    }
}