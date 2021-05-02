using System;
using StackExchange.Redis;

namespace DemirorenCase.Infrastructure.Abstractions.Services
{
    public interface ICacheClient
    {
        T JsonGet<T>(RedisKey key, CommandFlags flags = CommandFlags.None);
        bool JsonSet(RedisKey key, object value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None);
        bool RemoveKey(RedisKey key);
    }
}