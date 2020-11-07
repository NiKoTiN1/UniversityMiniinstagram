using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<bool> Register(RegisterViewModel vm);
        public Task<bool> Login(LoginViewModel vm);
        public Task<ApplicationUser> GetUser(string userId);
        public Task<bool> EditProfile(EditProfileViewModel vm);
        public Task<bool> AddRole(string name);
        public Task<bool> IsInRole(IsInRoleViewModel vm);
        public Task<bool> SetBanRole(ApplicationUser user);
        public Task<ICollection<string>> GetUserRoles(ApplicationUser user);
        public IList<ApplicationUser> GetAllUsers();
        public Task<bool> UnBanUser(ApplicationUser user);
        public Task<bool> SetModerator(ApplicationUser user);
        public Task<bool> SetNonModerator(ApplicationUser user);
        public bool IsAdminCreated();
        public Task<bool> RegisterAdmin(RegisterViewModel vm);
    }
}
