using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Infrastructure.Abstractions.Core;
using DemirorenCase.Infrastructure.Abstractions.Repositories;
using DemirorenCase.Infrastructure.Abstractions.ValueObjects;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DemirorenCase.Infrastructure.Respositories
{
    public class BaseRepository<T> : IRepository<T> where T : IMongoEntity
    {
        protected readonly string Database;
        protected readonly IMongoClient MongoClient;
        protected readonly IClientSessionHandle ClientSessionHandle;
        protected readonly string CollectionName;

        protected virtual IMongoCollection<T> Collection =>
            MongoClient.GetDatabase(Database).GetCollection<T>(CollectionName);

        public BaseRepository(IMongoClient mongoClient, IClientSessionHandle clientSessionHandle,
            IOptions<MongoInfo> options)
        {
            MongoClient = mongoClient;
            ClientSessionHandle = clientSessionHandle;
            Database = options.Value.Database;
            CollectionName = typeof(T).Name;

            if (!MongoClient.GetDatabase(Database).ListCollectionNames().ToList().Contains(CollectionName))
                MongoClient.GetDatabase(Database).CreateCollection(CollectionName);
        }

        public Task InsertAsync(T obj, CancellationToken token = default)
        {
            obj.CreateDate = DateTime.UtcNow;
            return Collection.InsertOneAsync(ClientSessionHandle, obj, cancellationToken: token);
        }

        public Task UpdateAsync(T obj, CancellationToken token = default)
        {
            if (obj == null) return Task.FromException(new NullReferenceException());
            var filter = Builders<T>.Filter.Eq(x => x.Id, obj.Id);
            obj.UpdateDate = DateTime.UtcNow;
            return Collection.ReplaceOneAsync(ClientSessionHandle, filter, obj, cancellationToken: token);
        }

        public Task DeleteAsync(string id, CancellationToken token = default)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, id);
            return Collection.DeleteOneAsync(ClientSessionHandle, filter, cancellationToken: token);
        }
        public async Task SoftDeleteAsync(string id, CancellationToken token = default)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, id);
            var found = await GetAsync(id, token);
            if (found != null)
            {
                found.UpdateDate = DateTime.UtcNow;
                found.IsDeleted = true;
                await Collection.ReplaceOneAsync(ClientSessionHandle, filter, found, cancellationToken: token);
            }
        }
        public Task<T> GetAsync(string id, CancellationToken token = default)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, id);
            var notDeleted = Builders<T>.Filter.Eq(x => x.IsDeleted, false);
            var combined = Builders<T>.Filter.And(filter, notDeleted);
            var findFluent = Collection.Find(ClientSessionHandle, combined);
            return findFluent.FirstOrDefaultAsync(token);
        }

        public IEnumerable<T> GetAll(CancellationToken token = default)
        {
            var notDeleted = Builders<T>.Filter.Eq(x => x.IsDeleted, false);
            var found = Collection.Find(ClientSessionHandle, notDeleted);
            return found.ToEnumerable(token);
        }

        public IQueryable<T> GetQueryable() => Collection.AsQueryable();
        public IEnumerator<T> GetEnumerator()
        {
            return Collection.AsQueryable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType => Collection.AsQueryable().ElementType;
        public Expression Expression => Collection.AsQueryable().Expression;
        public IQueryProvider Provider => Collection.AsQueryable().Provider;
    }
}