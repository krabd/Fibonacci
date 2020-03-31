using Fibonacci.DataAccess.Interfaces;

namespace Fibonacci.DataAccess.Models
{
    public class RabbitSettings : IRabbitSettings
    {
        public string ConnectionString { get; private set; }

        public string StartTopicName { get; private set; }

        public string MainTopicName { get; private set; }

        public void Set(string connectionString, string startTopicName, string mainTopicName)
        {
            ConnectionString = connectionString;
            StartTopicName = startTopicName;
            MainTopicName = mainTopicName;
        }
    }
}