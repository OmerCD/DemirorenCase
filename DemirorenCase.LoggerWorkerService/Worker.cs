using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Core.Abstractions;
using DemirorenCase.Core.ValueObjects;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DemirorenCase.LoggerWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RabbitMqOptions _rabbitMqOptions;

        public Worker(ILogger<Worker> logger, IOptions<RabbitMqOptions> options)
        {
            _logger = logger;
            _rabbitMqOptions = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var imageFileQueue = new LogQueue(_rabbitMqOptions.Address, _rabbitMqOptions.UserName, _rabbitMqOptions.Password, _rabbitMqOptions.QueueName);
            imageFileQueue.Start();
            imageFileQueue.SetConsumer(Consume);
    
            
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }
        private static void Consume(ILog obj)
        {
            Console.WriteLine(obj.Message);
        }
    }
}