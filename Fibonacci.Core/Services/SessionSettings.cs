using Fibonacci.Core.Interfaces;

namespace Fibonacci.Core.Services
{
    public class SessionSettings : ISessionSettings
    {
        public int ParallelCount { get; set; }

        public ulong LastNumber { get; set; }
    }
}
