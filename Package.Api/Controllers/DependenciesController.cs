using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Package.Api.RetrieveDependencies;

namespace Package.Api.Controllers
{
    [Route("api/dependencies")]
    [ApiController]
    public class DependenciesController : ControllerBase
    {
        private readonly INpmjsService _npmjsService;

        public DependenciesController(INpmjsService npmjsService)
        {
            _npmjsService = npmjsService;
        }

        [HttpGet("{packageName}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<JsonResult> GetDependencies(string packageName, [FromQuery] string version = "latest", [FromQuery] int page = 0)
        {
            var result = await _npmjsService.GetPackageDependencies(packageName, version, page);

            //I am returning json here so we can easily print it out as a tree view on the client side
            return new JsonResult(result);
        }
    }
}
