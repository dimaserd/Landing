using Croco.Core.Implementations.AmbientContext;
using Croco.Core.Implementations.TransactionHandlers;
using Croco.Core.Models;
using Ecc.Contract.Models;
using Ecc.Logic.Workers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CrocoLanding.Controllers
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
                return await SendCallBackRequestInnner(model);
            }
            catch(Exception ex)
            {
                return new BaseApiResponse(ex);
            }
        }

        public Task<BaseApiResponse> SendCallBackRequestInnner(CreateCallBackApiModel model)
        {
            var nModel = new CreateCallBackRequest
            {
                EmailOrPhoneNumber = model.EmailOrPhoneNumber,
                Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            return new CrocoTransactionHandler(() => new SystemCrocoAmbientContext()).ExecuteAndCloseTransaction(amb =>
            {
                return new CallBackRequestWorker(amb).CreateCallBackRequest(nModel);
            });
        }
    }
}