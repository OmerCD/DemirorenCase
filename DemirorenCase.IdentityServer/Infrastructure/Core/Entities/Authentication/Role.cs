using System;
using DemirorenCase.IdentityServer.Infrastructure.Core.Entities.Abstract;
using Microsoft.AspNetCore.Identity;

namespace DemirorenCase.IdentityServer.Infrastructure.Core.Entities.Authentication
{
    public class Role : IdentityRole<int>, IEntity
    {
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}