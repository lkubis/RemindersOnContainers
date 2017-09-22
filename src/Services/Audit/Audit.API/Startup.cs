using System;
using Audit.API.Infrastructure.Repositories;
using Audit.API.IntegrationEvents.EventHandling;
using Audit.API.IntegrationEvents.Events;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using Framework.Authentication.JwtBearer;
using Framework.Authentication.JwtBearer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Audit.API
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

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Service configuration
            services.Configure<JwtSecurityTokenOptions>(Configuration.GetSection("JwtSecurityToken"));
            services.Configure<AuditSettings>(Configuration);

            // RabbitMQ
            RegisterEventBus(services);

            // MVC
            services.AddMvc();

            // Identity
            var jwtOptions = new JwtSecurityTokenOptions();
            Configuration.GetSection("JwtSecurityToken").Bind(jwtOptions);
            services.ConfigureJwtAuthentication(jwtOptions, _env);

            services.AddTransient<IMessageRepository, MessageRepository>();

            var container = new ContainerBuilder();
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());
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

            ConfigureEventBus(app);
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBusConnection"],
                    Port = AmqpTcpEndpoint.UseDefaultPort
                };
                return new DefaultRabbitMQPersistentConnection(factory, logger);
            });
            services.AddSingleton<IEventBus, EventBusRabbitMQ.EventBusRabbitMQ>();
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<ReminderCreatedIntegrationEventHandler>();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<ReminderCreatedIntegrationEvent, ReminderCreatedIntegrationEventHandler>();
        }
    }
}
