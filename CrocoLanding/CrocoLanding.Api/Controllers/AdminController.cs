using Croco.Core.Abstractions.Models;
using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Logic.Workers;
using CrocoLanding.Model.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class AdminController : BaseApiController
    {
        public AdminController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor) : base(context, signInManager, userManager, httpContextAccessor)
        {
        }

        DeleteDataWorker DeleteDataWorker => new DeleteDataWorker(AmbientContext);

        [HttpPost("DeleteLogs")]
        public Task<BaseApiResponse> DeleteLogs()
        {
            return DeleteDataWorker.DeleteLogs();
        }
    }
}