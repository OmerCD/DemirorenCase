using System.Linq;
using System.Threading.Tasks;
using DemirorenCase.IdentityServer.Infrastructure.Core.Entities.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DemirorenCase.IdentityServer.Extensions
{
    public static class ServiceExtensions
    {
        public static async Task ApplyMigrationsWithDefaultUsers(this DbContext dbContext, UserManager<User> userManager)
        {
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await dbContext.Database.MigrateAsync();

                var found = await userManager.FindByNameAsync("admin");
                if (found == null)
                {
                    var user = new User
                    {
                        Email = "admin@admin.com",
                        UserName = "admin",
                    };
                    var identityResult = await userManager.CreateAsync(user, "Admin123!");
                    if (identityResult.Succeeded)
                    {
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        } 
    }
}