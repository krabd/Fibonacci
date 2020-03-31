namespace Fibonacci.MQ.Models
{
    public class FibonacciMessage
    {
        public ulong Prev { get; set; }

        public ulong Current { get; set; }
    }
}
