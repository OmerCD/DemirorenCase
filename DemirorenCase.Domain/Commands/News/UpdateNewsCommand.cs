using System;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.DTO.News;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;
using MediatR;

namespace DemirorenCase.Domain.Commands.News
{
    public record UpdateNewsCommand : IRequest<UpdateNewsCommandResponse>
    {
        public string Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }

    public record UpdateNewsCommandResponse;
    
    public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, UpdateNewsCommandResponse>
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public UpdateNewsCommandHandler(INewsService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }

        public async Task<UpdateNewsCommandResponse> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<UpdateNewsDto>(request);
            var response = await _newsService.UpdateNewsAsync(dto, cancellationToken);
            if (!response.IsSuccessful)
            {
                throw new Exception(response.Error);
            }

            return new UpdateNewsCommandResponse();
        }
    }
}