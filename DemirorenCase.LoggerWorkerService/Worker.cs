using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Core.Abstractions;
using DemirorenCase.Core.ValueObjects;
using DemirorenCase.LoggerWorkerService.ValueObjects;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;

namespace DemirorenCase.LoggerWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RabbitMqOptions _rabbitMqOptions;
        private readonly IElasticClient _elasticClient;

        public Worker(ILogger<Worker> logger, IOptions<RabbitMqOptions> rabbitOptions, IElasticClient elasticClient)
        {
            _logger = logger;
            _elasticClient = elasticClient;
            _rabbitMqOptions = rabbitOptions.Value;
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
        private void Consume(ILog obj)
        {
            _elasticClient.Index(obj, descriptor => descriptor.Index("log"));
        }
        private class Log : ILog
        {
            public string Message { get; set; }
            public DateTime MessageDate { get; set; }
        }
    }
} 