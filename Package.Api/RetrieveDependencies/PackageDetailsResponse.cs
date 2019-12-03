using System.Collections.Generic;

namespace Package.Api.RetrieveDependencies
{
    public class PackageDetailsResponse
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public Dictionary<string, string> Dependencies { get; set; }
    }
}