using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Reminder.API.Infrastructure;

namespace Reminder.API.Extensions
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
                    var context = services.GetRequiredService<ReminderContext>();

                    var dbInitializer = new DbInitializer(context);
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