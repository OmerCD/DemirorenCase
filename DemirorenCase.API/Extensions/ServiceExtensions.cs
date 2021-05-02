using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
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
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Polly;
using Polly.Extensions.Http;

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
            }).AddPolicyHandler(GetRetryPolicy());
            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                    retryAttempt)));
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

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "DemirorenCase.API", Version = "v1"});
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
            return services;
        }
    }
}