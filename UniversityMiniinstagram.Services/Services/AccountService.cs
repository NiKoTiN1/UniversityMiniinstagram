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
            this.accountReposetry = accountReposetry;
            this.imageService = imageService;
        }

        private readonly IAccountRepository accountReposetry;
        private readonly IImageService imageService;

        public async Task<bool> Register(string email, string description, string username, string password)
        {
            if (await this.accountReposetry.IsExist(email).ConfigureAwait(false))
            {
                return false;
            }
            var user = new ApplicationUser { Email = email, Description = description, UserName = username, AvatarId = new Guid().ToString() };
            if (!await this.accountReposetry.CreateUser(user, password).ConfigureAwait(false))
            {
                return false;
            }
            if (!await this.accountReposetry.AddRoleToUser(user, Enum.GetName(typeof(Roles), Roles.User)).ConfigureAwait(false))
            {
                await this.accountReposetry.RemoveUser(user.Id).ConfigureAwait(false);
                return false;
            }
            return true;
        }

        public async Task<bool> Register(ApplicationUser user)
        {
            user.AvatarId = new Guid().ToString();
            if (!await this.accountReposetry.CreateUser(user).ConfigureAwait(false))
            {
                return false;
            }
            if (!await this.accountReposetry.AddRoleToUser(user, Enum.GetName(typeof(Roles), Roles.User)).ConfigureAwait(false))
            {
                await this.accountReposetry.RemoveUser(user.Id).ConfigureAwait(false);
                return false;
            }
            return true;
        }

        public async Task<bool> IsInRole(string userId, string roleName, ApplicationUser user = null)
        {
            if (user == null)
            {
                user = await this.accountReposetry.GetUser(userId).ConfigureAwait(false);
            }
            return await this.accountReposetry.IsInRole(user, roleName).ConfigureAwait(false);
        }

        public async Task<bool> SetBanRole(string userId, ApplicationUser user = null)
        {
            if (user == null)
            {
                user = await this.accountReposetry.GetUser(userId).ConfigureAwait(false);
            }
            bool isBanned = await this.accountReposetry.IsInRole(user, Enum.GetName(typeof(Roles), Roles.Banned)).ConfigureAwait(false);
            if (isBanned)
            {
                return true;
            }
            IList<string> roleList = await this.accountReposetry.GetRoleList(user).ConfigureAwait(false);
            return await this.accountReposetry.RemoveRolesFromUser(user, roleList).ConfigureAwait(false) ? await this.accountReposetry.AddRoleToUser(user, Enum.GetName(typeof(Roles), Roles.Banned)).ConfigureAwait(false) : false;
        }

        public async Task Logout()
        {
            await this.accountReposetry.Logout().ConfigureAwait(false);
        }

        public async Task<bool> UnBanUser(string userId)
        {
            ApplicationUser user = await this.accountReposetry.GetUser(userId);
            return await this.accountReposetry.UnBanUser(user).ConfigureAwait(false);
        }

        public async Task<bool> Login(string email, string password)
        {
            return await this.accountReposetry.Login(email, password).ConfigureAwait(false);
        }

        public AuthenticationProperties GoogleLogin(string url)
        {
            return this.accountReposetry.GoogleLogin(url);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await this.accountReposetry.GetExternalLoginInfoAsync().ConfigureAwait(false);
        }

        public async Task<bool> ExternalLogin(ExternalLoginInfo info)
        {
            string email = info.Principal.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await this.accountReposetry.GetUserByEmail(email).ConfigureAwait(false);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    Email = email,
                    UserName = email.Split("@").First()
                };
                if (!await Register(user).ConfigureAwait(false) || !await this.accountReposetry.AddLoginToUser(user, info).ConfigureAwait(false))
                {
                    return false;
                }
            }
            if (!await this.accountReposetry.ExternalLogin(info).ConfigureAwait(false))
            {
                return false;
            }
            await this.accountReposetry.Login(user).ConfigureAwait(false);
            return true;
        }

        public async Task<ApplicationUser> GetUser(string userId)
        {
            return await this.accountReposetry.GetUser(userId).ConfigureAwait(false);
        }

        public async Task<bool> EditProfile(string username, string description, string password, string userId, string oldPassword, IFormFile file, string webRootPath)
        {
            Image image = null;
            ApplicationUser Ouser = await this.accountReposetry.GetUser(userId).ConfigureAwait(false);
            if (password != null)
            {
                if (!await this.accountReposetry.ChangePassword(Ouser, oldPassword, password).ConfigureAwait(false))
                {
                    return false;
                }
            }
            if (file != null)
            {
                image = await this.imageService.Add(file, webRootPath).ConfigureAwait(false);
            }
            var user = new ApplicationUser()
            {
                Id = userId,
                Avatar = image,
                Description = description,
                UserName = username
            };
            return await this.accountReposetry.UpdateUser(Ouser, user).ConfigureAwait(false);
        }

        public async Task<ICollection<string>> GetUserRoles(ApplicationUser user)
        {
            return await this.accountReposetry.GetRoleList(user).ConfigureAwait(false);
        }

        public IList<ApplicationUser> GetAllUsers()
        {
            return this.accountReposetry.GetAllUsers();
        }

        public async Task<bool> SetModerator(ApplicationUser user)
        {
            return await this.accountReposetry.AddRoleToUser(user, Enum.GetName(typeof(Roles), Roles.Moderator)).ConfigureAwait(false);
        }

        public async Task<bool> SetNonModerator(ApplicationUser user)
        {
            var roles = new List<string> { Enum.GetName(typeof(Roles), Roles.Moderator) };
            return await this.accountReposetry.RemoveRolesFromUser(user, roles).ConfigureAwait(false);
        }
    }
}
