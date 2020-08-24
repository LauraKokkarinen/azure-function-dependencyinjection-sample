using System.Threading.Tasks;

namespace AzureFunctionsDependencyInjection.Services
{
    public interface IBlobCache
    {
        Task<string> GetBlobContentAsync(string blobName);
        Task SetBlobContentAsync(string blobName, string content);
    }
}
