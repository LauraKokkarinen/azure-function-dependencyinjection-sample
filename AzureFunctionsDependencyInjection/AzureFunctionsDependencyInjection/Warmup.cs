using System;
using AzureFunctionsDependencyInjection.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsDependencyInjection
{
    public class Warmup
    {
        private readonly IDataService _dataService;

        public Warmup(IDataService dataService)
        {
            _dataService = dataService;
        }

        // The function app goes to sleep after 20 minutes of inactivity, trigger with a shorter interval to keep it awake.
        [FunctionName("Warmup")]
        public void Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer, ILogger log) 
        {
            // Triggering one function in the Functions app is enough to keep all of them warm.
            log.LogInformation($"Successfully warmed up functions.");

            // If you also want to refresh the cache every 15 minutes, you can do that here (example below)
            try
            {
                _dataService.GetAndCacheData();

                // Alternatively, you can create an another function that does the caching and trigger it with a HTTP request (store the URL in the app settings)
            }
            catch (Exception e)
            {
                log.LogError($"Failed to refresh cache: {e.Message}");
            }
        }
    }
}
