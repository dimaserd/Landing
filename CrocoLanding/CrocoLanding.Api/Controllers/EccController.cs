using Croco.Core.Abstractions.Models;
using Croco.Core.Application;
using Croco.Core.Extensions;
using Croco.Core.Implementations.TransactionHandlers;
using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Api.Models;
using CrocoLanding.Logic;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using CrocoLanding.Model.Entities.Ecc;
using Ecc.Implementation.Models;
using Ecc.Implementation.Workers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EccController : BaseApiController
    {
        readonly bool IsDevelopment = CrocoApp.Application.As<LandingWebApplication>().IsDevelopment;

        public EccController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor) : base(context, signInManager, userManager, httpContextAccessor)
        {
        }

        [HttpPost("SendCallBackRequest")]
        public async Task<BaseApiResponse> SendCallBackRequest([FromForm]CreateCallBackApiModel model)
        {
            try
            {
                var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                return await SendCallBackRequestInnner(model, ip);
            }
            catch (Exception ex)
            {
                return new BaseApiResponse(ex);
            }
        }


        [HttpPost("CallBacks")]
        public Task<List<CallBackRequest>> GetCallBacks(string pass)
        {
            if(pass != ApiConsts.Password)
            {
                return Task.FromResult((List<CallBackRequest>)null);
            }

            return CrocoTransactionHandler.System.ExecuteAndCloseTransaction(amb =>
            {
                return new CallBackRequestWorker(amb, IsDevelopment).GetCallBackRequests();
            });
        }


        private Task<BaseApiResponse> SendCallBackRequestInnner(CreateCallBackApiModel model, string ip)
        {
            var nModel = new CreateCallBackRequest
            {
                EmailOrPhoneNumber = model.EmailOrPhoneNumber,
                Ip = ip
            };

            return CrocoTransactionHandler.System.ExecuteAndCloseTransaction(amb =>
            {
                return new CallBackRequestWorker(amb, IsDevelopment).CreateCallBackRequest(nModel);
            });
        }
    }
}