using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Fibonacci.Core.Interfaces;
using Newtonsoft.Json;

namespace Fibonacci.REST.Controllers
{
    public class FibonacciController : ApiController
    {
        private readonly IFibonacciService _fibonacciService;

        public FibonacciController(IFibonacciService fibonacciService)
        {
            _fibonacciService = fibonacciService;
        }

        [HttpGet]
        public Task Get([FromUri] int count)
        {
            var startTasks = Enumerable.Range(0, count).Select(i => _fibonacciService.ProcessNextNumberAsync(0));
            return Task.WhenAll(startTasks);
        }

        [HttpPost]
        public async Task Start([FromBody] string value)
        {
            var currentNumber = JsonConvert.DeserializeObject<ulong>(value);
            await _fibonacciService.ProcessNextNumberAsync(currentNumber);
        }
    }
}
