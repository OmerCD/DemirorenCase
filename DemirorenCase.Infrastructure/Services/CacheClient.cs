using System;
using DemirorenCase.Infrastructure.Abstractions.Services;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DemirorenCase.Infrastructure.Services
{
    public class CacheClient : ICacheClient
    {
         private readonly ConfigurationOptions configuration = null;
        private Lazy<IConnectionMultiplexer> _Connection = null;

        public CacheClient(string host = "localhost", int port = 6379, bool allowAdmin = false)
        {
            configuration = new ConfigurationOptions()
            {
                //for the redis pool so you can extent later if needed
                EndPoints = { { host, port }, },
                AllowAdmin = allowAdmin,
                //Password = "", //to the security for the production
                ClientName = "My Redis Client",
                ReconnectRetryPolicy = new LinearRetry(5000),
                AbortOnConnectFail = false,
            };
            _Connection = new Lazy<IConnectionMultiplexer>(() =>
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configuration);
                //redis.ErrorMessage += _Connection_ErrorMessage;
                //redis.InternalError += _Connection_InternalError;
                //redis.ConnectionFailed += _Connection_ConnectionFailed;
                //redis.ConnectionRestored += _Connection_ConnectionRestored;
                return redis;
            });
        }

        //for the 'GetSubscriber()' and another Databases
        public IConnectionMultiplexer Connection => _Connection.Value;

        //for the default database
        public IDatabase Database => Connection.GetDatabase();

        public T JsonGet<T>(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            RedisValue rv = Database.StringGet(key, flags);
            if (!rv.HasValue)
                return default;
            var rgv = JsonConvert.DeserializeObject<T>(rv);
            return rgv;
        }

        public bool RemoveKey(RedisKey key)
        {
            return Database.KeyDelete(key);
        }
        public bool JsonSet(RedisKey key, object value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            if (value == null) return false;
            return Database.StringSet(key, JsonConvert.SerializeObject(value), expiry, when, flags);
        }
    }
}