using System;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.DTO.Authentication;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;
using MediatR;

namespace DemirorenCase.Domain.Commands.Authentication
{
    public record UserLoginCommand(string UserName, string Password) : IRequest<UserLoginCommandResponse>;
    public record UserLoginCommandResponse
    {
        public string Token { get; init; }
    }

    public class UserLoginCommandHandler:IRequestHandler<UserLoginCommand, UserLoginCommandResponse>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public UserLoginCommandHandler(IAuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<UserLoginCommandResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = _authenticationService.GetUserByName(request.UserName);
            if (user == null)
            {
                throw new Exception($"User not found: {request.UserName}");
            }

            var loginResultDto = await _authenticationService.LoginUserAsync(_mapper.Map<LoginUserDto>(request), cancellationToken);
            return _mapper.Map<UserLoginCommandResponse>(loginResultDto);
        }
    }
}