using System.Reflection;
using Framework.Authentication.JwtBearer;
using Identity.API.Configuration;
using Identity.API.Data;
using Identity.API.Extensions;
using Identity.API.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.API
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
            services.AddOptions();
            services.Configure<ServiceConfiguration>(Configuration);
            services.Configure<JwtSecurityTokenOptions>(Configuration.GetSection("IdentityOptions:JwtSecurityToken"));

            // DbContext (PostgreSQL)
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"],
                x => x.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name)));

            // MVC
            services.AddMvc();

            // Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.ConfigureIdentityOptions(Configuration);
            services.ConfigureJwtAuthentication(Configuration, _env);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
