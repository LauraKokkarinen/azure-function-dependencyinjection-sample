using AzureFunctionsDependencyInjection.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzureFunctionsDependencyInjection.Startup))]

namespace AzureFunctionsDependencyInjection
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IDataService, DataService>(); // Add custom data fetching service
            builder.Services.AddSingleton<IBlobCache, BlobCache>(); // Add custom blob caching service
        }
    }
}
