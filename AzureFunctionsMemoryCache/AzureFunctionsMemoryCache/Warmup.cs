using System;
using AzureFunctionsMemoryCache.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsMemoryCache
{
    public class Warmup
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public Warmup(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        [FunctionName("Warmup")]
        public void Run([TimerTrigger("0 */15 * * * *")]TimerInfo myTimer, ILogger log) // The function app goes to sleep after 20 minutes of inactivity, trigger with a shorter interval to keep it awake.
        {
            log.LogInformation($"Successfully warmed up functions."); // Triggering one function in the Functions app is enough to keep all of them warm.

            // If you also want to refresh the cache every 15 minutes, you can do that here (example below)
            try
            {
                var dataService = new DataService(_memoryCache, _configuration);
                dataService.GetAndCacheData();

                // Alternatively, you can create an another function that does the caching and trigger it with a HTTP request (store the URL in the app settings)
            }
            catch (Exception e)
            {
                log.LogError($"Failed to refresh cache: {e.Message}");
            }
        }
    }
}
