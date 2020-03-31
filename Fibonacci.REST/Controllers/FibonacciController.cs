using System;
using System.Diagnostics;
using System.Web.Http;
using EasyNetQ;
using Fibonacci.REST.Models;
using Newtonsoft.Json;

namespace Fibonacci.REST.Controllers
{
    public class FibonacciController : ApiController
    {
        private const string TOPIC_NAME = "FibonacciTopic";
        private const string TOPIC_PAR_NAME = "FibonacciParTopic";

        [HttpGet]
        public string Get()
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                bus.Publish("5", TOPIC_PAR_NAME);
            }
            return "I am Live";
        }

        [HttpPost]
        public void Start([FromBody] FibonacciMessage value)
        {
            Publish(value);
        }

        private void Publish(FibonacciMessage value)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                var message = new FibonacciMessage();

                var prev = value.Prev;
                var current = value.Current == 0 ? 1 : value.Current;
                message.Prev = current;
                message.Current = current + prev;

                Debug.WriteLine($"Prev = {message.Prev}, Current = {message.Current}");

                var json = JsonConvert.SerializeObject(message);
                bus.Publish(json, TOPIC_NAME);
            }
        }
    }
}
