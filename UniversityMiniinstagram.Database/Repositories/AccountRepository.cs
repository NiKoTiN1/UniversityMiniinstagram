using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Constants;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public async Task<bool> CreateUser(ApplicationUser user, string password)
        {
            IdentityResult result = await this.userManager.CreateAsync(user, password).ConfigureAwait(false);
            return result.Succeeded;
        }

        public async Task<bool> CreateUser(ApplicationUser user)
        {
            IdentityResult result = await this.userManager.CreateAsync(user).ConfigureAwait(false);
            return result.Succeeded;
        }

        public async Task<bool> AddLoginToUser(ApplicationUser user, ExternalLoginInfo info)
        {
            IdentityResult result = await this.userManager.AddLoginAsync(user, info).ConfigureAwait(false);
            return result.Succeeded;
        }

        public async Task<bool> AddRoleToUser(ApplicationUser user, string role)
        {
            IdentityResult result = await this.userManager.AddToRoleAsync(user, role).ConfigureAwait(false);
            await this.userManager.UpdateSecurityStampAsync(user).ConfigureAwait(false);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUser(string id)
        {
            ApplicationUser user = await this.userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
            IdentityResult result = await this.userManager.DeleteAsync(user).ConfigureAwait(false);
            return result.Succeeded;
        }

        public async Task<bool> ChangePassword(ApplicationUser user, string oldPass, string newPass)
        {
            IdentityResult result = await this.userManager.ChangePasswordAsync(user, oldPass, newPass).ConfigureAwait(false);
            return result.Succeeded;
        }

        public async Task<bool> Login(string email, string password)
        {
            ApplicationUser user = await this.userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user == null)
            {
                return false;
            }
            PasswordVerificationResult result = this.userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
            {
                return false;
            }
            await this.signInManager.SignInAsync(user, false).ConfigureAwait(false);
            return true;
        }

        public async Task Login(ApplicationUser user)
        {
            await this.signInManager.SignInAsync(user, false).ConfigureAwait(false);
        }

        public async Task Logout()
        {
            await this.signInManager.SignOutAsync().ConfigureAwait(false);
        }

        public AuthenticationProperties GoogleLogin(string url)
        {
            return this.signInManager.ConfigureExternalAuthenticationProperties("Google", url);
        }

        public async Task<bool> ExternalLogin(ExternalLoginInfo info)
        {
            SignInResult result = await this.signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: false).ConfigureAwait(true);
            return result.Succeeded;
        }

        public async Task<ApplicationUser> GetUser(string id)
        {
            ApplicationUser user = await this.userManager.FindByIdAsync(id).ConfigureAwait(false);
            return user;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            ApplicationUser user = await this.userManager.FindByEmailAsync(email).ConfigureAwait(false);
            return user;
        }

        public async Task<bool> AddRole(string name)
        {
            IdentityResult result = await this.roleManager.CreateAsync(new IdentityRole(name)).ConfigureAwait(false);
            return result.Succeeded;
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await this.signInManager.GetExternalLoginInfoAsync().ConfigureAwait(false);
        }

        public async Task<bool> RemoveRolesFromUser(ApplicationUser user, ICollection<string> roles)
        {
            IdentityResult result = await this.userManager.RemoveFromRolesAsync(user, roles).ConfigureAwait(false);
            await this.userManager.UpdateSecurityStampAsync(user).ConfigureAwait(false);
            return result.Succeeded;
        }

        public async Task<bool> UpdateUser(ApplicationUser Ouser, ApplicationUser user)
        {
            if (user.Avatar != null)
            {
                Ouser.Avatar = user.Avatar;
                Ouser.AvatarId = user.Avatar.Id;
            }
            Ouser.UserName = user.UserName;
            Ouser.Description = user.Description;
            IdentityResult result = await this.userManager.UpdateAsync(Ouser).ConfigureAwait(false);
            return result.Succeeded;
        }

        public async Task<bool> IsExist(string mail)
        {
            ApplicationUser result = await this.userManager.FindByEmailAsync(mail).ConfigureAwait(false);
            return result != null;
        }

        public async Task<bool> IsInRole(ApplicationUser user, string role)
        {
            return await this.userManager.IsInRoleAsync(user, role).ConfigureAwait(false);
        }

        public async Task<IList<string>> GetRoleList(ApplicationUser user)
        {
            return await this.userManager.GetRolesAsync(user).ConfigureAwait(false);
        }

        public IList<ApplicationUser> GetAllUsers()
        {
            return this.userManager.Users.ToList();
        }

        public async Task<bool> UnBanUser(ApplicationUser user)
        {
            IdentityResult result = await this.userManager.RemoveFromRoleAsync(user, Enum.GetName(typeof(Roles), Roles.Banned)).ConfigureAwait(false);
            await this.userManager.AddToRoleAsync(user, Enum.GetName(typeof(Roles), Roles.Admin)).ConfigureAwait(false);
            await this.userManager.UpdateSecurityStampAsync(user).ConfigureAwait(false);
            return result.Succeeded;
        }
    }
}
