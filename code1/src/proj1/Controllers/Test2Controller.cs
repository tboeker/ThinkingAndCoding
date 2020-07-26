using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace proj1.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "proj1-test2")]
    [Route("api/test2")]
    public class Test2Controller: ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(OperationId = "GetTest2")]
        public IActionResult Get()
        {
            return new JsonResult(new
            {
                Hallo = "Test 2"
            });
        }
    }
}