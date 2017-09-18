using System;
using Identity.API.Configuration.Sections;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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

        public static IServiceCollection ConfigureJwtAuthentication(
            this IServiceCollection services,
            IConfigurationRoot configuration,
            IHostingEnvironment env)
        {
            var o = new IdentityOptions();
            configuration.GetSection("IdentityOptions").Bind(o);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = !env.IsDevelopment();

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = o.JwtSecurityToken.SymetricSecurityKey,

                        ValidateIssuer = true,
                        ValidIssuer = o.JwtSecurityToken.Issuer,

                        ValidateAudience = true,
                        ValidAudience = o.JwtSecurityToken.Audience,

                        ValidateLifetime = true,
                    };
                });
            return services;
        }
    }
}