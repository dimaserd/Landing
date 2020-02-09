using Croco.Core.Abstractions.Models;
using Croco.Core.Implementations.TransactionHandlers;
using CrocoLanding.Api.Models;
using CrocoLanding.Model.Entities.Ecc;
using Ecc.Contract.Models;
using Ecc.Logic.Workers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EccController : ControllerBase
    {

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
            if(pass != "MyPassword")
            {
                return Task.FromResult((List<CallBackRequest>)null);
            }

            return CrocoTransactionHandler.System.ExecuteAndCloseTransaction(amb =>
            {
                return new CallBackRequestWorker(amb).GetCallBackRequests();
            });
        }


        private static Task<BaseApiResponse> SendCallBackRequestInnner(CreateCallBackApiModel model, string ip)
        {
            var nModel = new CreateCallBackRequest
            {
                EmailOrPhoneNumber = model.EmailOrPhoneNumber,
                Ip = ip
            };

            return CrocoTransactionHandler.System.ExecuteAndCloseTransaction(amb =>
            {
                return new CallBackRequestWorker(amb).CreateCallBackRequest(nModel);
            });
        }
    }
}