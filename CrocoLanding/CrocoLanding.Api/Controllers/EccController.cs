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
using Ecc.Contract.Models;
using Ecc.Implementation.Models;
using Ecc.Implementation.Workers;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class EccController : BaseApiController
    {
        readonly bool IsDevelopment = CrocoApp.Application.As<LandingWebApplication>().IsDevelopment;

        
        public EccController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, 
            IHttpContextAccessor httpContextAccessor,
            IEccPixelUrlProvider pixelUrlProvider, IEccFilePathMapper filePathMapper) : base(context, signInManager, userManager, httpContextAccessor)
        {
            PixelUrlProvider = pixelUrlProvider;
            FilePathMapper = filePathMapper;
        }

        EmailSender EmailSender => new EmailSender(AmbientContext, PixelUrlProvider, FilePathMapper);

        IEccPixelUrlProvider PixelUrlProvider { get; }
        IEccFilePathMapper FilePathMapper { get; }
        

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


        /// <summary>
        /// Отправить email через шаблон
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SendEmailViaTemplate")]
        public Task<BaseApiResponse> SendEmailViaTemplate([FromBody]SendMailMessageViaHtmlTemplate model) => EmailSender.SendEmailViaTemplate(model);

        public static SendMailMessageViaHtmlTemplate GetTestModel(string email)
        {
            var app = CrocoApp.Application.As<LandingWebApplication>();

            var dirName = "MailTemplates";

            return new SendMailMessageViaHtmlTemplate
            {
                Email = email,
                AttachmentFileIds = null,
                Replacings = new List<Replacing>
                {
                    new Replacing
                    {
                        Key = "src=\"images/check.png\"",
                        Value = $"src=\"{app.ApplicationUrl}/{dirName}/images/check.png\""
                    }
                },
                Subject = "Предложение о сотрудничестве от CrocoSoft",
                TemplateFilePath = $"/{dirName}/MyTemplate.html"
            };
        }

        /// <summary>
        /// Отправить email через шаблон
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("SendEmailViaTemplate/ToAddress")]
        public Task<BaseApiResponse> SendEmailViaTemplateToAddress(string email)
        {
            return EmailSender.SendEmailViaTemplate(GetTestModel(email));
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