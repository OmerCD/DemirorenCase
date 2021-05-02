using DemirorenCase.IdentityServer.Infrastructure.Core.Entities.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DemirorenCase.IdentityServer.Infrastructure
{
    public class AuthenticationDbContext : IdentityDbContext<User, Role, int>
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
        {
        }
    }
}