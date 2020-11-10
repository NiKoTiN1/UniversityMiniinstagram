using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
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
                    var isRoleAdded = await _accountReposetry.AddRoleToUser(user, "User");

                    if (isRoleAdded)
                    {
                        return true;
                    }
                    await _accountReposetry.RemoveUser(user.Id);
                }
            }
            return false;
        }
        public async Task<bool> Register(ApplicationUser user)
        {
            var result = await _accountReposetry.CreateUser(user);
            if(result)
            {
                var isRoleAdded = await _accountReposetry.AddRoleToUser(user, "User");

                if (isRoleAdded)
                {
                    return true;
                }
            }
            await _accountReposetry.RemoveUser(user.Id);
            return false;
        }


        public async Task<bool> IsUserExist(string email)
        {
            var result = await _accountReposetry.IsExist(email);
            return result;
        }
        public async Task<bool> RegisterAdmin(RegisterViewModel vm)
        {
            var isExist = await _accountReposetry.IsExist(vm.Email);
            if (isExist)
            {
                ApplicationUser user = new ApplicationUser { Email = vm.Email, Avatar = vm.Avatar, Description = vm.Description, UserName = vm.Username };
                var isCreated = await _accountReposetry.CreateUser(user, vm.Password);
                if (isCreated)
                {
                    var isRoleAdded = await _accountReposetry.AddRoleToUser(user, "Admin");
                    var isRoleAddedtemp1 = await _accountReposetry.AddRoleToUser(user, "Modarator");
                    var isRoleAddedtemp2 = await _accountReposetry.AddRoleToUser(user, "User");
                    if (isRoleAdded && isRoleAddedtemp1 && isRoleAddedtemp2)
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

        public async Task<bool> AddLoginToUser(ApplicationUser user, ExternalLoginInfo info)
        {
            var result = await _accountReposetry.AddLoginToUser(user, info);
            return result;
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
        public async Task Login(ApplicationUser user)
        {
            await _accountReposetry.Login(user);
        }

        public AuthenticationProperties GoogleLogin(string url)
        {
            var res = _accountReposetry.GoogleLogin(url);
            return res;
        }
        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            var info = await _accountReposetry.GetExternalLoginInfoAsync();
            return info;
        }
        public async Task<bool> ExternalLogin(ExternalLoginInfo info)
        {
            var result = await _accountReposetry.ExternalLogin(info);
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

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await _accountReposetry.GetUserByEmail(email);
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
            var Ouser = await _accountReposetry.GetUser(vm.Userid);
            if (vm.File != null)
            {
                image = await _imageService.Add(vm, vm.WebRootPath);
            }
            if(vm.Password != null)
            {
                var res = await _accountReposetry.ChangePassword(Ouser, vm.OldPassword, vm.Password);
                if(!res)
                {
                    return false;
                }
            }
            ApplicationUser user = new ApplicationUser()
            {
                Id = vm.Userid,
                Avatar = image,
                Description = vm.Description,
                UserName = vm.Username
            };
            var result = await _accountReposetry.UpdateUser(Ouser, user);
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

        public async Task <bool> SetModerator(ApplicationUser user)
        {
            var result = await _accountReposetry.AddRoleToUser(user, "Modarator");
            return result;
        }

        public async Task<bool> SetNonModerator(ApplicationUser user)
        {
            List<string> roles = new List<string>();
            roles.Add("Modarator");
            var result = await _accountReposetry.RemoveRolesFromUser(user, roles);
            return result;
        }
        public bool IsAdminCreated()
        {
            return _accountReposetry.IsAdminCreated();
        }
    }
}
