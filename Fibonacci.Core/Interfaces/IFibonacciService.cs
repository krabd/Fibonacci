using System.Threading;
using System.Threading.Tasks;

namespace Fibonacci.Core.Interfaces
{
    public interface IFibonacciService
    {
        Task ProcessNextNumberAsync(ulong currentNumber, CancellationToken token = default);
    }
}
