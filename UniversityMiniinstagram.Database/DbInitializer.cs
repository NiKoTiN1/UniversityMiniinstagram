using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Constants;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database
{
    public class DbInitializer
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.Roles.Any())
            {
                return;
            }
            await roleManager.CreateAsync(new IdentityRole(Enum.GetName(typeof(Roles), Roles.Admin)));
            await roleManager.CreateAsync(new IdentityRole(Enum.GetName(typeof(Roles), Roles.User)));
            await roleManager.CreateAsync(new IdentityRole(Enum.GetName(typeof(Roles), Roles.Moderator)));
            await roleManager.CreateAsync(new IdentityRole(Enum.GetName(typeof(Roles), Roles.Banned)));
        }
        public static async Task SeedAdmin(UserManager<ApplicationUser> userManager)
        {
            if (userManager.Users.Any())
            {
                return;
            }
            var admin = new ApplicationUser
            {
                Email = "Admin@mail.ru",
                Description = "AdminAcc",
                UserName = "Admin",
                AvatarId = new Guid().ToString()
            };
            await userManager.CreateAsync(admin, "Admin_1");
            await userManager.AddToRoleAsync(admin, Enum.GetName(typeof(Roles), Roles.Admin));
            await userManager.AddToRoleAsync(admin, Enum.GetName(typeof(Roles), Roles.Moderator));
            await userManager.AddToRoleAsync(admin, Enum.GetName(typeof(Roles), Roles.User));
        }
    }
}
