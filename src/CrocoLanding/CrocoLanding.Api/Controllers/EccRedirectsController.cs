using Croco.Core.Search.Models;
using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using Ecc.Contract.Models.EmailRedirects;
using Ecc.Logic.Workers.EmailRedirects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class EccRedirectsController : BaseApiController
    {
        EmailRedirectsQueryWorker Worker => new EmailRedirectsQueryWorker(AmbientContext);

        public EccRedirectsController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor) : base(context, signInManager, userManager, httpContextAccessor)
        {
        }

        [HttpPost("Query")]
        public Task<GetListResult<EmailLinkCatchRedirectsCountModel>> Query([FromBody]GetListSearchModel model) => Worker.Query(model);

        [HttpPost("GetById")]
        public Task<EmailLinkCatchDetailedModel> GetById(string id) => Worker.GetById(id);
    }
}