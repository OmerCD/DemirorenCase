namespace DemirorenCase.LoggerWorkerService.ValueObjects
{
    public class RabbitMqOptions
    {
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
    }
}