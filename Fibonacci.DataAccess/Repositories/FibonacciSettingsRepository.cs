using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Fibonacci.DataAccess.Interfaces;

namespace Fibonacci.DataAccess.Repositories
{
    public class FibonacciSettingsRepository : IFibonacciSettingsRepository
    {
        private readonly IRabbitSettings _rabbitSettings;

        public FibonacciSettingsRepository(IRabbitSettings rabbitSettings)
        {
            _rabbitSettings = rabbitSettings;
        }

        public async Task SetParallelCountAsync(int count, CancellationToken token = default)
        {
            using (var bus = RabbitHutch.CreateBus(_rabbitSettings.ConnectionString))
            {
                bus.Publish(count.ToString(), _rabbitSettings.StartTopicName);
            }
        }
    }
}
