using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AzureFunctionsDependencyInjection.Services
{
    public interface IDataService
    {
        Task<IEnumerable<JToken>> GetDataAsync();
        Task<IEnumerable<JToken>> GetAndCacheDataAsync();
    }
}
