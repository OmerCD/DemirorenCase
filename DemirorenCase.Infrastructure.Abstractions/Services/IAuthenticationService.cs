using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.DTO.Authentication;

namespace DemirorenCase.Infrastructure.Abstractions.Services
{
    public interface IAuthenticationService : IScopedService
    {
        GetUserDto GetUserByName(string name);
        Task<GetUserDto> CreateUserAsync(CreateUserDto userDto, CancellationToken token = default);
        Task<LoginResultDto> LoginUserAsync(LoginUserDto loginUserDto, CancellationToken token = default);
    }
}