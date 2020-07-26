using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace home.Controllers
{
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

            var section = _configuration.GetSection("Home");
            ViewBag.SwaggerUiUri = section["SwaggerUiUri"];
            ViewBag.HealthUiUri = section["HealthUiUri"];
            ViewBag.HelloUri = section["HelloUri"];
            ViewBag.Proj1Uri = section["Proj1Uri"];
            ViewBag.Proj2Uri = section["Proj2Uri"];
            ViewBag.ApiPathBase = _configuration.GetApiPathBase();


            return View();
        }
    }
}