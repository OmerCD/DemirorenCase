using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;
using MediatR;

namespace DemirorenCase.Domain.Queries.News
{
    public record GetNewsGroupQuery :IRequest<GetNewsGroupQueryResponse>
    {
        public string GroupId { get; set; }
    }

    public record GetNewsGroupQueryResponse
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<GetNewsGroupOrderedNewsItemCommandResponse> OrderedNews { get; set; }
    };
    public record GetNewsGroupOrderedNewsItemCommandResponse
    {
        public GetNewsGroupNewsItemCommandResponse News { get; set; }
        public int Order { get; set; }
    }
    public record GetNewsGroupNewsItemCommandResponse
    {
        public string Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
    
    public class GetNewsGroupQueryHandler:IRequestHandler<GetNewsGroupQuery,GetNewsGroupQueryResponse>
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public GetNewsGroupQueryHandler(INewsService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }

        public async Task<GetNewsGroupQueryResponse> Handle(GetNewsGroupQuery request, CancellationToken cancellationToken)
        {
            var newsGroupDto = await _newsService.GetNewsGroupAsync(request.GroupId, cancellationToken);
            var mapped = _mapper.Map<GetNewsGroupQueryResponse>(newsGroupDto);
            return mapped;
        }
    }
}