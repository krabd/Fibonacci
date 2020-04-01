using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ.Management.Client;
using Fibonacci.Core.Extensions;
using Fibonacci.DataAccess.Interfaces;
using Fibonacci.DataAccess.Models;
using Fibonacci.DataAccess.Repositories;
using Fibonacci.MQ.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fibonacci.MQ
{
    class Program
    {
        private static MainService _mainService;

        static async Task Main(string[] args)
        {
            var startupService = new StartupService();
            var services = startupService.Configure();

            services.AddSingleton<IRabbitSettings>(p =>
            {
                var rabbitSettings = new RabbitSettings();
                rabbitSettings.Set(ConfigurationManager.AppSettings.Get("rabbitConnectionString"),
                    ConfigurationManager.AppSettings.Get("startTopicName"),
                    ConfigurationManager.AppSettings.Get("mainTopicName"));
                return rabbitSettings;
            });

            services.AddTransient<IFibonacciRepository>(p =>
            {
                var repository = new RestFibonacciRepository();
                repository.SetUri(ConfigurationManager.AppSettings.Get("apiUri"));
                return repository;
            });

            var provider = startupService.BuildProvider(services);

            try
            {
                var client = new ManagementClient(ConfigurationManager.AppSettings.Get("rabbitUri"),
                    ConfigurationManager.AppSettings.Get("rabbitUser"),
                    ConfigurationManager.AppSettings.Get("rabbitPass"));
                var queues = await client.GetQueuesAsync();
                var queuePurgeTasks = queues.Where(i => i.Name.Contains("Fibonacci")).Select(i => client.PurgeAsync(i));
                await Task.WhenAll(queuePurgeTasks.Where(t => t != null));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            _mainService = provider.Resolve<MainService>();
            _mainService.Start();
        }
    }
}
