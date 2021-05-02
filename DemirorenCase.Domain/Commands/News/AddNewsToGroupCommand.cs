using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.DTO.News;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;
using MediatR;

namespace DemirorenCase.Domain.Commands.News
{
    public record AddNewsToGroupCommand : IRequest<AddNewsToGroupCommandResponse>
    {
        public string GroupId { get; set; }
        public string NewsId { get; set; }
        public int Order { get; set; }
    }

    public record AddNewsToGroupCommandResponse
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<AddNewsToGroupOrderedNewsItemCommandResponse> OrderedNews { get; set; }
    };
    public record AddNewsToGroupOrderedNewsItemCommandResponse
    {
        public AddNewsToGroupNewsItemCommandResponse News { get; set; }
        public int Order { get; set; }
    }
    public record AddNewsToGroupNewsItemCommandResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
    
    public class AddNewsToGroupCommandHandler : IRequestHandler<AddNewsToGroupCommand, AddNewsToGroupCommandResponse>
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public AddNewsToGroupCommandHandler(INewsService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }

        public async Task<AddNewsToGroupCommandResponse> Handle(AddNewsToGroupCommand request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<InsertNewsToGroupDto>(request);
            var getNewsGroupDto = await _newsService.InsertNewsToGroupAsync(dto, cancellationToken);
            return _mapper.Map<AddNewsToGroupCommandResponse>(getNewsGroupDto);
        }
    }
}