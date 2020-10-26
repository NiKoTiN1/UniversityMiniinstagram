using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfases;

namespace UniversityMiniinstagram.Database.Reposetries
{
    public class AccountReposetry : IAccountReposetry
    {
        public AccountReposetry(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        SignInManager<ApplicationUser> _signInManager;

        public async Task<bool> CreateUser(ApplicationUser user, string password)
        {
            var result =  await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        public async Task<bool> AddRoleToUser(ApplicationUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
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

        public async Task<ApplicationUser> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<bool> AddRole(string name)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(name));
            return result.Succeeded;
        }

        public async Task<bool> UpdateUser(ApplicationUser user)
        {
            var baseUser = await _userManager.FindByIdAsync(user.Id);
            baseUser.Avatar = user.Avatar;
            baseUser.UserName = user.UserName;
            baseUser.Description = user.Description;
            var result = await _userManager.UpdateAsync(baseUser);
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
    }
}
