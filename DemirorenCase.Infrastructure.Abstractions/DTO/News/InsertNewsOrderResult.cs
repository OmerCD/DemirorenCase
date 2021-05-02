namespace DemirorenCase.Infrastructure.Abstractions.DTO.News
{
    public class InsertNewsOrderResult
    {
        public string Error { get; set; }
        public bool IsSuccessful { get; set; }
        public GetNewsGroupDto NewsGroupDto { get; set; }
    }
}