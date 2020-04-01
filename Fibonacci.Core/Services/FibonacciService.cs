using System;
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

        public FibonacciService(IFibonacciRepository fibonacciRepository, ICalculateFibonacciService calculateService)
        {
            _fibonacciRepository = fibonacciRepository;
            _calculateService = calculateService;
        }

        public async Task ProcessNextNumberAsync(ulong currentNumber, CancellationToken token = default)
        {
            var newNumber  = await _calculateService.CalculateNextNumberAsync(currentNumber, token);

            Console.WriteLine($"Prev = {currentNumber}. Current = {newNumber}");

            await _fibonacciRepository.SendNextNumberAsync(newNumber, token);
        }
    }
}
