using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfinetworksOnlineTest.ServiceConfig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinetworksOnlineTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //Config Database Connection and Auth Server
            Constant.connectionString = configuration.GetConnectionString("InfinetConnectionDatabase");
            Constant.AuthServer = configuration.GetSection("AuthServer").GetValue<string>("LocalSSL");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(60);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaPageViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/AdminArea/{2}/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/AdminArea/{2}/Views/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            services.AddRouting();

            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.AllowAreas = true;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseMvc(routes =>
            {
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
