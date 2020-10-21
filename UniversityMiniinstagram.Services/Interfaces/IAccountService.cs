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
    }
}
