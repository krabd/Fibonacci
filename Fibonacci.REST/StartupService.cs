using System;
using Fibonacci.Core;
using Fibonacci.DataAccess;
using Fibonacci.REST.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Fibonacci.REST
{
    public class StartupService
    {
        public IServiceCollection Configure()
        {
            var services = new ServiceCollection();

            services.AddTransient<FibonacciController>();
            services.ConfigureCore();
            services.ConfigureDataAccess();

            return services;
        }

        public IServiceProvider BuildProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }
    }
}
