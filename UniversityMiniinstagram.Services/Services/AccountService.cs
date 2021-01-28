using Microsoft.AspNetCore.Authentication;
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
using UniversityMiniinstagram.Views;

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

        public async Task<bool> Register(RegisterViewModel vm)
        {
            if (!await this.AccountReposetry.IsExist(vm.Email))
            {
                return false;
            }
            var user = new ApplicationUser { Email = vm.Email, Avatar = vm.Avatar, Description = vm.Description, UserName = vm.Username, AvatarId = new Guid().ToString() };
            if (!await this.AccountReposetry.CreateUser(user, vm.Password))
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
        public async Task<bool> Login(LoginViewModel vm)
        {
            return await this.AccountReposetry.Login(vm.Email, vm.Password);
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

        public async Task<bool> EditProfile(EditProfileViewModel vm)
        {
            Image image = null;
            ApplicationUser Ouser = await this.AccountReposetry.GetUser(vm.UserId);
            if (vm.Password != null)
            {
                if (!await this.AccountReposetry.ChangePassword(Ouser, vm.OldPassword, vm.Password))
                {
                    return false;
                }
            }
            if (vm.File != null)
            {
                image = await this.ImageService.Add(vm, vm.WebRootPath);
            }
            var user = new ApplicationUser()
            {
                Id = vm.UserId,
                Avatar = image,
                Description = vm.Description,
                UserName = vm.Username
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
