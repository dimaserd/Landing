using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CrocoLanding.Models;
using Microsoft.AspNetCore.Hosting;

namespace CrocoLanding.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public HomeController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            var webRoot = _env.WebRootPath;
            var path = System.IO.Path.Combine(webRoot, "index.html");

            return File(path, "text/html");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}