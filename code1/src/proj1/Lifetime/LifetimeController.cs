using Microsoft.AspNetCore.Mvc;

namespace proj1.Lifetime
{
    [ApiController]
    [Route("api/lifetime")]
    [ApiExplorerSettings(GroupName = "proj1-lifetime")]
    public class LifetimeController : ControllerBase
    {
        [HttpGet]
        public string Get([FromServices] IMyApplicationLifetime lifetime)
        {
            if (lifetime.IsStarted)
            {
                return "started";
            }

            if (lifetime.IsStarting)
            {
                return "starting";
            }

            if (lifetime.IsStopped)
            {
                return "stopped";
            }

            if (lifetime.IsStopping)
            {
                return "stopping";
            }

            return string.Empty;
        }

        [HttpPost("stop")]
        public void Stop([FromServices] IMyApplicationLifetime lifetime)
        {
            if (!lifetime.IsStopped && !lifetime.IsStopping)
            {
                lifetime.HostApplicationLifetime.StopApplication();
            }
        }
    }
}