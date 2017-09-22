﻿using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using Framework.Authentication.JwtBearer;
using Framework.Authentication.JwtBearer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Reminder.API.Infrastructure;
using Reminder.API.IntegrationEvents.EventHandling;
using Reminder.API.IntegrationEvents.Events;

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

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Service configuration
            services.Configure<JwtSecurityTokenOptions>(Configuration.GetSection("JwtSecurityToken"));

            // DbContext (PostgreSQL)
            services.AddDbContext<ReminderContext>(options =>
                options.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"],
                    x => x.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name)));

            // RabbitMQ
            RegisterEventBus(services);

            // MVC
            services.AddMvc();

            // Identity
            var jwtOptions = new JwtSecurityTokenOptions();
            Configuration.GetSection("JwtSecurityToken").Bind(jwtOptions);
            services.ConfigureJwtAuthentication(jwtOptions, _env);

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
            // TODO: Move to Audit microservice
            services.AddTransient<ReminderCreatedIntegrationEventHandler>();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            // TODO: Move to Audit microservice
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<ReminderCreatedIntegrationEvent, ReminderCreatedIntegrationEventHandler>();
        }
    }
}
