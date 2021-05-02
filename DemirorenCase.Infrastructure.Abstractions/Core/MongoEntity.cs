using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DemirorenCase.Infrastructure.Abstractions.Core
{
    public abstract class MongoEntity : IMongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}