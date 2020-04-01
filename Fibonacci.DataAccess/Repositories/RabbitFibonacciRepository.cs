using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Fibonacci.DataAccess.Interfaces;
using Newtonsoft.Json;

namespace Fibonacci.DataAccess.Repositories
{
    public class RabbitFibonacciRepository : IFibonacciRepository
    {
        private readonly IRabbitSettings _rabbitSettings;

        public RabbitFibonacciRepository(IRabbitSettings rabbitSettings)
        {
            _rabbitSettings = rabbitSettings;
        }

        public async Task SendNextNumberAsync(ulong number, CancellationToken token = default)
        {
            using (var bus = RabbitHutch.CreateBus(_rabbitSettings.ConnectionString))
            {
                var content = JsonConvert.SerializeObject(number);
                bus.Publish(content, _rabbitSettings.MainTopicName);
            }
        }
    }
}
