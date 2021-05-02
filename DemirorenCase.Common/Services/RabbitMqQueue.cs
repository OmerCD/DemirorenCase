using System;
using System.Threading.Tasks;
using MassTransit;

namespace DemirorenCase.Common.Services
{
    public interface IRabbitQueue<T> where T : class
    {
        void InitConnection(string url, string userName, string password, string queueName);
        void Start();
        void Publish(T input);
    }
    public class RabbitQueue<T> : IRabbitQueue<T> where T : class
    {
        private IBusControl control;
        private Action<T> act;

        public RabbitQueue()
        {

        }
        public RabbitQueue(Action<T> act)
        {
            this.act = act;

        }
        public void InitConnection(string url, string userName, string password, string queueName)
        {
            control = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(url), h =>
                {
                    h.Username(userName);
                    h.Password(password);
                });
                cfg.ReceiveEndpoint(queueName, x => x.Handler<T>(HandlerMethod));
            });
        }
        public void Start()
        {
            if (control != null)
            {
                control.Start();
            }
        }
        private Task HandlerMethod(ConsumeContext<T> context)
        {
            this.act(context.Message);
            return Task.CompletedTask;
        }
        public void SetConsumer(Action<T> act)
        {
            this.act = act;
        }
        public void Publish(T input)
        {
            if (control != null)
            {
                control.Publish(input);
            }
        }
    }
}