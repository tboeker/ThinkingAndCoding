using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace proj1.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public HomeController(IHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Home";
            ViewBag.EnvironmentName = _env.EnvironmentName;
            ViewBag.ApplicationName = _env.ApplicationName;
            ViewBag.Version = Assembly.GetEntryAssembly()
                ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
            ViewBag.ApiPathBase = _configuration.GetApiPathBase();
            
            if (HttpContext.Items.ContainsKey("UpSince"))
            {
                ViewBag.UpSince = (DateTime) HttpContext.Items["UpSince"];
            }
            return View();
        }
    }
}