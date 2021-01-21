using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IAccountRepository
    {
        public Task<bool> IsExist(string mail);
        public Task<bool> IsInRole(ApplicationUser user, string role);
        public Task<bool> CreateUser(ApplicationUser user, string password);
        public Task<bool> CreateUser(ApplicationUser user);
        public Task<bool> AddLoginToUser(ApplicationUser user, ExternalLoginInfo info);
        public Task<bool> AddRoleToUser(ApplicationUser user, string role);
        public Task<bool> RemoveUser(string id);
        public Task<bool> Login(string email, string password);
        public Task Login(ApplicationUser user);
        public AuthenticationProperties GoogleLogin(string url);
        public Task<bool> ExternalLogin(ExternalLoginInfo info);
        public Task Logout();
        public Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        public Task<ApplicationUser> GetUser(string id);
        public Task<ApplicationUser> GetUserByEmail(string email);
        public Task<bool> AddRole(string name);
        public Task<bool> UpdateUser(ApplicationUser oldUser, ApplicationUser user);
        public Task<IList<string>> GetRoleList(ApplicationUser user);
        public Task<bool> RemoveRolesFromUser(ApplicationUser user, ICollection<string> roles);
        public IList<ApplicationUser> GetAllUsers();
        public Task<bool> UnBanUser(ApplicationUser user);
        public Task<bool> ChangePassword(ApplicationUser user, string oldPass, string newPass);
    }
}
