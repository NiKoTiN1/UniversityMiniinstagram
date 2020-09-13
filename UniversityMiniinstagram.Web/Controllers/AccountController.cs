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
        public async Task<IActionResult> Profile()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == "UserId");
            if (userIdClaim != null)
            {
                var userInfo = await UserInfo(userIdClaim.Value);
                if (userInfo != null)
                    return Ok(userInfo);
            }
            return Unauthorized();
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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid && vm != null)
            {
                var user = await ValidateUser(vm);
                if (user != null)
                {
                    return Ok(new { Token = GenerateToken(user), Message = "Success" });
                }
            }
            return BadRequest(new { Message = "Login faild" });
        }

        public IActionResult Logout()
        {
            return Ok();
        }

        private async Task<ApplicationUser> ValidateUser(LoginViewModel vm)
        {
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user != null)
            {
                var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, vm.Password);
                if (result != PasswordVerificationResult.Failed)
                {
                    return user;
                }
            }
            return null;
        }

        private string GenerateToken(ApplicationUser user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.UserData, user.UserName),
                    new Claim("UserId", user.Id)
                }),
                Expires = DateTime.UtcNow.AddMinutes(AuthOptions.LIFETIME),
                SigningCredentials = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature),
                Audience = AuthOptions.AUDIENCE,
                Issuer = AuthOptions.ISSUER,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<ApplicationUser> UserInfo(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }
    }
}