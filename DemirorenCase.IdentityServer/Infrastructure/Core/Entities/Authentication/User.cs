using System;
using DemirorenCase.IdentityServer.Infrastructure.Core.Entities.Abstract;
using Microsoft.AspNetCore.Identity;

namespace DemirorenCase.IdentityServer.Infrastructure.Core.Entities.Authentication
{
    public class User : IdentityUser<int>, IEntity
    {
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Female,
        Male
    }

   
}