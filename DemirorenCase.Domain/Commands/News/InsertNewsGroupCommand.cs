using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.DTO.News;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;
using MediatR;

namespace DemirorenCase.Domain.Commands.News
{
    public class InsertNewsGroupCommand : IRequest<InsertNewsGroupResponse>
    {
        public string NewsId { get; set; }
        public string GroupId { get; set; }
        public int Order { get; set; }
    }
    public record InsertNewsGroupResponse(string Id);
        
    public class InsertNewsGroupCommandHandler : IRequestHandler<InsertNewsGroupCommand,InsertNewsGroupResponse>
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public InsertNewsGroupCommandHandler(INewsService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }
        public async  Task<InsertNewsGroupResponse> Handle(InsertNewsGroupCommand request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<InsertNewsToGroupDto>(request);
            var getInsertDto =await _newsService.InsertNewsToGroupAsync(dto, cancellationToken);
            return new InsertNewsGroupResponse(getInsertDto.Id);

        }
    }
}