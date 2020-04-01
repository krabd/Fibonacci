namespace Fibonacci.DataAccess.Interfaces
{
    public interface IRabbitSettings
    {
        string ConnectionString { get; }

        string StartTopicName { get; }
    }
}