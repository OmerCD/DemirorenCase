using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Core.Entities.Mongo;
using DemirorenCase.Infrastructure.Abstractions.Core;
using DemirorenCase.Infrastructure.Abstractions.Repositories;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;
using DemirorenCase.Infrastructure.Abstractions.DTO.Authentication;
using DemirorenCase.Infrastructure.Abstractions.ValueObjects;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DemirorenCase.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _identityHttpClient;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IdentityServerOptions _identityServerOptions;

        public AuthenticationService(IRepository<User> userRepository, IMapper mapper, IHttpClientFactory httpClientFactory, ILogger<AuthenticationService> logger, IOptions<IdentityServerOptions> options)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _identityHttpClient = httpClientFactory.CreateClient("identityServer");
            _identityServerOptions = options.Value;
        }

        public GetUserDto GetUserByName(string name)
        {
            var normalizedName = name.ToUpperInvariant().Trim();
            var user = _userRepository.FirstOrDefault(x => x.NormalizedName == normalizedName);
            return user == null ? null : _mapper.Map<GetUserDto>(user);
        }

        public async Task<GetUserDto> CreateUserAsync(CreateUserDto userDto, CancellationToken token = default)
        {
            var user = _mapper.Map<User>(userDto);
            user.NormalizedName = user.Name.ToUpperInvariant().Trim();
            await _userRepository.InsertAsync(user, token);
            var found = await _userRepository.GetAsync(user.Id, token);
            return _mapper.Map<GetUserDto>(found);
        }

        public async Task<LoginResultDto> LoginUserAsync(LoginUserDto loginUserDto, CancellationToken token = default)
        {
            var discoveryDocumentResponse = await _identityHttpClient.GetDiscoveryDocumentAsync(cancellationToken: token);
            if (discoveryDocumentResponse.IsError)
            {
                _logger.LogError(discoveryDocumentResponse.Error);
                throw new Exception(discoveryDocumentResponse.Error);
            }

            var tokenResponse = await _identityHttpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                ClientId = _identityServerOptions.ClientId,
                ClientSecret = _identityServerOptions.ClientSecret,
                Scope = _identityServerOptions.Scope,
                UserName = loginUserDto.UserName,
                Password = loginUserDto.Password,
                GrantType = OidcConstants.GrantTypes.Password
            }, cancellationToken: token);
            if (tokenResponse.IsError)
            {
                _logger.LogError(tokenResponse.Error);
                throw new Exception(tokenResponse.Error);
            }

            return JsonConvert.DeserializeObject<LoginResultDto>(tokenResponse.Json.ToString());
        }
    }
}