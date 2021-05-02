using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;
using MediatR;

namespace DemirorenCase.Domain.Queries.News
{
    public record GetAllNewsQuery : IRequest<GetAllNewsQueryResponse>;

    public record GetAllNewsQueryResponse
    {
        public IEnumerable<GetAllNewsQueryListItemResponse> News { get; set; }
    }

    public record GetAllNewsQueryListItemResponse
    {
        public string Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
    
    public class GetAllNewsQueryHandler : IRequestHandler<GetAllNewsQuery,GetAllNewsQueryResponse>
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public GetAllNewsQueryHandler(INewsService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }

        public Task<GetAllNewsQueryResponse> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
        {
            var allNews = _newsService.GetAllNews(cancellationToken);
            return Task.FromResult(_mapper.Map<GetAllNewsQueryResponse>(allNews));
        }
    }
}