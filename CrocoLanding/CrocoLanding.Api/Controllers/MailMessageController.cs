﻿using Croco.Core.Search.Models;
using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using Ecc.Logic.Models.Messaging;
using Ecc.Logic.Workers.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class MailMessageController : BaseApiController
    {
        public MailMessageController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor) : base(context, signInManager, userManager, httpContextAccessor)
        {
        }

        MailDistributionQueryWorker MailDistributionQueryWorker => new MailDistributionQueryWorker(AmbientContext);

        [HttpPost("GetSingle")]
        public Task<MailMessageDetailedModel> GetMailMessageDetailed(string id)
        {
            return MailDistributionQueryWorker.GetMailMessageDetailed(id);
        }

        [HttpPost("GetList")]
        public Task<GetListResult<MailMessageModel>> GetClientMailMessages(GetClientInteractions model)
        {
            return MailDistributionQueryWorker.GetClientMailMessages(model);
        }
    }
}