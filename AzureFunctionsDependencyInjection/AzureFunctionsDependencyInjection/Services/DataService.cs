using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace AzureFunctionsDependencyInjection.Services
{
    public class DataService : IDataService
    {
        private readonly IMemoryCache _cache;
        private readonly int _cacheDuration;

        private const string CacheKeyData = "CACHE_KEY_DATA";

        public DataService(IMemoryCache memoryCache, IConfiguration configuration)
        {
            // If you want, you can specify and get the cache duration from app settings
            int.TryParse(configuration["CacheDurationMinutes"], out int cacheDuration);

            _cache = memoryCache;
            _cacheDuration = cacheDuration;
        }

        public IEnumerable<JToken> GetData()
        {
            return GetCachedData() ?? GetAndCacheData();
        }

        public IEnumerable<JToken> GetCachedData()
        {
            _cache.TryGetValue(CacheKeyData, out IEnumerable<JToken> data);

            return data;
        }

        public IEnumerable<JToken> GetAndCacheData()
        {
            var data = new List<JToken>(); // Get your data from the actual data source here

            CacheData(data);

            return data;
        }

        public void CacheData(IEnumerable<JToken> data)
        {
            if (_cacheDuration == 0)
            {
                _cache.Remove(CacheKeyData);
            }
            else
            {
                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheDuration)
                };

                _cache.Set(CacheKeyData, data, options);
            }
        }
    }
}
