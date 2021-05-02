using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.Core;

namespace DemirorenCase.Infrastructure.Abstractions.Repositories
{
    public interface IRepository<T> : IQueryable<T> where T:IMongoEntity
    {
        Task InsertAsync(T obj, CancellationToken token = default);
        Task UpdateAsync(T obj, CancellationToken token = default);
        Task DeleteAsync(string id, CancellationToken token = default);
        Task<T> GetAsync(string id, CancellationToken token = default);
        IQueryable<T> GetQueryable();
        IEnumerable<T> GetAll(CancellationToken token = default);
        Task SoftDeleteAsync(string id, CancellationToken token = default);
    }
}