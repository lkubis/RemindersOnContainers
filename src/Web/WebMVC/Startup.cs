using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Framework.Authentication.JwtBearer;
using Framework.Authentication.JwtBearer.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Resilience.Http;
using WebMVC.Services;

namespace WebMVC
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
            // Service configuration
            services.AddOptions();
            services.Configure<AppSettings>(Configuration);
            services.Configure<IOptions<JwtSecurityTokenOptions>>(Configuration.GetSection("JwtSecurityToken"));

            // Health check
            services.AddHealthChecks(checks =>
            {
                var minutes = 1;
                if (int.TryParse(Configuration["HealthCheck:Timeout"], out var minutesParsed))
                    minutes = minutesParsed;

                checks.AddUrlCheck(Configuration["IdentityUrl"] + "/hc", TimeSpan.FromMinutes(minutes));
                checks.AddUrlCheck(Configuration["ReminderUrl"] + "/hc", TimeSpan.FromMinutes(minutes));
                checks.AddUrlCheck(Configuration["AuditUrl"] + "/hc", TimeSpan.FromMinutes(minutes));
            });

            // MVC
            services.AddMvc();

            // Identity
            var jwtOptions = new JwtSecurityTokenOptions();
            Configuration.GetSection("JwtSecurityToken").Bind(jwtOptions);

            var tokenValidationParameters = new TokenValidationParameters();
            tokenValidationParameters.AddOptions(jwtOptions);

            // Turn off Microsoft's JWT handler that maps claim types to .NET's long claim type names
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();

            services.AddAuthentication(options =>
                {
                    //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.Cookie.Name = "authCookie";
                    options.TicketDataFormat = new JwtTicketDataFormat(
                        SecurityAlgorithms.HmacSha256,
                        tokenValidationParameters);
                });

            // Add application services
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IReminderService, ReminderService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpClient, StandardHttpClient>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Reminders}/{action=Index}/{id?}");
            });
        }
    }
}
