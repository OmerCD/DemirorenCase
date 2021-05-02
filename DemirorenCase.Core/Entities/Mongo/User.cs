using DemirorenCase.Infrastructure.Abstractions.Core;

namespace DemirorenCase.Core.Entities.Mongo
{
    public class User : MongoEntity
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string LastName { get; set; }
        public object RelationId { get; set; }
    }
}