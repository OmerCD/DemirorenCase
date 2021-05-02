using System.Linq;
using DemirorenCase.Core.Entities.Mongo;
using DemirorenCase.Core.ValueObjects;
using DemirorenCase.Infrastructure.Abstractions.Core;
using DemirorenCase.Infrastructure.Abstractions.Repositories;
using DemirorenCase.Infrastructure.Abstractions.Services;
using DemirorenCase.Infrastructure.Respositories;
using DemirorenCase.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DemirorenCase.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBaseRepositories(this IServiceCollection services)
        {
            var entityBaseType = typeof(IMongoEntity);
            var entityTypes = typeof(User).Assembly.GetTypes()
                .Where(x => entityBaseType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
            foreach (var entityType in entityTypes)
            {
                var genericInterfaceType = typeof(IRepository<>).MakeGenericType(entityType);
                var baseRepoType = typeof(BaseRepository<>).MakeGenericType(entityType);
                services.AddScoped(genericInterfaceType, baseRepoType);
            }

            return services;
        }

        public static IServiceCollection AddRedisClient(this IServiceCollection services)
        {
            services.AddSingleton<ICacheClient>(provider =>
            {
                var options = provider.GetService<IOptions<RedisOptions>>().Value;
                return new CacheClient(options.Address, options.Port, true);
            });
            return services;
        }
    }
}