using System.Threading;
using System.Threading.Tasks;
using Fibonacci.Core.Interfaces;

namespace Fibonacci.Core.Services
{
    public class CalculateFibonacciService : ICalculateFibonacciService
    {
        public async Task<ulong> CalculateNextNumberAsync(ulong previousNumber, ulong currentNumber, CancellationToken token = default)
        {
            return previousNumber 
                + currentNumber != 0 ? currentNumber : 1;
        }
    }
}
