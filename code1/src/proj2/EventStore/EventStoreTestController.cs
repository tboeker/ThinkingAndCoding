using EventStore.ClientAPI;
using Microsoft.AspNetCore.Mvc;

namespace proj2.EventStore
{
    [ApiController]
    [Route("api/eventstore")]
    [ApiExplorerSettings(GroupName = "proj2")]
    public class EventStoreTestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromServices] IEventStoreConnection connection)
        {
            return new JsonResult(new
            {
                connection.ConnectionName
            });
        }
    }
}