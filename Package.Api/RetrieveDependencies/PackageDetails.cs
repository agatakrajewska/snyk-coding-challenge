using System.Collections.Generic;

namespace Package.Api.RetrieveDependencies
{
    public class PackageDetails
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public IList<Dependency> Dependencies { get; set; }
        public int? PageNumber { get; set; }
    }
}