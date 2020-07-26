using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace proj2.MongoDb
{
    [ApiController]
    [Route("api/mongodb")]
    [ApiExplorerSettings(GroupName = "proj2")]
    public class MongoDbTestController : ControllerBase
    {
        private readonly ILogger<MongoDbTestController> _logger;

        public MongoDbTestController(ILogger<MongoDbTestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IMongoDatabase db)
        {
            
            var cols = await db.ListCollectionsAsync();

            var ret = new List<string>();

            while (await cols.MoveNextAsync())
            {
                var items = cols.Current.ToArray();
                ret.AddRange(items.Select(x => x["name"].AsString));
            }

            return new JsonResult(ret);
        }
    }
}