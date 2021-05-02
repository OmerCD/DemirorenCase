namespace DemirorenCase.Infrastructure.Abstractions.DTO.Authentication
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public object RelationId { get; set; }
    }
}