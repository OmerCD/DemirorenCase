using System;
using System.Threading.Tasks;
using DemirorenCase.IdentityServer.Infrastructure.Core.Entities.Authentication;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;

namespace DemirorenCase.IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<User> _userManager;

        public ResourceOwnerPasswordValidator(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, context.Password))
            {
                context.Result = new GrantValidationResult(user.Id.ToString(),
                    OidcConstants.AuthenticationMethods.Password, DateTime.Now);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
            }
        }
    }
}