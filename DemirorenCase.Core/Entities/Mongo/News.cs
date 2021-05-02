using DemirorenCase.Infrastructure.Abstractions.Core;

namespace DemirorenCase.Core.Entities.Mongo
{
    public class News : MongoEntity
    {
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }
}