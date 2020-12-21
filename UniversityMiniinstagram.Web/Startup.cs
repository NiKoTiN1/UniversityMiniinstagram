using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Database.Reposetries;
using UniversityMiniinstagram.Services;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Services.Services;
using UniversityMiniinstagram.Web.Constraints;
using UniversityMiniinstagram.Web.Hubs;


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

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
            });

            services.AddSignalR(o => o.EnableDetailedErrors = true);


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");
                    options.ClientId = Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                });

            services.AddTransient<IPostReposetry, PostReposetry>();
            services.AddTransient<IPostService, PostService>();

            services.AddTransient<IImageReposetry, ImageReposetry>();
            services.AddTransient<IImageService, ImageService>();

            services.AddTransient<IAccountReposetry, AccountReposetry>();
            services.AddTransient<IAccountService, AccountService>();

            services.AddTransient<IAdminReposetry, AdminReposetry>();
            services.AddTransient<IAdminService, AdminService>();

            services.AddTransient<IViewRenderService, ViewRenderService>();

            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddRazorPages();
            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                CultureInfo[] supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("de"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

        }

        [Obsolete]
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

            IOptions<RequestLocalizationOptions> locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseRequestLocalization();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSignalR(routes => { routes.MapHub<CommentaryHub>("/comment"); });

            app.UseMvc(routing =>
            {
                routing.MapRoute("1", "account/login", new { controller = "Account", action = "Login" });
                routing.MapRoute("2", "account/logined", new { controller = "Account", action = "LoginPost" });

                routing.MapRoute("3", "account/profile", new { controller = "Account", action = "Profile" });
                routing.MapRoute("4", "account/profile/{numb}", new { controller = "Account", action = "ProfileNumb" },
                    new { numb = new CustomRouteConstraint() });

                routing.MapRoute("5", "account/register", new { controller = "Account", action = "Register" });
                routing.MapRoute("6", "account/registed", new { controller = "Account", action = "RegisterPost" });
                routing.MapRoute("7", "account/logout", new { controller = "Account", action = "Logout" });

                routing.MapRoute("8", "news/all", new { controller = "News", action = "GetAllPosts" });
                routing.MapRoute("9", "news/addPost", new { controller = "News", action = "AddPost" });
                routing.MapRoute("10", "account/setlanguage", new { controller = "Account", action = "SetLanguage" });
                routing.MapRoute("11", "account/profile/edit", new { controller = "Account", action = "EditProfile" });
                routing.MapRoute("12", "account/profile/edited", new { controller = "Account", action = "EditProfilePost" });
                routing.MapRoute("13", "news/getPost", new { controller = "News", action = "GetPost" });
                routing.MapRoute("14", "Account/AccessDenied", new { controller = "Account", action = "AccessDenied" });
                routing.MapRoute("15", "news/removedComment", new { controller = "News", action = "RemoveCommentPost" });
                routing.MapRoute("16", "report/send", new { controller = "Admin", action = "SendReport" });
                routing.MapRoute("17", "news/removedPost", new { controller = "News", action = "RemovePost" });
                routing.MapRoute("18", "admin/reports/posts", new { controller = "Admin", action = "GetPostReports" });
                routing.MapRoute("19", "admin/reports/comments", new { controller = "Admin", action = "GetCommentReports" });
                routing.MapRoute("20", "admin/roles", new { controller = "Admin", action = "SetDeleteRoles" });
                routing.MapRoute("21", "admin/appeals", new { controller = "Admin", action = "Appeals" });
                routing.MapRoute("22", "admin/pardon", new { controller = "Admin", action = "PardonPost" });
                routing.MapRoute("23", "admin/post/decision", new { controller = "Admin", action = "PostReportDecision" });
                routing.MapRoute("24", "admin/comment/decision", new { controller = "Admin", action = "CommentReportDecision" });
                routing.MapRoute("25", "account/ban", new { controller = "Account", action = "BanUser" });
                routing.MapRoute("26", "account/unban", new { controller = "Account", action = "UnBanUser" });
                routing.MapRoute("27", "admin/moderroots", new { controller = "Admin", action = "AddModerator" });
                routing.MapRoute("28", "admin/userroots", new { controller = "Admin", action = "RemoveModerator" });
                routing.MapRoute("29", "account/banned", new { controller = "Account", action = "BanPage" });
                routing.MapRoute("30", "settings", new { controller = "Settings", action = "GetSettings" });
                routing.MapRoute("31", "settings/language", new { controller = "Settings", action = "SetLanguage" });
                routing.MapRoute("32", "account/google-login", new { controller = "Account", action = "GoogleLogin" });
                routing.MapRoute("33", "account/google-response", new { controller = "Account", action = "GoogleResponse" });

                routing.MapRoute("34", "account/addrole", new { controller = "Account", action = "CreateRolePost" });

            });
        }
    }
}
