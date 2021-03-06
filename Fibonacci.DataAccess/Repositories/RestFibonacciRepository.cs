﻿using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fibonacci.DataAccess.Interfaces;
using Newtonsoft.Json;

namespace Fibonacci.DataAccess.Repositories
{
    public class RestFibonacciRepository : IFibonacciRepository
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public RestFibonacciRepository(IHttpClientFactory httpClientFactory, string baseUrl)
        {
            _baseUrl = baseUrl;
            _client = httpClientFactory.CreateClient();
        }

        public async Task SendNextNumberAsync(ulong number, CancellationToken token = default)
        {
            var content = new StringContent(JsonConvert.SerializeObject(number), Encoding.UTF8, "application/json");
            await _client.PostAsync(_baseUrl, content, token);
        }
    }
}
