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
            builder.Services.AddMemoryCache(); // Add memory cache to your app
            builder.Services.AddSingleton<IDataService, DataService>(); // Add your custom service
        }
    }
}
