using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfases;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Reposetries
{
    public class AccountReposetry : IAccountReposetry
    {
        public AccountReposetry(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, DatabaseContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        SignInManager<ApplicationUser> _signInManager;
        DatabaseContext _context;

        public async Task<bool> CreateUser(ApplicationUser user, string password)
        {
            var result =  await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }
        public async Task<bool> CreateUser(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user);
            return result.Succeeded;
        }
        public async Task<bool> AddLoginToUser(ApplicationUser user, ExternalLoginInfo info)
        {
            var result = await _userManager.AddLoginAsync(user, info);
            return result.Succeeded;
        }

        public async Task<bool> AddRoleToUser(ApplicationUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            await _userManager.UpdateSecurityStampAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ChangePassword(ApplicationUser user, string oldPass, string newPass)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPass, newPass);
            return result.Succeeded;
        }

        public async Task<bool> Login(string email ,string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                if (result != PasswordVerificationResult.Failed)
                {
                    await _signInManager.SignInAsync(user, false);
                    return true;
                }
            }
            return false;
        }
        public async Task Login(ApplicationUser user)
        {
            await _signInManager.SignInAsync(user, false);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
        public AuthenticationProperties GoogleLogin(string url)
        {
            var prop = _signInManager.ConfigureExternalAuthenticationProperties("Google", url);
            return prop;
        }
        public async Task<bool> ExternalLogin(ExternalLoginInfo info)
        {
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: false);
            return result.Succeeded;
        }

        public async Task<ApplicationUser> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<bool> AddRole(string name)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(name));
            return result.Succeeded;
        }
        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            return info;
        }

        public async Task<bool> RemoveRolesFromUser(ApplicationUser user, ICollection<string> roles)
        {
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.UpdateSecurityStampAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> UpdateUser(ApplicationUser Ouser, ApplicationUser user)
        {
            if(user.Avatar != null)
            {
                Ouser.Avatar = user.Avatar;
                Ouser.AvatarId = user.Avatar.Id;
            }
            Ouser.UserName = user.UserName;
            Ouser.Description = user.Description;
            var result = await _userManager.UpdateAsync(Ouser);
            return result.Succeeded;
        }

        public async Task<bool> IsExist(string mail)
        {
            var result = await _userManager.FindByEmailAsync(mail);
            return (result == null);
        }

        public async Task<bool> IsInRole(ApplicationUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<IList<string>> GetRoleList(ApplicationUser user)
        {
            var roleList = await _userManager.GetRolesAsync(user);
            return roleList;
        }

        public IList<ApplicationUser> GetAllUsers()
        {
            var allUsers =  _userManager.Users.ToList();
            return allUsers;
        }

        public async Task<bool> SetRolesBeforeBan(ApplicationUser user, string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            RolesBeforeBan beforeBan = new RolesBeforeBan()
            {
                Id = Guid.NewGuid(),
                Role = role,
                User = user
            };
            _context.RolesBeforeBan.Add(beforeBan);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> UnBanUser(ApplicationUser user)
        {
            await _userManager.UpdateSecurityStampAsync(user);
            var result = await _userManager.RemoveFromRoleAsync(user, "Banned");
            await _userManager.AddToRoleAsync(user, "User");
            return result.Succeeded;
        }
        public void  DeleteSavedRoles(string userId)
        {
            var roles = _context.RolesBeforeBan.Where(role => role.UserId == userId).ToList();
            _context.RolesBeforeBan.RemoveRange(roles);
            _context.SaveChanges();
        }
        public bool IsAdminCreated()
        {
            return _roleManager.Roles.ToList().Count != 0;
        }
    }
}
