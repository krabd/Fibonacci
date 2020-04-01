using System.Threading.Tasks;
using System.Web.Http;
using Fibonacci.Core.Interfaces;
using Fibonacci.DataAccess.Interfaces;
using Newtonsoft.Json;

namespace Fibonacci.REST.Controllers
{
    public class FibonacciController : ApiController
    {
        private readonly ISessionSettings _sessionSettings;
        private readonly IFibonacciService _fibonacciService;
        private readonly IFibonacciSettingsRepository _fibonacciSettingsRepository;

        public FibonacciController(ISessionSettings sessionSettings, IFibonacciService fibonacciService, IFibonacciSettingsRepository fibonacciSettingsRepository)
        {
            _sessionSettings = sessionSettings;
            _fibonacciService = fibonacciService;
            _fibonacciSettingsRepository = fibonacciSettingsRepository;
        }

        [HttpGet]
        public string Get([FromUri] int count)
        {
            _sessionSettings.ParallelCount = count;
            _fibonacciSettingsRepository.SetParallelCountAsync(count);

            return $"Count has been set ({count})";
        }

        [HttpPost]
        public async Task Start([FromBody] string value)
        {
            var currentNumber = JsonConvert.DeserializeObject<ulong>(value);
            await _fibonacciService.ProcessNextNumberAsync(currentNumber);
        }
    }
}
