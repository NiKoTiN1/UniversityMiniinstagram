using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            this.UserManager = userManager;
            this.RoleManager = roleManager;
            this.SignInManager = signInManager;
        }

        private readonly UserManager<ApplicationUser> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly SignInManager<ApplicationUser> SignInManager;

        public async Task<bool> CreateUser(ApplicationUser user, string password)
        {
            IdentityResult result = await this.UserManager.CreateAsync(user, password);
            return result.Succeeded;
        }
        public async Task<bool> CreateUser(ApplicationUser user)
        {
            IdentityResult result = await this.UserManager.CreateAsync(user);
            return result.Succeeded;
        }
        public async Task<bool> AddLoginToUser(ApplicationUser user, ExternalLoginInfo info)
        {
            IdentityResult result = await this.UserManager.AddLoginAsync(user, info);
            return result.Succeeded;
        }

        public async Task<bool> AddRoleToUser(ApplicationUser user, string role)
        {
            IdentityResult result = await this.UserManager.AddToRoleAsync(user, role);
            await this.UserManager.UpdateSecurityStampAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUser(string id)
        {
            ApplicationUser user = await this.UserManager.FindByIdAsync(id.ToString());
            IdentityResult result = await this.UserManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ChangePassword(ApplicationUser user, string oldPass, string newPass)
        {
            IdentityResult result = await this.UserManager.ChangePasswordAsync(user, oldPass, newPass);
            return result.Succeeded;
        }

        public async Task<bool> Login(string email, string password)
        {
            ApplicationUser user = await this.UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                PasswordVerificationResult result = this.UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                if (result != PasswordVerificationResult.Failed)
                {
                    await this.SignInManager.SignInAsync(user, false);
                    return true;
                }
            }
            return false;
        }
        public async Task Login(ApplicationUser user)
        {
            await this.SignInManager.SignInAsync(user, false);
        }

        public async Task Logout()
        {
            await this.SignInManager.SignOutAsync();
        }
        public AuthenticationProperties GoogleLogin(string url)
        {
            AuthenticationProperties prop = this.SignInManager.ConfigureExternalAuthenticationProperties("Google", url);
            return prop;
        }
        public async Task<bool> ExternalLogin(ExternalLoginInfo info)
        {
            SignInResult result = await this.SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: false);
            return result.Succeeded;
        }

        public async Task<ApplicationUser> GetUser(string id)
        {
            ApplicationUser user = await this.UserManager.FindByIdAsync(id);
            return user;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            ApplicationUser user = await this.UserManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<bool> AddRole(string name)
        {
            IdentityResult result = await this.RoleManager.CreateAsync(new IdentityRole(name));
            return result.Succeeded;
        }
        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await this.SignInManager.GetExternalLoginInfoAsync();
        }

        public async Task<bool> RemoveRolesFromUser(ApplicationUser user, ICollection<string> roles)
        {
            IdentityResult result = await this.UserManager.RemoveFromRolesAsync(user, roles);
            await this.UserManager.UpdateSecurityStampAsync(user);
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
            IdentityResult result = await this.UserManager.UpdateAsync(Ouser);
            return result.Succeeded;
        }

        public async Task<bool> IsExist(string mail)
        {
            ApplicationUser result = await this.UserManager.FindByEmailAsync(mail);
            return (result == null);
        }

        public async Task<bool> IsInRole(ApplicationUser user, string role)
        {
            return await this.UserManager.IsInRoleAsync(user, role);
        }

        public async Task<IList<string>> GetRoleList(ApplicationUser user)
        {
            IList<string> roleList = await this.UserManager.GetRolesAsync(user);
            return roleList;
        }

        public IList<ApplicationUser> GetAllUsers()
        {
            var allUsers = this.UserManager.Users.ToList();
            return allUsers;
        }

        public async Task<bool> UnBanUser(ApplicationUser user)
        {
            IdentityResult result = await this.UserManager.RemoveFromRoleAsync(user, "Banned");
            await this.UserManager.AddToRoleAsync(user, "User");
            await this.UserManager.UpdateSecurityStampAsync(user);
            return result.Succeeded;
        }
    }
}
