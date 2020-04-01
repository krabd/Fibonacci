using System.Threading.Tasks;
using System.Web.Http;
using EasyNetQ;
using Fibonacci.Core.Interfaces;
using Fibonacci.DataAccess.Interfaces;
using Newtonsoft.Json;

namespace Fibonacci.REST.Controllers
{
    public class FibonacciController : ApiController
    {
        private readonly IRabbitSettings _rabbitSettings;
        private readonly ISessionSettings _sessionSettings;
        private readonly IFibonacciService _fibonacciService;

        public FibonacciController(IRabbitSettings rabbitSettings, ISessionSettings sessionSettings, IFibonacciService fibonacciService)
        {
            _rabbitSettings = rabbitSettings;
            _sessionSettings = sessionSettings;
            _fibonacciService = fibonacciService;
        }

        [HttpGet]
        public string Get([FromUri] int count)
        {
            _sessionSettings.ParallelCount = count;

            using (var bus = RabbitHutch.CreateBus(_rabbitSettings.ConnectionString))
            {
                bus.Publish(count.ToString(), _rabbitSettings.StartTopicName);
            }

            return "I am Live";
        }

        [HttpPost]
        public async Task Start([FromBody] string value)
        {
            var currentNumber = JsonConvert.DeserializeObject<ulong>(value);
            await _fibonacciService.ProcessNextNumberAsync(currentNumber);
        }
    }
}
