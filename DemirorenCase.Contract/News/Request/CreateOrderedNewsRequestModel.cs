namespace DemirorenCase.Contract.News.Request
{
    public class CreateOrderedNewsRequestModel
    {
        public CreateNewsRequestModel News { get; set; }
        public int Order { get; set; }
    }
}