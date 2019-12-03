using System.Threading.Tasks;

namespace Package.Api.RetrieveDependencies
{
    public interface INpmjsHttpService
    {
        Task<PackageDetailsResponse> GetDependencies(string packageName, string version);
    }
}