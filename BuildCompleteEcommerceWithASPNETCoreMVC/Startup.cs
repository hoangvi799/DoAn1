using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DoAn1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoAn1
{
    public class Startup
    {
        public IConfiguration configuration { get; }

        public Startup(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "User_Schema";
            })
            .AddCookie("User_Schema", options =>
              {
                  options.LoginPath = "/customer/login";
                  options.LogoutPath = "/customer/sigout";
                  options.AccessDeniedPath = "/customer/accessdenied";
              })
              .AddCookie("Admin_Schema", options =>
              {
                  options.LoginPath = "/admin/login/index";
                  options.LogoutPath = "/admin/login/signout";
                  options.AccessDeniedPath = "/admin/account/accessdenied";
              });

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //.AddCookie(options =>
            //{
            //    options.LoginPath = "/admin/login/index";
            //    options.LogoutPath = "/admin/login/signout";
            //    options.AccessDeniedPath = "/admin/account/accessdenied";
            //});
            services.AddSession();

            services.AddMvc();

            services.AddMvc().AddSessionStateTempDataProvider();

            var connection = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DatabaseContext>(options => options.UseLazyLoadingProxies
            ().UseSqlServer(connection));
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                var principal = new ClaimsPrincipal();

                var result1 = await context.AuthenticateAsync("User_Schema");
                if (result1?.Principal != null)
                {
                    principal.AddIdentities(result1.Principal.Identities);
                }

                var result2 = await context.AuthenticateAsync("Admin_Schema");
                if (result2?.Principal != null)
                {
                    principal.AddIdentities(result2.Principal.Identities);
                }

                context.User = principal;

                await next();

            });
            app.UseAuthentication();

            app.UseSession();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
