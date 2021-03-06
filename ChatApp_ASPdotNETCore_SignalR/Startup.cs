using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp_ASPdotNETCore_SignalR.Database;
using ChatApp_ASPdotNETCore_SignalR.Models;
using ChatApp_ASPdotNETCore_SignalR.Infrastructure.Respository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ChatApp_ASPdotNETCore_SignalR.Hubs;
using Microsoft.AspNetCore.Http;

namespace ChatApp_ASPdotNETCore_SignalR
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
            services.AddControllersWithViews();
            var connection = Configuration.GetConnectionString("ChatAppDB");
            services.AddDbContext<ChatAppDbContext>(options =>
            options.UseSqlServer(connection));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<ChatAppDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IChatRepository, ChatRepository>();


            services.AddSignalR(); 
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSignalR(routes => {
                routes.MapHub<ChatHub>("/chatHub");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
