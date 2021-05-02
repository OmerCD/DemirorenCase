using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.DTO.News;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;
using MediatR;

namespace DemirorenCase.Domain.Commands.News
{
    public record CreateNewsCommand : IRequest<CreateNewsCommandResponse>
    {
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }
    public record CreateNewsCommandResponse(string Id, string Link);
    
    public class  CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, CreateNewsCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly INewsService _newsService;

        public CreateNewsCommandHandler(IMapper mapper, INewsService newsService)
        {
            _mapper = mapper;
            _newsService = newsService;
        }

        public async Task<CreateNewsCommandResponse> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<CreateNewsDto>(request);
            var getNewsDto = await _newsService.CreateNewsAsync(dto, cancellationToken);
            return new CreateNewsCommandResponse(getNewsDto.Id, getNewsDto.Link);
        }
    }
}