using DemirorenCase.Infrastructure.Abstractions.Core;

namespace DemirorenCase.Core.Entities.Mongo
{
    public class OrderedNews : MongoEntity
    {
        public News News { get; set; }
        public int Order { get; set; }
    }
}