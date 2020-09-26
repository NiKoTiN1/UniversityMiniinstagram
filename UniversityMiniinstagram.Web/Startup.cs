using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Web;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Services.Services;
using UniversityMiniinstagram.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using UniversityMiniinstagram.Web.Contraints;

namespace UniversityMiniinstagram
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/account/login");

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.AddScoped<PostServices>();
            services.AddScoped<ImageServices>();
            services.AddMvc(option => option.EnableEndpointRouting = false) ;

            services.AddRazorPages();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(routing =>
            {
                routing.MapRoute("1", "account/login", new { controller = "Account", action = "Login"});
                routing.MapRoute("2", "account/logined", new { controller = "Account", action = "LoginPost" });

                routing.MapRoute("3", "account/profile", new { controller = "Account", action = "Profile" });
                routing.MapRoute("4", "account/profile/{numb?}", new { controller = "Account", action = "ProfileNumb" },
    new { numb = new CustomRouteConstraint() });
                routing.MapRoute("5", "account/register", new { controller = "Account", action = "Register" });
                routing.MapRoute("6", "account/registed", new { controller = "Account", action = "RegisterPost" });
                routing.MapRoute("7", "account/logout", new { controller = "Account", action = "Logout" });

                routing.MapRoute("8", "news/all", new { controller = "News", action = "GetAllPosts" });
                routing.MapRoute("9", "news/addPost", new { controller = "News", action = "AddPost" });
            });
        }
    }
}
