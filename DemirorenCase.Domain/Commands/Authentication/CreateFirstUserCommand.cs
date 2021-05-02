using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.DTO.Authentication;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;
using MediatR;

namespace DemirorenCase.Domain.Commands.Authentication
{
    public record CreateFirstUserCommand
        (string Name, string LastName, object RelationId) : IRequest<CreateFirstUserCommandResponse>;

    public record CreateFirstUserCommandResponse(string UserId);

    public class CreateFirstUserCommandHandler : IRequestHandler<CreateFirstUserCommand, CreateFirstUserCommandResponse>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public CreateFirstUserCommandHandler(IAuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<CreateFirstUserCommandResponse> Handle(CreateFirstUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = _authenticationService.GetUserByName("admin");
            if (user != null) return new CreateFirstUserCommandResponse(null);
            var newUser = _mapper.Map<CreateUserDto>(request);
            var getUserDto = await _authenticationService.CreateUserAsync(newUser, cancellationToken);
            return new CreateFirstUserCommandResponse(getUserDto.Id);

        }
    }
}