using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsDependencyInjection
{
    public class Warmup
    {
        private readonly IConfiguration _configuration;

        public Warmup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // The function app goes to sleep after 20 minutes of inactivity, trigger with a shorter interval to keep it awake.
        [FunctionName("Warmup")]
        public void Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer, ILogger log) 
        {
            var urls = _configuration["FunctionUrls"].Split(';');

            foreach (string url in urls)
            {
                try
                {
                    // TODO: Auth and make a GET request to each of the function URLs with a query string param ?warmup=true
                    log.LogInformation($"Successfully warmed up a function at {url}");
                }
                catch (Exception e)
                {
                    log.LogError($"Failed to warmup a function at {url}. {e.Message}");
                }
            }
        }
    }
}
