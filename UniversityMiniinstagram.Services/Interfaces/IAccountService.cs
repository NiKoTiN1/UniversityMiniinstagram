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
        public Task Login(ApplicationUser user);
        public Task Logout();
        public Task<bool> IsUserExist(string email);
        public AuthenticationProperties GoogleLogin(string url);
        public Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        public Task<bool> ExternalLogin(ExternalLoginInfo info);
        public Task<ApplicationUser> GetUser(string userId);
        public Task<ApplicationUser> GetUserByEmail(string email);
        public Task<bool> EditProfile(EditProfileViewModel vm);
        public Task<bool> AddRole(string name);
        public Task<bool> IsInRole(IsInRoleViewModel vm);
        public Task<bool> SetBanRole(ApplicationUser user);
        public Task<ICollection<string>> GetUserRoles(ApplicationUser user);
        public Task<bool> AddLoginToUser(ApplicationUser user, ExternalLoginInfo info);
        public IList<ApplicationUser> GetAllUsers();
        public Task<bool> UnBanUser(ApplicationUser user);
        public Task<bool> SetModerator(ApplicationUser user);
        public Task<bool> SetNonModerator(ApplicationUser user);
        public bool IsAdminCreated();
        public Task<bool> RegisterAdmin(RegisterViewModel vm);
    }
}
