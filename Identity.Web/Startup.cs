using System.Runtime.InteropServices;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.DataAccess.Context;
using Identity.Entity;
using Identity.Service.CustomValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Identity.Service.Utilities;

namespace Identity.Web
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddDbContext<IdentityContext>(opt =>
         opt.UseSqlServer(Configuration.GetConnectionString("IdentityConnectionStr"), b => b.MigrationsAssembly("Identity.Web")));

         services.AddAuthorization(opt =>
         {
            opt.AddPolicy("İstanbulPolicy", policy =>
            {
               policy.RequireClaim("city", "İstanbul");
            });
            opt.AddPolicy("ViolencePolicy", policy =>
            {
               policy.RequireClaim("violence");
            });
            opt.AddPolicy("ExchangePolicy", policy =>
            {
               policy.AddRequirements(new ExpireDateExchangeRequirement());
            });
         });

         services.AddAuthentication()
         .AddFacebook(opt =>
         {
            opt.AppId = Configuration["Authentication:Facebook:AppId"];
            opt.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
         })
         .AddGoogle(opt=>{
            opt.ClientId = Configuration["Authentication:Google:ClientID"];
            opt.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
         });

         services.AddIdentity<User, Role>(opt =>
         {
            opt.User.AllowedUserNameCharacters = "abcçdefgğhıijklmnoöpqrsştuüvwxyzABCÇDEFGĞHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._@+#=<>&%^'!";
            opt.User.RequireUniqueEmail = true;

            opt.Password.RequiredLength = 4;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireDigit = false;
         }).AddPasswordValidator<CustomePasswordValidator>()
         .AddUserValidator<CustomUserValidator>()
         .AddErrorDescriber<CustomeIdentityErrorDescriber>()
         .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();



         CookieBuilder cookieBuilder = new CookieBuilder();
         cookieBuilder.Name = "MyPrettyCookie";
         cookieBuilder.HttpOnly = true;
         cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;
         cookieBuilder.SameSite = SameSiteMode.Strict;

         services.ConfigureApplicationCookie(opt =>
         {
            opt.LoginPath = new PathString("/Login/SignIn");
            opt.LogoutPath = new PathString("/Home/Logout");
            opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
            opt.Cookie = cookieBuilder;
            opt.ExpireTimeSpan = TimeSpan.FromDays(60);
            opt.SlidingExpiration = true;
         });


         services.AddControllersWithViews();

      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

         app.UseRouting();
         app.UseAuthentication();
         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Admin}/{action=Home}/{id?}");
         });
      }
   }
}
