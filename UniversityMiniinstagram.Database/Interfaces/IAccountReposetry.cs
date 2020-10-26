using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UniversityMiniinstagram.Database.Interfases
{
    public interface IAccountReposetry
    {
        public Task<bool> IsExist(string mail);
        public Task<bool> IsInRole(ApplicationUser user, string role);

        public Task<bool> CreateUser(ApplicationUser user, string password);

        public Task<bool> AddRoleToUser(ApplicationUser user, string role);

        public Task<bool> RemoveUser(string id);

        public Task<bool> Login(string email, string password);

        public Task<ApplicationUser> GetUser(string id);

        public Task<bool> AddRole(string name);

        public Task<bool> UpdateUser(ApplicationUser user);
    }
}
