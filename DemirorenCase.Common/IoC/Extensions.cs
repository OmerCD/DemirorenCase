using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Winton.Extensions.Configuration.Consul;

namespace DemirorenCase.Common.IoC
{
    public static class Extensions
    {
        public static IHostBuilder UseConsul(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration((context, builder) =>
            {
                var applicationName = context.HostingEnvironment.ApplicationName.Replace(".",string.Empty);
                var environmentName = context.HostingEnvironment.EnvironmentName;
                builder.AddJsonFile($"appsettings.{environmentName}.json", false, true);
                // builder.AddJsonFile("appsettings.Defaults.json");
                var configurationRoot = builder.Build();
                var host = configurationRoot["Consul:Host"];
                var consulClient = new ConsulClient(configuration => { configuration.Address = new Uri(host); });
                var environmentKVAddress = $"{applicationName}/{environmentName}";
                var task = consulClient.KV.Get(environmentKVAddress).Result;
                if (task.Response == null)
                {
                    var result = File.ReadAllBytes($"appsettings.Defaults.{environmentName}.json");
                    consulClient.KV.Put(new KVPair($"{environmentKVAddress}")
                    {
                        Value = result
                    });
                }

                builder.AddConsul(environmentKVAddress, source =>
                {
                    source.ReloadOnChange = true;
                    source.ConsulConfigurationOptions = configuration => { configuration.Address = new Uri(host); };
                });
            });
            return hostBuilder;
        }

        public static IServiceCollection AddConsulClient(this IServiceCollection services, string hostAddress)
        {
            services.AddSingleton<IConsulClient>(new ConsulClient(clientConfiguration =>
            {
                clientConfiguration.Address = new Uri(hostAddress);
            }));
            return services;
        }
    }
}