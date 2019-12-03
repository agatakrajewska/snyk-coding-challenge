using Xunit;

namespace Package.Api.Test.IntegrationTests
{
    public class NpmjsApiIntegrationTest
    {
        /* I'd have this test to check the integration with npm, in case, let's say, contract changes on their side.
         I would have it to be run as a scheduled task/job, maybe overnight for example, as the integration tests tend to take a bit
         more time and be a bit more flaky :-) */
        [Fact(Skip = "Run as a job")]
        public void Test1()
        {
        }
    }
}
