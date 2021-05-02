namespace DemirorenCase.Infrastructure.Abstractions.DTO.News
{
    public class GetOrderedNewsDto
    {
        public GetNewsDto News { get; set; }
        public int Order { get; set; }
    }
}