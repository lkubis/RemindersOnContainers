using System;
using Identity.API.Data;
using Identity.API.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.API.Extensions
{
    public static class IWebHostExtensions
    {
        public static IWebHost InitializeDb(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                    var dbInitializer = new DbInitializer(context, userManager, roleManager);
                    dbInitializer.SeedAsync().Wait();
                    return host;
                }
                catch (Exception exception)
                {
                    // TODO: Handle exception
                    throw;
                }
            }
        }
    }
}