using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemirorenCase.API.Extensions;
using DemirorenCase.API.Middleware;
using DemirorenCase.Common.IoC;
using DemirorenCase.Common.Services;
using DemirorenCase.Core.Abstractions;
using DemirorenCase.Core.ValueObjects;
using DemirorenCase.Domain.Commands.Authentication;
using DemirorenCase.Domain.Queries.Authentication;
using DemirorenCase.Infrastructure.Abstractions.Core;
using DemirorenCase.Infrastructure.Abstractions.Services;
using DemirorenCase.Infrastructure.Abstractions.ValueObjects;
using DemirorenCase.Infrastructure.Extensions;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace DemirorenCase.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddConsulClient(Configuration["Consul:Host"]);
            services.AddMediatR(typeof(Domain.Domain));
            services.AddMapper();
            services.AddServices();
            services.Configure<MongoInfo>(Configuration.GetSection("Mongo"));
            services.AddBaseRepositories();
            services.AddMongoClient(Configuration.GetConnectionString("MongoDb"));
            services.AddIdentityServerHttpClient(Configuration["IdentityServer:BaseAddress"]);
            services.Configure<IdentityServerOptions>(Configuration.GetSection("IdentityServer"));
            services.AddAuthentication(identityAddress: Configuration["IdentityServer:BaseAddress"]);
            services.Configure<RabbitMqOptions>(Configuration.GetSection("RabbitMq"));
            services.AddSingleton<IRabbitQueue<ILog>>(provider =>
            {
                var options = provider.GetService<IOptions<RabbitMqOptions>>().Value;
                return new RabbitQueue<ILog>(options.Address, options.UserName, options.Password, options.QueueName);
            });
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "DemirorenCase.API", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMediator mediator)
        {
            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemirorenCase.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            mediator.Send(new CreateFirstUserCommand("admin", "admin", mediator.Send(new GetAdminIdQuery()).Result.Id))
                .Wait();
        }
    }
}