using System.Threading;
using System.Threading.Tasks;

namespace Fibonacci.DataAccess.Interfaces
{
    public interface IFibonacciRepository
    {
        Task SendNextNumberAsync(ulong number, CancellationToken token = default);
    }
}
