using System;

namespace Package.Api.RetrieveDependencies
{
    public class NpmjsConfiguration
    {
        public string BaseUrl { get; set; }
        public Uri BaseAddress => new Uri(BaseUrl);
        public int PageSize { get; set; }
    }
}