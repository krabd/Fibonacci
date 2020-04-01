using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EasyNetQ;
using Fibonacci.Core.Interfaces;
using Fibonacci.DataAccess.Interfaces;
using Newtonsoft.Json;

namespace Fibonacci.MQ.Services
{
    public class MainService : IDisposable
    {
        private readonly IRabbitSettings _rabbitSettings;
        private readonly IFibonacciService _fibonacciService;

        private IBus _bus;
        private ISubscriptionResult _startSubscription;

        public MainService(IRabbitSettings rabbitSettings, IFibonacciService fibonacciService)
        {
            _rabbitSettings = rabbitSettings;
            _fibonacciService = fibonacciService;
        }

        public void Start()
        {
            try
            {
                _bus = RabbitHutch.CreateBus(_rabbitSettings.ConnectionString);

                _startSubscription = _bus.SubscribeAsync<string>("Fibonacci", OnReceiveFibonacciMessageAsync, x => x.WithTopic($"{_rabbitSettings.StartTopicName}.*"));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private async Task OnReceiveFibonacciMessageAsync(string message)
        {
            try
            {
                var currentNumber = JsonConvert.DeserializeObject<ulong>(message);
                await _fibonacciService.ProcessNextNumberAsync(currentNumber);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void Dispose()
        {
            _bus?.Dispose();
            _startSubscription?.Dispose();
        }
    }
}
