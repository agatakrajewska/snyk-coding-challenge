using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Package.Api.RetrieveDependencies;
using Xunit;

namespace Package.Api.Test.UnitTests
{
    public class NpmjsTests
    {
        public NpmjsService CreateTarget(INpmjsHttpService npmjsHttpService = null, NpmjsConfiguration config = null)
        {
            npmjsHttpService = npmjsHttpService ?? Substitute.For<INpmjsHttpService>();

            config = config ?? new NpmjsConfiguration() { PageSize = 2 };

            var options = Substitute.For<IOptions<NpmjsConfiguration>>();
            options.Value.Returns(config);

            return new NpmjsService(npmjsHttpService, options);
        }

        [Fact]
        public async Task GetDependencies_GivenPageNumber_RetrievesOnlyAppropriateDependencies()
        {
            var npmjsHttpService = Substitute.For<INpmjsHttpService>();
            var response = new PackageDetailsResponse()
            {
                Name = "testPackage",
                Version = "1.1.1",
                Dependencies = new Dictionary<string, string>()
                {
                    {"dependency1","0.1.1"},
                    {"dependency2","0.1.1"},
                    {"dependency3","0.1.1"},
                    {"dependency4","0.1.1"}
                }
            };
            var responseNoDependencies3 = new PackageDetailsResponse()
            {
                Name = "dependency3",
                Version = "1.1.1"
            };
            var responseNoDependencies4 = new PackageDetailsResponse()
            {
                Name = "dependency4",
                Version = "1.1.1"
            };

            npmjsHttpService.GetDependencies("testPackage", "1.1.1")
                .Returns(r => response);

            npmjsHttpService.GetDependencies("dependency3", "0.1.1")
                .Returns(r => responseNoDependencies3);
            npmjsHttpService.GetDependencies("dependency4", "0.1.1")
                .Returns(r => responseNoDependencies4);


            //Act
            var target = CreateTarget(npmjsHttpService);

            var result = await target.GetPackageDependencies("testPackage", "1.1.1", 1);

            //Assert
            result.Dependencies[0].PackageDetails.Name.Should().Be("dependency3");
            result.Dependencies[1].PackageDetails.Name.Should().Be("dependency4");
        }

        //I'd definitely add more test cases, had I had more time :-)
    }
}
