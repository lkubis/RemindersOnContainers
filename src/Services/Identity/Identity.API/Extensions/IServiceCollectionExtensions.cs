using System;
using Identity.API.Configuration.Sections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureIdentityOptions(
            this IServiceCollection services, 
            IConfigurationRoot configuration)
        {
            services.Configure<Microsoft.AspNetCore.Identity.IdentityOptions>(options =>
            {
                var o = new IdentityOptions();
                configuration.GetSection("IdentityOptions").Bind(o);

                // Password settings
                options.Password.RequireDigit = o.Password.RequireDigit;
                options.Password.RequiredLength = o.Password.RequiredLength;
                options.Password.RequireNonAlphanumeric = o.Password.RequireNonAlphanumeric;
                options.Password.RequireUppercase = o.Password.RequireUppercase;
                options.Password.RequireLowercase = o.Password.RequireLowercase;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(o.Lockout.DefaultLockoutTimeSpan);
                options.Lockout.MaxFailedAccessAttempts = o.Lockout.MaxFailedAccessAttempts;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }
    }
}