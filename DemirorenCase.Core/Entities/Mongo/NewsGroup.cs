using System.Collections.Generic;
using DemirorenCase.Infrastructure.Abstractions.Core;

namespace DemirorenCase.Core.Entities.Mongo
{
    public class NewsGroup : MongoEntity
    {
        public ICollection<OrderedNews> OrderedNews { get; set; }
        public string Name { get; set; }
    }
}