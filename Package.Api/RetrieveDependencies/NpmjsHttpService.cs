using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Package.Api.RetrieveDependencies
{
    public class NpmjsHttpService : INpmjsHttpService
    {
        private readonly HttpClient _httpClient;

        public NpmjsHttpService(HttpClient httpClient, IOptions<NpmjsConfiguration> configuration)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = configuration.Value.BaseAddress;
        }

        public async Task<PackageDetailsResponse> GetDependencies(string packageName, string version)
        {

            var encodedPackageName = WebUtility.HtmlEncode(packageName);
            var encodedVersion = WebUtility.HtmlEncode(version);

            var uri = new Uri($"/{encodedPackageName}/{encodedVersion}", UriKind.Relative);

            var responseMessage = await _httpClient.GetAsync(uri);

            return await DeserializeResponse(responseMessage);
        }

        private static async Task<PackageDetailsResponse> DeserializeResponse(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new NpmjsHttpException(responseMessage);
            }

            return await responseMessage.Content.ReadAsAsync<PackageDetailsResponse>();
        }
    }
}