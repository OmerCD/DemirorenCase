using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemirorenCase.Common.IoC;
using DemirorenCase.Core.ValueObjects;
using DemirorenCase.LoggerWorkerService.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Nest;

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
                .UseConsul()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.Configure<RabbitMqOptions>(hostContext.Configuration.GetSection("RabbitMq"));
                    services.Configure<ElasticOptions>(hostContext.Configuration.GetSection("Elastic"));
                    services.AddSingleton<IElasticClient>(provider =>
                    {
                        var elasticOptions = provider.GetService<IOptions<ElasticOptions>>();
                        var node = new Uri(elasticOptions.Value.Address);
                        var settings = new ConnectionSettings(node);
                        var client = new ElasticClient(settings);
                        return client;
                    });
                    services.AddConsulClient(hostContext.Configuration["Consul:Host"]);
                });
    }
}