using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<bool> Register(RegisterViewModel vm);
        public Task<bool> Register(ApplicationUser user);
        public Task<bool> Login(LoginViewModel vm);
        public Task Logout();
        public AuthenticationProperties GoogleLogin(string url);
        public Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        public Task<bool> ExternalLogin(ExternalLoginInfo info);
        public Task<ApplicationUser> GetUser(string userId);
        public Task<bool> EditProfile(EditProfileViewModel vm);
        public Task<bool> IsInRole(string userId, string roleName, ApplicationUser user = null);
        public Task<bool> SetBanRole(string userId, ApplicationUser user = null);
        public Task<ICollection<string>> GetUserRoles(ApplicationUser user);
        public IList<ApplicationUser> GetAllUsers();
        public Task<bool> UnBanUser(string userId);
        public Task<bool> SetModerator(ApplicationUser user);
        public Task<bool> SetNonModerator(ApplicationUser user);
    }
}
