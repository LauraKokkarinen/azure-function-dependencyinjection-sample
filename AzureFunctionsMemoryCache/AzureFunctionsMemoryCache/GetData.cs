using System;
using System.Security.Claims;
using AzureFunctionsMemoryCache.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctionsMemoryCache
{
    public class GetData
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public GetData(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        [FunctionName("GetData")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = null)] HttpRequest req, ClaimsPrincipal currentUser, ILogger log)
        {
            try
            {
                // If you are using Azure AD authentication on your Functions app, you can use the ClaimsPrincipal object to check the currently logged in user's permissions 
                string currentUserPrincipalName = currentUser.Identity.Name;

                // Get data using a custom service (contains caching). You could also use dependency injection for this.
                var dataService = new DataService(_memoryCache, _configuration);
                var data = dataService.GetData();

                return new ObjectResult(JsonConvert.SerializeObject(data)) { StatusCode = 200 };
            }
            catch (Exception e)
            {
                return new ObjectResult(new { message = e.Message }) { StatusCode = 500 };
            }
        }
    }
}
