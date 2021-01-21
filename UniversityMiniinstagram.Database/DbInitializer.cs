using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database
{
    public class DbInitializer
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
                await roleManager.CreateAsync(new IdentityRole("Moderator"));
                await roleManager.CreateAsync(new IdentityRole("Banned"));
            }
        }
        public static async Task SeedAdmin(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var admin = new ApplicationUser
                {
                    Email = "Admin@mail.ru",
                    Description = "AdminAcc",
                    UserName = "Admin",
                    AvatarId = new Guid().ToString()
                };
                await userManager.CreateAsync(admin, "Admin_1");
                await userManager.AddToRoleAsync(admin, "Admin");
                await userManager.AddToRoleAsync(admin, "Moderator");
                await userManager.AddToRoleAsync(admin, "User");
            }
        }
    }
}
