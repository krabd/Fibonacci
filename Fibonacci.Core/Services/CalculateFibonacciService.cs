using System.Threading;
using System.Threading.Tasks;
using Fibonacci.Core.Interfaces;

namespace Fibonacci.Core.Services
{
    public class CalculateFibonacciService : ICalculateFibonacciService
    {
        public Task<ulong> CalculateNextNumberAsync(ulong currentNumber, CancellationToken token = default)
        {
            return Task.Run<ulong>(() =>
            {
                return 1;
            }, token);
        }
    }
}
