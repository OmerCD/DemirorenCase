using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Contract.Authentication.Request;
using DemirorenCase.Contract.Authentication.Response;
using DemirorenCase.Domain.Commands.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DemirorenCase.API.Controllers.v1._0
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel model, CancellationToken token)
        {
            var command = new UserLoginCommand(model.UserName, model.Password);
            var response = await _mediator.Send(command, token);
            return Ok(_mapper.Map<LoginResponseModel>(response));
        }
    }
}