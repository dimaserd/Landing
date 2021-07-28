using Croco.Core.Contract.Models;
using CrocoLanding.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class EccController : Controller
    {
        [HttpPost("SendCallBackRequest")]
        public async Task<BaseApiResponse> SendCallBackRequest([FromForm] CreateCallBackApiModel model)
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