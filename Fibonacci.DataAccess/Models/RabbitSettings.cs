using Fibonacci.DataAccess.Interfaces;

namespace Fibonacci.DataAccess.Models
{
    public class RabbitSettings : IRabbitSettings
    {
        public string ConnectionString { get; private set; }

        public string StartTopicName { get; private set; }

        public void Set(string connectionString, string startTopicName)
        {
            ConnectionString = connectionString;
            StartTopicName = startTopicName;
        }
    }
}