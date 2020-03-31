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

        [HttpGet]
        public string Get()
        {
            return "I am Live";
        }

        [HttpPost]
        public void Start([FromBody] FibonacciMessage value)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                var message = new FibonacciMessage();

                if (value is null)
                {
                    message.Prev = 0;
                    message.Current = 1;
                }
                else
                {
                    var prev = value.Prev;
                    var current = value.Current;
                    message.Prev = current;
                    message.Current = current + prev;
                }

                Debug.WriteLine($"Prev = {message.Prev}, Current = {message.Current}");

                var json = JsonConvert.SerializeObject(message);
                bus.Publish(json, TOPIC_NAME);
            }
        }
    }
}
