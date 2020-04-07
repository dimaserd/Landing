using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using DocumentFormat.OpenXml.Drawing;
using Ecc.Implementation.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("Api/Redirect"), ApiController]
    public class RedirectController : BaseApiController
    {
        public RedirectController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor) : base(context, signInManager, userManager, httpContextAccessor)
        {
        }

        [HttpGet("To")]
        public async Task<IActionResult> To(string id)
        {
            var service = new RedirectToUrlService(AmbientContext);

            var res = await service.GetUrlToRedirect(id);

            if(res.IsSucceeded)
            {
                return Content($"<script>window.location = '{res.ResponseObject}';</script>", "text/html");
            }

            return Redirect("~/");
        }
    }
}