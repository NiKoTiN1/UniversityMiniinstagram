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
            if (await this.AccountReposetry.IsExist(email).ConfigureAwait(false))
            {
                return false;
            }
            var user = new ApplicationUser { Email = email, Description = description, UserName = username, AvatarId = new Guid().ToString() };
            if (!await this.AccountReposetry.CreateUser(user, password).ConfigureAwait(false))
            {
                return false;
            }
            if (!await this.AccountReposetry.AddRoleToUser(user, Enum.GetName(typeof(Roles), Roles.User)).ConfigureAwait(false))
            {
                await this.AccountReposetry.RemoveUser(user.Id).ConfigureAwait(false);
                return false;
            }
            return true;
        }
        public async Task<bool> Register(ApplicationUser user)
        {
            user.AvatarId = new Guid().ToString();
            if (!await this.AccountReposetry.CreateUser(user).ConfigureAwait(false))
            {
                return false;
            }
            if (!await this.AccountReposetry.AddRoleToUser(user, Enum.GetName(typeof(Roles), Roles.User)).ConfigureAwait(false))
            {
                await this.AccountReposetry.RemoveUser(user.Id).ConfigureAwait(false);
                return false;
            }
            return true;
        }

        public async Task<bool> IsInRole(string userId, string roleName, ApplicationUser user = null)
        {
            if (user == null)
            {
                user = await this.AccountReposetry.GetUser(userId).ConfigureAwait(false);
            }
            return await this.AccountReposetry.IsInRole(user, roleName).ConfigureAwait(false);
        }

        public async Task<bool> SetBanRole(string userId, ApplicationUser user = null)
        {
            if (user == null)
            {
                user = await this.AccountReposetry.GetUser(userId).ConfigureAwait(false);
            }
            var isBanned = await this.AccountReposetry.IsInRole(user, Enum.GetName(typeof(Roles), Roles.Banned)).ConfigureAwait(false);
            if (isBanned)
            {
                return true;
            }
            IList<string> roleList = await this.AccountReposetry.GetRoleList(user).ConfigureAwait(false);
            return await this.AccountReposetry.RemoveRolesFromUser(user, roleList).ConfigureAwait(false) ? await this.AccountReposetry.AddRoleToUser(user, Enum.GetName(typeof(Roles), Roles.Banned)).ConfigureAwait(false) : false;
        }

        public async Task Logout()
        {
            await this.AccountReposetry.Logout().ConfigureAwait(false);
        }

        public async Task<bool> UnBanUser(string userId)
        {
            ApplicationUser user = await this.AccountReposetry.GetUser(userId);
            return await this.AccountReposetry.UnBanUser(user).ConfigureAwait(false);
        }
        public async Task<bool> Login(string email, string password)
        {
            return await this.AccountReposetry.Login(email, password).ConfigureAwait(false);
        }
        public AuthenticationProperties GoogleLogin(string url)
        {
            return this.AccountReposetry.GoogleLogin(url);
        }
        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await this.AccountReposetry.GetExternalLoginInfoAsync().ConfigureAwait(false);
        }
        public async Task<bool> ExternalLogin(ExternalLoginInfo info)
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await this.AccountReposetry.GetUserByEmail(email).ConfigureAwait(false);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    Email = email,
                    UserName = email.Split("@").First()
                };
                if (!await Register(user).ConfigureAwait(false) || !await this.AccountReposetry.AddLoginToUser(user, info).ConfigureAwait(false))
                {
                    return false;
                }
            }
            if (!await this.AccountReposetry.ExternalLogin(info).ConfigureAwait(false))
            {
                return false;
            }
            await this.AccountReposetry.Login(user).ConfigureAwait(false);
            return true;
        }

        public async Task<ApplicationUser> GetUser(string userId)
        {
            return await this.AccountReposetry.GetUser(userId).ConfigureAwait(false);
        }

        public async Task<bool> EditProfile(string username, string description, string password, string userId, string oldPassword, IFormFile file, string webRootPath)
        {
            Image image = null;
            ApplicationUser Ouser = await this.AccountReposetry.GetUser(userId).ConfigureAwait(false);
            if (password != null)
            {
                if (!await this.AccountReposetry.ChangePassword(Ouser, oldPassword, password).ConfigureAwait(false))
                {
                    return false;
                }
            }
            if (file != null)
            {
                image = await this.ImageService.Add(file, webRootPath).ConfigureAwait(false);
            }
            var user = new ApplicationUser()
            {
                Id = userId,
                Avatar = image,
                Description = description,
                UserName = username
            };
            return await this.AccountReposetry.UpdateUser(Ouser, user).ConfigureAwait(false);
        }

        public async Task<ICollection<string>> GetUserRoles(ApplicationUser user)
        {
            return await this.AccountReposetry.GetRoleList(user).ConfigureAwait(false);
        }

        public IList<ApplicationUser> GetAllUsers()
        {
            return this.AccountReposetry.GetAllUsers();
        }

        public async Task<bool> SetModerator(ApplicationUser user)
        {
            return await this.AccountReposetry.AddRoleToUser(user, Enum.GetName(typeof(Roles), Roles.Moderator)).ConfigureAwait(false);
        }

        public async Task<bool> SetNonModerator(ApplicationUser user)
        {
            var roles = new List<string> { Enum.GetName(typeof(Roles), Roles.Moderator) };
            return await this.AccountReposetry.RemoveRolesFromUser(user, roles).ConfigureAwait(false);
        }
    }
}
