using System;

namespace DemirorenCase.Infrastructure.Abstractions.DTO.Authentication
{
    public class GetUserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public object RelationId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}