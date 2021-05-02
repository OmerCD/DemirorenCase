using System;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MassTransit;

namespace DemirorenCase.Common.Services
{
    public class RabbitQueue<T> : IRabbitQueue<T> where T : class
    {
        private IBusControl _control;
        private Action<T> _act;

        public RabbitQueue(string url, string userName, string password, string queueName)
        {
            InitConnection(url, userName, password, queueName);
        }

        public RabbitQueue(Action<T> act)
        {
            this._act = act;
        }

        public void InitConnection(string url, string userName, string password, string queueName)
        {
            _control = Bus.Factory.CreateUsingRabbitMq(cfg =>
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
            if (_control != null)
            {
                _control.Start();
            }
        }

        private Task HandlerMethod(ConsumeContext<T> context)
        {
            this._act(context.Message);
            return Task.CompletedTask;
        }

        public void SetConsumer(Action<T> act)
        {
            this._act = act;
        }

        public void Publish(T input)
        {
            if (_control != null)
            {
                _control.Publish(input);
            }
        }
    }
}