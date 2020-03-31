using System;
using Fibonacci.Core;
using Fibonacci.MQ.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fibonacci.MQ
{
    public class StartupService
    {
        public IServiceCollection Configure()
        {
            var services = new ServiceCollection();

            services.AddTransient<FibonacciService>();
            services.ConfigureCore();

            return services;
        }

        public IServiceProvider BuildProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }
    }
}
