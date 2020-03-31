using System.Threading;
using System.Threading.Tasks;
using Fibonacci.DataAccess.Interfaces;

namespace Fibonacci.DataAccess.Repositories
{
    public class RabbitFibonacciRepository : IFibonacciRepository
    {
        private readonly IRabbitSettings _rabbitSettings;

        public RabbitFibonacciRepository(IRabbitSettings rabbitSettings)
        {
            _rabbitSettings = rabbitSettings;
        }

        public Task SendNextNumberAsync(ulong number, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
