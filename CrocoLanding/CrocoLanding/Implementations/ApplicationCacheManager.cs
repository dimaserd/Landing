using System;
using Croco.Core.Abstractions.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace CrocoLanding.Implementations
{
    public class ApplicationCacheManager : ICrocoCacheManager
    {
        private readonly IMemoryCache _cache;

        public ApplicationCacheManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddValue(ICrocoCacheValue cacheValue)
        {
            var offSet = cacheValue.AbsoluteExpiration.HasValue ? new DateTimeOffset(cacheValue.AbsoluteExpiration.Value) : DateTimeOffset.MaxValue;

            _cache.Set(cacheValue.Key, cacheValue.Value, offSet);
        }

        public T GetValue<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}