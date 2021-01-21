using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            CreateDbIfNotExists(host);
            host.Run();
        }
        private static async void CreateDbIfNotExists(IHost host)
        {
            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                try
                {
                    UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await DbInitializer.SeedRoles(roleManager).ConfigureAwait(false);
                    await DbInitializer.SeedAdmin(userManager).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        });
        }
    }
}
