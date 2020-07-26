using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace proj1.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "proj1-test1")]
    [Route("api/test1")]
    public class Test1Controller : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(OperationId = "GetTest1")]
        public IActionResult Get()
        {
            return new JsonResult(new
            {
                Hallo = "Test 1"
            });
        }
    }
}