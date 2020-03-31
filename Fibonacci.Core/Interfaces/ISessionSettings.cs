namespace Fibonacci.Core.Interfaces
{
    public interface ISessionSettings
    {
        int ParallelCount { get; set; }

        ulong LastNumber { get; set; }
    }
}
