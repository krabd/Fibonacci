using Fibonacci.Core.Interfaces;
using Fibonacci.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fibonacci.Core
{
    public static class Configurator
    {
        public static IServiceCollection ConfigureCore(this IServiceCollection services)
        {
            services.AddTransient<ICalculateFibonacciService, CalculateFibonacciService>();
            services.AddTransient<IFibonacciService, FibonacciService>();

            services.AddSingleton<ISessionSettings, SessionSettings>();
            
            return services;
        }
    }
}
