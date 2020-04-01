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
        private readonly ISessionSettings _sessionSettings;
        private readonly IFibonacciService _fibonacciService;

        private IBus _bus;

        public MainService(IRabbitSettings rabbitSettings, ISessionSettings sessionSettings, IFibonacciService fibonacciService)
        {
            _rabbitSettings = rabbitSettings;
            _sessionSettings = sessionSettings;
            _fibonacciService = fibonacciService;
        }

        public void Start()
        {
            try
            {
                _bus = RabbitHutch.CreateBus(_rabbitSettings.ConnectionString);

                _bus.SubscribeAsync<string>(_rabbitSettings.StartTopicName, OnReceiveFibonacciParallelCountAsync);
                _bus.SubscribeAsync<string>(_rabbitSettings.MainTopicName, OnReceiveFibonacciMessageAsync);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private async Task OnReceiveFibonacciParallelCountAsync(string message)
        {
            try
            {
                _sessionSettings.ParallelCount = Convert.ToInt32(message);

                await _fibonacciService.ProcessNextNumberAsync(1);
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
        }
    }
}
