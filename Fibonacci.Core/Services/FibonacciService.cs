using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fibonacci.Core.Interfaces;
using Fibonacci.DataAccess.Interfaces;

namespace Fibonacci.Core.Services
{
    public class FibonacciService : IFibonacciService
    {
        private readonly IFibonacciRepository _fibonacciRepository;
        private readonly ICalculateFibonacciService _calculateService;
        private readonly ISessionSettings _sessionSettings;

        public FibonacciService(IFibonacciRepository fibonacciRepository, ICalculateFibonacciService calculateService, ISessionSettings sessionSettings)
        {
            _fibonacciRepository = fibonacciRepository;
            _calculateService = calculateService;
            _sessionSettings = sessionSettings;
        }

        public async Task ProcessNextNumberAsync(ulong currentNumber, CancellationToken token = default)
        {
            var calculateTasks = Enumerable.Range(0, _sessionSettings.ParallelCount).Select(i =>
                _calculateService.CalculateNextNumberAsync(_sessionSettings.LastNumber, currentNumber, token)).ToList();

            await Task.WhenAll(calculateTasks);

            _sessionSettings.LastNumber = calculateTasks.First().Result;

            Console.WriteLine($"Prev = {currentNumber}. Current = {_sessionSettings.LastNumber}");

            await _fibonacciRepository.SendNextNumberAsync(_sessionSettings.LastNumber, token);
        }
    }
}
