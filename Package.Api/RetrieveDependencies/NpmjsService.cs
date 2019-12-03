using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Package.Api.RetrieveDependencies
{
    public class NpmjsService : INpmjsService
    {
        private readonly INpmjsHttpService _npmjsHttpService;
        private readonly NpmjsConfiguration _configuration;

        //Another thing, it would be great to implement mem cache here so we don't have to do a network call every time, 
        //we could have a configurable expiry on it for invalidation
        public NpmjsService(INpmjsHttpService npmjsHttpService, IOptions<NpmjsConfiguration> configuration)
        {
            _npmjsHttpService = npmjsHttpService;
            _configuration = configuration.Value;
        }

        public async Task<PackageDetails> GetPackageDependencies(string packageName, string version, int page)
        {
            return await GetDependencies(packageName, version, page);
        }

        private async Task<PackageDetails> GetDependencies(string packageName, string version, int? page)
        {
            var response = await _npmjsHttpService.GetDependencies(packageName, version);

            var result = new PackageDetails()
            {
                Name = response.Name,
                Version = response.Version,
                PageNumber = page
            };

            if (response.Dependencies == null)
            {
                return result;
            }

            var dependencyList = response.Dependencies.ToList();
            result.Dependencies = new List<Dependency>();

            var resultCount = response.Dependencies.Count;

            var start = 0;
            var limit = resultCount;

            //page bigger than result would have to get handled here, maybe throw an exception
            if (page != null)
            {
                start = _configuration.PageSize * page.Value;
                limit = start + _configuration.PageSize;
            }

            for (var i = start; i < (resultCount < limit ? resultCount : limit); i++)
            {
                var match = Regex.Match(dependencyList[i].Value, "[0-9]\\.[0-9]\\.[0-9]");
                var packageDetails = await GetDependencies(dependencyList[i].Key, match.Value, null);

                result.Dependencies.Add(new Dependency()
                {
                    PackageDetails = packageDetails
                });
            }

            return result;
        }
    }
}