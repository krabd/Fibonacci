using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Management.Client;
using EasyNetQ.Management.Client.Model;
using Fibonacci.MQ.Models;
using Newtonsoft.Json;

namespace Fibonacci.MQ
{
    class Program
    {
        private const string API_URI = "http://localhost:3479/api/fibonacci";

        private const string RABBIT_URI = "http://localhost";
        private const string RABBIT_USER = "guest";
        private const string RABBIT_PASS = "guest";

        private const string QUEUE_NAME = "Fibonacci";
        private const string TOPIC_NAME = "FibonacciTopic";
        private const string TOPIC_PAR_NAME = "FibonacciParTopic";

        private static IBus _bus;

        static async Task Main(string[] args)
        {
            try
            {
                var client = new ManagementClient(RABBIT_URI, RABBIT_USER, RABBIT_PASS);
                var queues = await client.GetQueuesAsync();
                var queuePurgeTasks = queues.Where(i => i.Name.Contains(QUEUE_NAME)).Select(i => client.PurgeAsync(i));
                await Task.WhenAll(queuePurgeTasks.Where(t => t != null));

                _bus = RabbitHutch.CreateBus("host=localhost");
                _bus.SubscribeAsync<string>(TOPIC_NAME, OnReceiveFibonacciMessage);
                _bus.SubscribeAsync<string>(TOPIC_PAR_NAME, OnReceiveFibonacciParMessage);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private static async Task OnReceiveFibonacciMessage(string message)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<FibonacciMessage>(message);

                var fibonacciMessage = new FibonacciMessage();
                var prev = result.Prev;
                var current = result.Current;
                fibonacciMessage.Prev = current;
                fibonacciMessage.Current = current + prev;

                Console.WriteLine($"Prev = {result.Prev}. Current = {result.Current}");

                await SendResultToApiAsync(fibonacciMessage);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private static async Task OnReceiveFibonacciParMessage(string arg)
        {
            try
            {
                var count = Convert.ToInt32(arg);

                await SendResultToApiAsync(new FibonacciMessage{Prev = 0, Current = 0});
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private static async Task SendResultToApiAsync(FibonacciMessage fibonacciMessage)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(fibonacciMessage), Encoding.UTF8, "application/json");
                await client.PostAsync(API_URI, content, CancellationToken.None);
            }
        }
    }
}
