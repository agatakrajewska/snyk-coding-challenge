using System.Threading.Tasks;

namespace Package.Api.RetrieveDependencies
{
    public interface INpmjsService
    {
        Task<PackageDetails> GetPackageDependencies(string packageName, string version, int page);
    }
}