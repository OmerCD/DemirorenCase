using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.DTO.News;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;
using MediatR;

namespace DemirorenCase.Domain.Commands.News
{
    public record CreateNewsGroupCommand : IRequest<CreateNewsGroupCommandResponse>
    {
        public string GroupName { get; set; }
    }

    public record CreateNewsGroupCommandResponse(string Id);
    
    public class CreateNewsGroupCommandHandler : IRequestHandler<CreateNewsGroupCommand, CreateNewsGroupCommandResponse>
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public CreateNewsGroupCommandHandler(INewsService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }

        public async Task<CreateNewsGroupCommandResponse> Handle(CreateNewsGroupCommand request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<CreateNewsGroupDto>(request);
            var getNewGroupsDto = await _newsService.CreateNewsGroupAsync(dto, cancellationToken);
            return new CreateNewsGroupCommandResponse(getNewGroupsDto.Id);
        }
    }
}