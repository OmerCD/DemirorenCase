using System;
using System.Net.Http.Headers;
using DemirorenCase.Infrastructure.Abstractions.Core;
using DemirorenCase.Infrastructure.Abstractions.Services;
using DemirorenCase.Infrastructure.Respositories;
using DemirorenCase.Infrastructure.Services;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace DemirorenCase.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddScoped<IMapper, Mapper>(provider =>
            {
                var config = new TypeAdapterConfig();
                config.Scan(
                    typeof(Startup).Assembly, typeof(IScopedService).Assembly,
                    typeof(Domain.Domain).Assembly, typeof(BaseRepository<>).Assembly);
                return new Mapper(config);
            });
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.Scan(selector => selector
                .FromAssemblyOf<IScopedService>().FromAssemblyOf<AuthenticationService>()
                .AddClasses(classes => classes.AssignableTo<IScopedService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            return services;
        }

        public static IServiceCollection AddMongoClient(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IMongoClient>(provider => new MongoClient(connectionString));
            services.AddScoped(provider => provider.GetService<IMongoClient>().StartSession());
            return services;
        }

        public static IServiceCollection AddIdentityServerHttpClient(this IServiceCollection services,
            string baseAddress)
        {
            services.AddHttpClient("identityServer", client =>
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, string identityAddress)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = identityAddress;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                    };
                    options.RequireHttpsMetadata = false;
                });

            return services;
        }
    }
}