using System;
using DemirorenCase.Common.Services;
using DemirorenCase.Core.Abstractions;

namespace DemirorenCase.LoggerWorkerService
{
    public class LogQueue : RabbitQueue<ILog>
    {
        public LogQueue(string url, string userName, string password, string queueName) : base(url, userName, password, queueName)
        {
        }

        public LogQueue(Action<ILog> act) : base(act)
        {
        }
    }
}