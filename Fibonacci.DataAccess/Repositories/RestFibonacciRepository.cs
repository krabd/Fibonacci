using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fibonacci.DataAccess.Interfaces;
using Newtonsoft.Json;

namespace Fibonacci.DataAccess.Repositories
{
    public class RestFibonacciRepository : IFibonacciRepository
    {
        private string _uri;

        public void SetUri(string uri)
        {
            _uri = uri;
        }

        public async Task SendNextNumberAsync(ulong number, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(_uri))
                throw new ArgumentException("Не задан Uri для запроса по API");

            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(number), Encoding.UTF8, "application/json");
                await client.PostAsync(_uri, content, token);
            }
        }
    }
}
