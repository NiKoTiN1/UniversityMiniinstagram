using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Constants;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;

namespace UniversityMiniinstagram.Services
{
    public class AccountService : IAccountService
    {
        public AccountService(IAccountRepository accountReposetry, IImageService imageService)
        {
            this.AccountReposetry = accountReposetry;
            this.ImageService = imageService;
        }

        private readonly IAccountRepository AccountReposetry;
        private readonly IImageService ImageService;

        public async Task<bool> Register(string email, string description, string username, string password)
        {
            if (!await this.AccountReposetry.IsExist(email))
            {
                return false;
            }
            var user = new ApplicationUser { Email = email, Description = description, UserName = username, AvatarId = new Guid().ToString() };
            if (!await this.AccountReposetry.CreateUser(user, password))
            {
                return false;
            }
            if (!await this.AccountReposetry.AddRoleToUser(user, Enum.GetName(typeof(Roles), Roles.User)))
            {
                await this.AccountReposetry.RemoveUser(user.Id);
                return false;
            }
            return true;
        }
        public async Task<bool> Register(ApplicationUser user)
        {
            user.AvatarId = new Guid().ToString();
            if (!await this.AccountReposetry.CreateUser(user))
            {
                return false;
            }
            if (!await this.AccountReposetry.AddRoleToUser(user, Enum.GetName(typeof(Roles), Roles.User)))
            {
                await this.AccountReposetry.RemoveUser(user.Id);
                return false;
            }
            return true;
        }

        public async Task<bool> IsInRole(string userId, string roleName, ApplicationUser user = null)
        {
            if (user == null)
            {
                user = await this.AccountReposetry.GetUser(userId);
            }
            return await this.AccountReposetry.IsInRole(user, roleName);
        }

        public async Task<bool> SetBanRole(string userId, ApplicationUser user = null)
        {
            if (user == null)
            {
                user = await this.AccountReposetry.GetUser(userId);
            }
            var isBanned = await this.AccountReposetry.IsInRole(user, Enum.GetName(typeof(Roles), Roles.Banned));
            if (isBanned)
            {
                return true;
            }
            IList<string> roleList = await this.AccountReposetry.GetRoleList(user);
            return await this.AccountReposetry.RemoveRolesFromUser(user, roleList) ? await this.AccountReposetry.AddRoleToUser(user, Enum.GetName(typeof(Roles), Roles.Banned)) : false;
        }

        public async Task Logout()
        {
            await this.AccountReposetry.Logout();
        }

        public async Task<bool> UnBanUser(string userId)
        {
            ApplicationUser user = await this.AccountReposetry.GetUser(userId);
            return await this.AccountReposetry.UnBanUser(user);
        }
        public async Task<bool> Login(string email, string password)
        {
            return await this.AccountReposetry.Login(email, password);
        }
        public AuthenticationProperties GoogleLogin(string url)
        {
            return this.AccountReposetry.GoogleLogin(url);
        }
        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await this.AccountReposetry.GetExternalLoginInfoAsync();
        }
        public async Task<bool> ExternalLogin(ExternalLoginInfo info)
        {
            if (!await this.AccountReposetry.ExternalLogin(info))
            {
                return false;
            }
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await this.AccountReposetry.GetUserByEmail(email);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    Email = email,
                    UserName = email.Split("@").First()
                };
                if (!await Register(user) || !await this.AccountReposetry.AddLoginToUser(user, info))
                {
                    return false;
                }
            }
            await this.AccountReposetry.Login(user);
            return true;
        }

        public async Task<ApplicationUser> GetUser(string userId)
        {
            return await this.AccountReposetry.GetUser(userId);
        }

        public async Task<bool> EditProfile(string username, string description, string password, string userId, string oldPassword, IFormFile file, string webRootPath)
        {
            Image image = null;
            ApplicationUser Ouser = await this.AccountReposetry.GetUser(userId);
            if (password != null)
            {
                if (!await this.AccountReposetry.ChangePassword(Ouser, oldPassword, password))
                {
                    return false;
                }
            }
            if (file != null)
            {
                image = await this.ImageService.Add(file, webRootPath);
            }
            var user = new ApplicationUser()
            {
                Id = userId,
                Avatar = image,
                Description = description,
                UserName = username
            };
            return await this.AccountReposetry.UpdateUser(Ouser, user);
        }

        public async Task<ICollection<string>> GetUserRoles(ApplicationUser user)
        {
            return await this.AccountReposetry.GetRoleList(user);
        }

        public IList<ApplicationUser> GetAllUsers()
        {
            return this.AccountReposetry.GetAllUsers();
        }

        public async Task<bool> SetModerator(ApplicationUser user)
        {
            return await this.AccountReposetry.AddRoleToUser(user, Enum.GetName(typeof(Roles), Roles.Moderator));
        }

        public async Task<bool> SetNonModerator(ApplicationUser user)
        {
            var roles = new List<string> { Enum.GetName(typeof(Roles), Roles.Moderator) };
            return await this.AccountReposetry.RemoveRolesFromUser(user, roles);
        }
    }
}
