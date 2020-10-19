using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Database.Interfases;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services.Services
{
    public class AccountService : IAccountService
    {
        public AccountService(IAccountReposetry accountReposetry)
        {
            _accountReposetry = accountReposetry;
        }

        IAccountReposetry _accountReposetry;
        public async Task<bool> Register(RegisterViewModel vm)
        {
            var isExist = await _accountReposetry.IsExist(vm.Email);
            if(isExist)
            {
                ApplicationUser user = new ApplicationUser { Email = vm.Email, Avatar = vm.Avatar, Description = vm.Description, UserName = vm.Username };
                var isCreated = await _accountReposetry.CreateUser(user, vm.Password);
                if (isCreated)
                {
                    var isRoleAdded = await _accountReposetry.AddRoleToUser(user, vm.Role);
                    if(isRoleAdded)
                    {
                        return true;
                    }
                    await _accountReposetry.RemoveUser(user.Id);
                }
            }
            return false;
        }

        public async Task<bool> Login (LoginViewModel vm)
        {
            var result = await _accountReposetry.Login(vm.Email, vm.Password);
            return result;
        }

        public async Task<ApplicationUser> GetUser(string userId)
        {
            var user = await _accountReposetry.GetUser(userId);
            return user;
        }

        public async Task<bool> AddRole(string name)
        {
            var result = await _accountReposetry.AddRole(name);
            return result;
        }
    }
}
