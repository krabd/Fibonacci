using System.Threading;
using System.Threading.Tasks;

namespace Fibonacci.Core.Interfaces
{
    public interface ICalculateFibonacciService
    {
        Task<ulong> CalculateNextNumberAsync(ulong previousNumber, ulong currentNumber, CancellationToken token = default);
    }
}
