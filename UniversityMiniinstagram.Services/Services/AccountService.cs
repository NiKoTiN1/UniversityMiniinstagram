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
        public AccountService(IAccountReposetry accountReposetry, IImageService imageService)
        {
            _accountReposetry = accountReposetry;
            _imageService = imageService;
        }

        IAccountReposetry _accountReposetry;
        IImageService _imageService;
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

        public async Task<bool> IsInRole(IsInRoleViewModel vm)
        {
            var isInRole = await _accountReposetry.IsInRole(vm.user, vm.roleName);
            return isInRole;
        }

        public async Task<bool> SetBanRole (ApplicationUser user)
        {
            var isBanned = await _accountReposetry.IsInRole(user, "Banned");
            if (isBanned)
            {
                return true;
            }
            var roleList = await _accountReposetry.GetRoleList(user);
            foreach(var role in roleList)
            {
                if(role != "User")
                {
                    await _accountReposetry.SetRolesBeforeBan(user, role);
                }
            }
            var result = await _accountReposetry.RemoveRolesFromUser(user, roleList);
            if (!result)
            {
                return false;
            }
            return await _accountReposetry.AddRoleToUser(user, "Banned");
        }

        public async Task<bool> UnBanUser(ApplicationUser user)
        {
            await _accountReposetry.UnBanUser(user);
            _accountReposetry.DeleteSavedRoles(user.Id);
            return true;
        }

        public async Task<bool> Login (LoginViewModel vm)
        {
            var result = await _accountReposetry.Login(vm.Email, vm.Password);
            return result;
        }

        public async Task<ApplicationUser> GetUser(string userId)
        {
            var user = await _accountReposetry.GetUser(userId);
            if(user.AvatarId != null)
            {
                var image = _imageService.GetImage(user.AvatarId.Value);
                if (image != null)
                {
                    user.Avatar = image;
                    return user;
                }
            }
            user.Avatar = new Image() { Path = "/Images/noPhoto.png" };
            return user;
        }

        public async Task<bool> AddRole(string name)
        {
            var result = await _accountReposetry.AddRole(name);
            return result;
        }

        public async Task<bool> EditProfile(EditProfileViewModel vm)
        {
            Image image = null;
            if(vm.File != null)
            {
                image = await _imageService.Add(vm, vm.WebRootPath);
            }
            ApplicationUser user = new ApplicationUser()
            {
                Id = vm.Userid,
                Avatar = image,
                Description = vm.Description,
                UserName = vm.Username
            };
            var result = await _accountReposetry.UpdateUser(user);
            return result;
        }

        public async Task<ICollection<string>> GetUserRoles(ApplicationUser user)
        {
            var roleList = await _accountReposetry.GetRoleList(user);
            return roleList;
        }
        public IList<ApplicationUser> GetAllUsers()
        {
            var userList = _accountReposetry.GetAllUsers();
            foreach(var user in userList)
            {
                if (user.AvatarId != null)
                {
                    var image = _imageService.GetImage(user.AvatarId.Value);
                    if (image != null)
                    {
                        user.Avatar = image;
                    }
                }
                else
                {
                    user.Avatar = new Image() { Path = "/Images/noPhoto.png" };
                }
            }
            return userList;
        }
    }
}
