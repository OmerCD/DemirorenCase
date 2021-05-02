using System;

namespace DemirorenCase.IdentityServer.Infrastructure.Core.Entities.Abstract
{
    public interface IEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}