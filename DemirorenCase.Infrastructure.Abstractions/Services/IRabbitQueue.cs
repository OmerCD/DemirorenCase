namespace DemirorenCase.Infrastructure.Abstractions.Services
{
    public interface IRabbitQueue<in T> where T : class
    {
        void InitConnection(string url, string userName, string password, string queueName);
        void Start();
        void Publish(T input);
    }
}