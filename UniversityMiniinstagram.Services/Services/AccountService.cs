using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services.Services
{
    public class AccountService : IAccountService
    {
        public AccountService(IAccountReposetry accountReposetry, IImageService imageService)
        {
            this.AccountReposetry = accountReposetry;
            this.ImageService = imageService;
        }

        private readonly IAccountReposetry AccountReposetry;
        private readonly IImageService ImageService;
        public async Task<bool> Register(RegisterViewModel vm)
        {
            var isExist = await this.AccountReposetry.IsExist(vm.Email);
            if (isExist)
            {
                var user = new ApplicationUser { Email = vm.Email, Avatar = vm.Avatar, Description = vm.Description, UserName = vm.Username };
                var isCreated = await this.AccountReposetry.CreateUser(user, vm.Password);
                if (isCreated)
                {
                    var isRoleAdded = await this.AccountReposetry.AddRoleToUser(user, "User");

                    if (isRoleAdded)
                    {
                        return true;
                    }
                    await this.AccountReposetry.RemoveUser(user.Id);
                }
            }
            return false;
        }
        public async Task<bool> Register(ApplicationUser user)
        {
            var result = await this.AccountReposetry.CreateUser(user);
            if (result)
            {
                var isRoleAdded = await this.AccountReposetry.AddRoleToUser(user, "User");

                if (isRoleAdded)
                {
                    return true;
                }
            }
            await this.AccountReposetry.RemoveUser(user.Id);
            return false;
        }


        public async Task<bool> IsUserExist(string email)
        {
            var result = await this.AccountReposetry.IsExist(email);
            return result;
        }
        public async Task<bool> RegisterAdmin(RegisterViewModel vm)
        {
            var isExist = await this.AccountReposetry.IsExist(vm.Email);
            if (isExist)
            {
                var user = new ApplicationUser { Email = vm.Email, Avatar = vm.Avatar, Description = vm.Description, UserName = vm.Username };
                var isCreated = await this.AccountReposetry.CreateUser(user, vm.Password);
                if (isCreated)
                {
                    var isRoleAdded = await this.AccountReposetry.AddRoleToUser(user, "Admin");
                    var isRoleAddedtemp1 = await this.AccountReposetry.AddRoleToUser(user, "Moderator");
                    var isRoleAddedtemp2 = await this.AccountReposetry.AddRoleToUser(user, "User");
                    if (isRoleAdded && isRoleAddedtemp1 && isRoleAddedtemp2)
                    {
                        return true;
                    }
                    await this.AccountReposetry.RemoveUser(user.Id);
                }
            }
            return false;
        }

        public async Task<bool> IsInRole(IsInRoleViewModel vm)
        {
            var isInRole = await this.AccountReposetry.IsInRole(vm.User, vm.RoleName);
            return isInRole;
        }

        public async Task<bool> SetBanRole(ApplicationUser user)
        {
            var isBanned = await this.AccountReposetry.IsInRole(user, "Banned");
            if (isBanned)
            {
                return true;
            }
            IList<string> roleList = await this.AccountReposetry.GetRoleList(user);
            foreach (var role in roleList)
            {
                if (role != "User")
                {
                    await this.AccountReposetry.SetRolesBeforeBan(user, role);
                }
            }
            var result = await this.AccountReposetry.RemoveRolesFromUser(user, roleList);
            return !result ? false : await this.AccountReposetry.AddRoleToUser(user, "Banned");
        }

        public async Task<bool> AddLoginToUser(ApplicationUser user, ExternalLoginInfo info)
        {
            var result = await this.AccountReposetry.AddLoginToUser(user, info);
            return result;
        }

        public async Task Logout()
        {
            await this.AccountReposetry.Logout();
        }

        public async Task<bool> UnBanUser(ApplicationUser user)
        {
            await this.AccountReposetry.UnBanUser(user);
            this.AccountReposetry.DeleteSavedRoles(user.Id);
            return true;
        }

        public async Task<bool> Login(LoginViewModel vm)
        {
            var result = await this.AccountReposetry.Login(vm.Email, vm.Password);
            return result;
        }
        public async Task Login(ApplicationUser user)
        {
            await this.AccountReposetry.Login(user);
        }

        public AuthenticationProperties GoogleLogin(string url)
        {
            AuthenticationProperties res = this.AccountReposetry.GoogleLogin(url);
            return res;
        }
        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            ExternalLoginInfo info = await this.AccountReposetry.GetExternalLoginInfoAsync();
            return info;
        }
        public async Task<bool> ExternalLogin(ExternalLoginInfo info)
        {
            var result = await this.AccountReposetry.ExternalLogin(info);
            return result;
        }

        public async Task<ApplicationUser> GetUser(string userId)
        {
            ApplicationUser user = await this.AccountReposetry.GetUser(userId);
            if (user.AvatarId != null)
            {
                Image image = this.ImageService.GetImage(user.AvatarId.Value);
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
            ApplicationUser user = await this.AccountReposetry.GetUserByEmail(email);
            return user;
        }


        public async Task<bool> AddRole(string name)
        {
            var result = await this.AccountReposetry.AddRole(name);
            return result;
        }

        public async Task<bool> EditProfile(EditProfileViewModel vm)
        {
            Image image = null;
            ApplicationUser Ouser = await this.AccountReposetry.GetUser(vm.Userid);
            if (vm.File != null)
            {
                image = await this.ImageService.Add(vm, vm.WebRootPath);
            }
            if (vm.Password != null)
            {
                var res = await this.AccountReposetry.ChangePassword(Ouser, vm.OldPassword, vm.Password);
                if (!res)
                {
                    return false;
                }
            }
            var user = new ApplicationUser()
            {
                Id = vm.Userid,
                Avatar = image,
                Description = vm.Description,
                UserName = vm.Username
            };
            var result = await this.AccountReposetry.UpdateUser(Ouser, user);
            return result;
        }

        public async Task<ICollection<string>> GetUserRoles(ApplicationUser user)
        {
            IList<string> roleList = await this.AccountReposetry.GetRoleList(user);
            return roleList;
        }

        public IList<ApplicationUser> GetAllUsers()
        {
            IList<ApplicationUser> userList = this.AccountReposetry.GetAllUsers();
            foreach (ApplicationUser user in userList)
            {
                if (user.AvatarId != null)
                {
                    Image image = this.ImageService.GetImage(user.AvatarId.Value);
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

        public async Task<bool> SetModerator(ApplicationUser user)
        {
            var result = await this.AccountReposetry.AddRoleToUser(user, "Moderator");
            return result;
        }

        public async Task<bool> SetNonModerator(ApplicationUser user)
        {
            var roles = new List<string>
            {
                "Moderator"
            };
            var result = await this.AccountReposetry.RemoveRolesFromUser(user, roles);
            return result;
        }
        public bool IsAdminCreated()
        {
            return this.AccountReposetry.IsAdminCreated();
        }
    }
}
