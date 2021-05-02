using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Domain.Exceptions;
using DemirorenCase.Infrastructure.Abstractions.DTO.News;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;
using MediatR;

namespace DemirorenCase.Domain.Commands.News
{
    public record ChangeNewsOrderCommand : IRequest<ChangeNewsOrderCommandResponse>
    {
        public string GroupId { get; set; }
        public string NewsId { get; set; }
        public int Order { get; set; }
    }

    public record ChangeNewsOrderCommandResponse;
    
    public class ChangeNewsOrderCommandHandler : IRequestHandler<ChangeNewsOrderCommand, ChangeNewsOrderCommandResponse>
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public ChangeNewsOrderCommandHandler(INewsService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }

        public async Task<ChangeNewsOrderCommandResponse> Handle(ChangeNewsOrderCommand request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<ChangeNewsOrderDto>(request);
            var result = await _newsService.ChangeNewsOrderAsync(dto, cancellationToken);
            if (!result.IsSuccessful)
            {
                throw new NewsOrderException(result.Error);
            }

            return new ChangeNewsOrderCommandResponse();
        }
    }
}