using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using Ecc.Implementation.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("Redirect")]
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
                return Redirect(res.ResponseObject);
            }

            return Redirect("~/");
        }

        [HttpGet("ToTest")]
        public IActionResult To(string url, bool main = false)
        {
            if(main)
            {
                return Redirect("~/");
            }

            return Redirect(url);
        }
    }
}