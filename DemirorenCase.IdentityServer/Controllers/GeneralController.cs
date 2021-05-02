using DemirorenCase.IdentityServer.Infrastructure.Core.Entities.Authentication;
using DemirorenCase.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemirorenCase.IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneralController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public GeneralController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("adminId")]
        public IActionResult GetAdminId()
        {
            return Ok(new GetAdminInfoResponseModel(_userManager.FindByNameAsync("admin").Id));
        }
    }
}