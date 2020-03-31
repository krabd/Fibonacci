using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Fibonacci.Core.Interfaces;
using Fibonacci.DataAccess.Interfaces;
using Newtonsoft.Json;

namespace Fibonacci.MQ.Services
{
    public class FibonacciService : IDisposable
    {
        private readonly IRabbitSettings _rabbitSettings;
        private readonly IFibonacciRepository _fibonacciRepository;
        private readonly ICalculateFibonacciService _calculateService;
        private readonly ISessionSettings _sessionSettings;

        private IBus _bus;

        public FibonacciService(IRabbitSettings rabbitSettings, IFibonacciRepository fibonacciRepository, ICalculateFibonacciService calculateService, ISessionSettings sessionSettings)
        {
            _rabbitSettings = rabbitSettings;
            _fibonacciRepository = fibonacciRepository;
            _calculateService = calculateService;
            _sessionSettings = sessionSettings;
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

                await ProcessNextNumberAsync(1);
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
                await ProcessNextNumberAsync(currentNumber);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private async Task ProcessNextNumberAsync(ulong currentNumber, CancellationToken token = default)
        {
            var calculateTasks = Enumerable.Range(0, _sessionSettings.ParallelCount).Select(i =>
                _calculateService.CalculateNextNumberAsync(_sessionSettings.LastNumber, currentNumber, token)).ToList();

            await Task.WhenAll(calculateTasks);

            Console.WriteLine($"Prev = {currentNumber}. Current = {_sessionSettings.LastNumber}");

            _sessionSettings.LastNumber = calculateTasks.First().Result;
            await _fibonacciRepository.SendNextNumberAsync(_sessionSettings.LastNumber, token);
        }

        public void Dispose()
        {
            _bus?.Dispose();
        }
    }
}
