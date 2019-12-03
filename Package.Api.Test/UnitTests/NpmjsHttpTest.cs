using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NSubstitute;
using Package.Api.RetrieveDependencies;
using Xunit;

namespace Package.Api.Test.UnitTests
{
    public class NpmjsHttpTest
    {
        public NpmjsHttpService CreateTarget(NpmjsConfiguration config = null, HttpClient httpClient = null)
        {
            config = config ?? new NpmjsConfiguration() { BaseUrl = "http://base-url" };

            var options = Substitute.For<IOptions<NpmjsConfiguration>>();
            options.Value.Returns(config);

            var messageHandler = new MockHttpMessageHandler();
            httpClient = httpClient ?? new HttpClient(messageHandler);

            return new NpmjsHttpService(httpClient, options);
        }

        [Fact]
        public async Task GetDependencies_GivenRequestWasUnsuccessful_ThrowsNpmjsHttpException()
        {
            var npmjsHttpService = CreateTarget();

            await Assert.ThrowsAsync<NpmjsHttpException>(() => npmjsHttpService.GetDependencies("packageName", "1.1.1"));
        }

        //DAnother possible test case :-)
        [Fact]
        public async Task GetDependencies_GivenUserProvidedSpecialCharanctersInPackageName_TheyHaveBeenEcoded()
        {

        }

        //this mock could of course be configurable, should we need more results
        public class MockHttpMessageHandler : HttpMessageHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("This is an error response from npmjs"),
                    RequestMessage = new HttpRequestMessage()
                    {
                        RequestUri = new Uri("http://base-url")
                    }
                };
            }
        }
    }
}
