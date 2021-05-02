namespace DemirorenCase.Infrastructure.Abstractions.DTO.News
{
    public class InsertNewsToGroupDto
    {
        public string NewsId { get; set; }
        public string GroupId { get; set; }
        public int Order { get; set; }
    }
}