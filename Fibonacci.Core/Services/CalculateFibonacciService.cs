using System.Threading;
using System.Threading.Tasks;
using Fibonacci.Core.Interfaces;

namespace Fibonacci.Core.Services
{
    public class CalculateFibonacciService : ICalculateFibonacciService
    {
        public Task<ulong> CalculateNextNumberAsync(ulong currentNumber, CancellationToken token = default)
        {
            return Task.Run(() =>
            {
                ulong firstNumber = 0;
                ulong secondNumber = 1;

                ulong newNumber = 0;

                while (currentNumber >= newNumber)
                {
                    newNumber = firstNumber + secondNumber;

                    firstNumber = secondNumber;
                    secondNumber = newNumber;
                }

                return newNumber;
            }, token);
        }
    }
}
