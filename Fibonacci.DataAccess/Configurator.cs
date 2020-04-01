using Fibonacci.DataAccess.Interfaces;
using Fibonacci.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Fibonacci.DataAccess
{
    public static class Configurator
    {
        public static IServiceCollection ConfigureDataAccess(this IServiceCollection services)
        {
            services.AddTransient<IFibonacciSettingsRepository, FibonacciSettingsRepository>();
            
            return services;
        }
    }
}
