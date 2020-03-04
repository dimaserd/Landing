using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CrocoLanding.Models;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Ecc.Logic.Workers.Emails;
using Croco.Core.Abstractions.Models.Log;
using Croco.Core.Application;
using CrocoLanding.Logic;
using CrocoLanding.Controllers.Base;
using CrocoLanding.Model.Contexts;
using CrocoLanding.Logic.Services;
using Microsoft.AspNetCore.Http;

namespace CrocoLanding.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IWebHostEnvironment _env;

        public HomeController(LandingDbContext context, ApplicationUserManager userManager, ApplicationSignInManager signInManager, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env) : base(context, userManager, signInManager, httpContextAccessor)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            var path = System.IO.Path.Combine(_env.WebRootPath, "index.html");

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

        /// <summary>
        /// Адрес для выдачи пикселя для писем чтобы уловить их прочитанность
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Img/{id}.jpg", Name = "GetLAPI")]
        public async Task<IActionResult> Img(string id)
        {
            var resp = await new UserMailMessageWorker(AmbientContext)
                .DeterminingDateOfOpening(id);

            AmbientContext.Logger.LogInfo("Ecc.Email.Opened", "Открыто сообщение", new LogNode("Response", resp));

            var fileName = $"Filename.docx";

            var app = CrocoApp.Application;

            var filePath = app.MapPath($"~/wwwroot/Docs/{fileName}");

            var mime = LandingWebApplication.GetMimeMapping(fileName);

            return PhysicalFile(filePath, mime, fileName);
        }
    }
}