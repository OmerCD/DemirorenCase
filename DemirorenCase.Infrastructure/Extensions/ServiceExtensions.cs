using System.Linq;
using DemirorenCase.Core.Entities.Mongo;
using DemirorenCase.Infrastructure.Abstractions.Core;
using DemirorenCase.Infrastructure.Abstractions.Repositories;
using DemirorenCase.Infrastructure.Respositories;
using Microsoft.Extensions.DependencyInjection;

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
    }
}