using System;
using System.Threading.Tasks;
using AzureFunctionsDependencyInjection.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsDependencyInjection
{
    public class CacheRefresh
    {
        private readonly IDataService _dataService;

        public CacheRefresh(IDataService dataService)
        {
            _dataService = dataService;
        }

        //Refreshes the cache once per hour
        [FunctionName("CacheRefresh")]
        public async Task RunAsync([TimerTrigger("0 0 * * * *")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                await _dataService.GetAndCacheDataAsync();
                log.LogInformation($"Successfully refreshed cache.");
            }
            catch (Exception e)
            {
                log.LogError($"Failed to refresh cache: {e.Message}");
            }
        }
    }
}
