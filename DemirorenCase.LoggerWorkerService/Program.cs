using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemirorenCase.Core.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemirorenCase.LoggerWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.Configure<RabbitMqOptions>(hostContext.Configuration.GetSection("RabbitMq"));
                });
    }
}