using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using Ecc.Contract.Models.Messaging;
using Ecc.Logic.Workers.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class MessageDistributionController : BaseApiController
    {
        public MessageDistributionController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor) : base(context, signInManager, userManager, httpContextAccessor)
        {
        }

        MessageDistributionQueryWorker QueryWorker => new MessageDistributionQueryWorker(AmbientContext);

        [HttpPost("List")]
        public Task<List<MessageDistributionCountModel>> GetDistributions()
        {
            return QueryWorker.GetDistributions();
        }
    }
}