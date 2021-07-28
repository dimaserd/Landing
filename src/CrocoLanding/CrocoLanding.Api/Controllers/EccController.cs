using Croco.Core.Contract.Models;
using CrocoLanding.Logic.Models;
using CrocoLanding.Logic.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("api/Ecc"), ApiController]
    public class EccController : Controller
    {
        CallBackRequestWorker CallBackRequestWorker { get; }
        bool IsDevelopment { get; }

        public EccController(CallBackRequestWorker callBackRequestWorker, IWebHostEnvironment environment)
        {
            CallBackRequestWorker = callBackRequestWorker;
            IsDevelopment = environment.IsDevelopment();
        }

        
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

            return CallBackRequestWorker.CreateCallBackRequest(nModel, IsDevelopment);
        }
    }
}