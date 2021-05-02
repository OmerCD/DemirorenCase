using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.DTO.News;

namespace DemirorenCase.Infrastructure.Abstractions.Services
{
    public interface INewsService : IScopedService
    {
        Task<GetNewsDto> CreateNewsAsync(CreateNewsDto createNewsDto, CancellationToken token = default);

         Task<GetNewsGroupDto> CreateNewsGroupAsync(CreateNewsGroupDto createNewsGroupDto,
            CancellationToken token = default);

         Task<GetNewsGroupDto> InsertNewsToGroupAsync(InsertNewsToGroupDto insertNewsToGroupDto,
             CancellationToken token = default);

         Task<GetNewsGroupDto> GetNewsGroupAsync(string id, CancellationToken token = default);
         IEnumerable<GetNewsGroupListItemDto> GetAllNewsGroups(CancellationToken token = default);
         IEnumerable<GetNewsDto> GetAllNews(CancellationToken token = default);
    }
}