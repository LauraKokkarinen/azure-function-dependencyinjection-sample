using System;
using System.Security.Claims;
using AzureFunctionsDependencyInjection.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AzureFunctionsDependencyInjection
{
    public class GetData
    {
        private readonly IDataService _dataService;

        public GetData(IDataService dataService)
        {
            _dataService = dataService;
        }

        [FunctionName("GetData")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = null)] HttpRequest req, ClaimsPrincipal currentUser, ILogger log)
        {
            if (req.Query.ContainsKey("warmup"))
            {
                return new ObjectResult(null) { StatusCode = 204 };
            }

            try
            {
                // If you are using Azure AD authentication on your Functions app, you can use the ClaimsPrincipal object to check the currently logged in user's permissions 
                string currentUserPrincipalName = currentUser.Identity.Name;

                // Get data using a custom service (contains caching)
                var data = await _dataService.GetDataAsync();

                return new ObjectResult(JsonConvert.SerializeObject(data)) { StatusCode = 200 };
            }
            catch (Exception e)
            {
                return new ObjectResult(new { message = e.Message }) { StatusCode = 500 };
            }
        }
    }
}
