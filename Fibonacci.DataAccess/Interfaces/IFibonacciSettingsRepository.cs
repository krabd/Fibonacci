using System.Threading;
using System.Threading.Tasks;

namespace Fibonacci.DataAccess.Interfaces
{
    public interface IFibonacciSettingsRepository
    {
        Task SetParallelCountAsync(int count, CancellationToken token = default);
    }
}
