using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfinetworksOnlineTest.ServiceConfig;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfinetworksOnlineTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //Config Database Connection and Auth Server
            Constant.connectionString = configuration.GetConnectionString("InfinetConnectionDatabase");
            Constant.AuthServer = configuration.GetSection("AuthServer").GetValue<string>("applicationUrl");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            // Konfigurasi Untuk Identitas User
            services.Configure<IdentityOptions>(options =>
            {
                //Berikut ini adalah password setting authentications untuk user yang akan register
                //options.ClaimsIdentity.RoleClaimType = "Admin";
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0; // there must be integer

                //mengatur Logout session
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                // maksimal user melakukan kesalahan saat akan login
                options.Lockout.MaxFailedAccessAttempts = 5;
                // memperbolehkan user baru untuk login setelah user lama logout 
                options.Lockout.AllowedForNewUsers = true;

                //User Settings
                // karakter yang diperbolehkan untuk digunakan oleh user
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
                // email yang diperbolehkan digunakan oleh user
                options.User.RequireUniqueEmail = false;
            });

            //Config Add Authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("UsernOnly", policy => policy.RequireRole("User"));
            });

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(10);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.CookiePrefix)
                .AddCookie(option => 
                {
                    option.AccessDeniedPath = new PathString("/AdminArea/View/Shared/Accessdenied");
                    option.SlidingExpiration = true;
                    option.Cookie.SameSite = SameSiteMode.Strict;
                    option.ExpireTimeSpan = TimeSpan.FromHours(6);
                });

            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.AllowAreas = true;
            });

            services.AddSession();
            services.AddRouting();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapAreaRoute(
                    "LoginAdmin",
                    "AdminArea",
                    "AdminArea/{controller=HomeAdmin}/{action=LoginAction}");

                routes.MapAreaRoute(
                    "defaultAdmin",
                    "AdminArea",
                    "AdminArea/{controller=HomeAdmin}/{action=Home}");

                routes.MapRoute(
                    "Guess", "{controller=Home}/{action=RegisterInterviewer}");

                routes.MapRoute(
                    "GuessQuestions", "{controller=Home}/{action=AddAnswerInterviewer}");

                routes.MapRoute(
                    "GuessCongratulation", "{controller=Home}/{action=Congratulation}");
            });
        }
    }
}
