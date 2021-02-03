using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<bool> Register(string email, string description, string username, string password);

        public Task<bool> Register(ApplicationUser user);

        public Task<bool> Login(string email, string password);

        public Task Logout();

        public AuthenticationProperties GoogleLogin(string url);

        public Task<ExternalLoginInfo> GetExternalLoginInfoAsync();

        public Task<bool> ExternalLogin(ExternalLoginInfo info);

        public Task<ApplicationUser> GetUser(string userId);

        public Task<bool> EditProfile(string username, string description, string password, string userId, string oldPassword, IFormFile file, string webRootPath);
        
        public Task<bool> IsInRole(string userId, string roleName, ApplicationUser user = null);
        public Task<bool> SetBanRole(string userId, ApplicationUser user = null);

        public Task<ICollection<string>> GetUserRoles(ApplicationUser user);

        public IList<ApplicationUser> GetAllUsers();

        public Task<bool> UnBanUser(string userId);

        public Task<bool> SetModerator(ApplicationUser user);

        public Task<bool> SetNonModerator(ApplicationUser user);
    }
}
