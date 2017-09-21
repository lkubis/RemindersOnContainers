using System.Reflection;
using Framework.Authentication.JwtBearer;
using Framework.Authentication.JwtBearer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reminder.API.Infrastructure;

namespace Reminder.API
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Service configuration
            services.Configure<JwtSecurityTokenOptions>(Configuration.GetSection("JwtSecurityToken"));

            // DbContext (PostgreSQL)
            services.AddDbContext<ReminderContext>(options =>
                options.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"],
                    x => x.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name)));

            // MVC
            services.AddMvc();

            // Identity
            var jwtOptions = new JwtSecurityTokenOptions();
            Configuration.GetSection("JwtSecurityToken").Bind(jwtOptions);
            services.ConfigureJwtAuthentication(jwtOptions, _env);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
