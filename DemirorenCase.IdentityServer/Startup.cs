// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using DemirorenCase.Common.IoC;
using DemirorenCase.IdentityServer.Extensions;
using DemirorenCase.IdentityServer.Infrastructure;
using DemirorenCase.IdentityServer.Infrastructure.Core.Entities.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemirorenCase.IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            //services.AddControllersWithViews();
            services.AddControllers();

            var builder = services.AddIdentityServer(options =>
            {
                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
            services.AddDbContext<AuthenticationDbContext>(optionsBuilder =>
                    optionsBuilder.UseNpgsql(Configuration.GetConnectionString("PostGre")))
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuthenticationDbContext>();
            builder.Services.AddConsulClient(Configuration["Consul:Host"]);
            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();
        }

        public void Configure(IApplicationBuilder app, AuthenticationDbContext dbContext, UserManager<User> userManager)
        {
            if (!Environment.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to add MVC
            //app.UseStaticFiles();
            app.UseRouting();
            
            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            //app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            dbContext.ApplyMigrationsWithDefaultUsers(userManager).Wait();
        }
    }
}
