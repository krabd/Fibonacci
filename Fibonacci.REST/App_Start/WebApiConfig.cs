using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Fibonacci.Core.Extensions;
using Fibonacci.DataAccess.Interfaces;
using Fibonacci.DataAccess.Models;
using Fibonacci.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Fibonacci.REST
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var startupService = new StartupService();
            var services = startupService.Configure();

            services.AddSingleton<IRabbitSettings>(p =>
            {
                var rabbitSettings = new RabbitSettings();
                rabbitSettings.Set(WebConfigurationManager.AppSettings.Get("rabbitConnectionString"),
                    WebConfigurationManager.AppSettings.Get("startTopicName"),
                    WebConfigurationManager.AppSettings.Get("mainTopicName"));
                return rabbitSettings;
            });

            services.AddTransient<IFibonacciRepository>(p =>
            {
                var repository = new RabbitFibonacciRepository(p.Resolve<IRabbitSettings>());
                return repository;
            });

            var provider = startupService.BuildProvider(services);

            GlobalConfiguration.Configuration.DependencyResolver = new DefaultDependencyResolver(provider);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
            );
        }
    }

    public class DefaultDependencyResolver : IDependencyResolver
    {
        protected IServiceProvider ServiceProvider { get; set; }

        public DefaultDependencyResolver(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ServiceProvider.GetServices(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return new DefaultDependencyResolver(ServiceProvider.CreateScope().ServiceProvider);
        }

        public void Dispose()
        {
            // you can implement this interface just when you use .net core 2.0
            // this.ServiceProvider.Dispose();
        }
    }
}
