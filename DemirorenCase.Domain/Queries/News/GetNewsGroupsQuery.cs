using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;
using MediatR;

namespace DemirorenCase.Domain.Queries.News
{
    public record GetNewsGroupsQuery : IRequest<GetNewsGroupsQueryResponse>
    {
        
    }

    public class GetNewsGroupsQueryResponse
    {
        public IEnumerable<GetNewsGroupsQueryResponseItem> GetNewsGroupsQueryResponseItems { get; set; }
        
    };
    public class GetNewsGroupsQueryResponseItem
    {
        public string GroupName { get; set; }
        public string Id { get; set; }
    }
    public class GetNewsGroupsQueryHandler : IRequestHandler<GetNewsGroupsQuery,GetNewsGroupsQueryResponse>
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public GetNewsGroupsQueryHandler(INewsService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }

        public Task<GetNewsGroupsQueryResponse> Handle(GetNewsGroupsQuery request, CancellationToken cancellationToken)
        {
            var groups = _newsService.GetAllNewsGroups(cancellationToken);
            var getNewsGroupsQueryResponse = _mapper.Map<GetNewsGroupsQueryResponse>(groups);
            return Task.FromResult(getNewsGroupsQueryResponse);
        }
    }
}