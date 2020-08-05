using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AzureFunctionsDependencyInjection.Services
{
    public interface IDataService
    {
        IEnumerable<JToken> GetData();

        IEnumerable<JToken> GetCachedData();

        IEnumerable<JToken> GetAndCacheData();

        void CacheData(IEnumerable<JToken> data);
    }
}
