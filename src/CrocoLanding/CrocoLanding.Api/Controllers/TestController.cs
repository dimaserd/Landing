using Croco.Core.Abstractions;
using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using Ecc.Contract.Models;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Core.Workers;
using Ecc.Logic.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class TestController : BaseApiController
    {
        EmailDelayedSender DelayedSender => DelayedSenderProvider(AmbientContext);
        Func<ICrocoAmbientContext, EmailDelayedSender> DelayedSenderProvider { get; }
        IEccFilePathMapper FilePathMapper { get; }

        public TestController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager,

            Func<ICrocoAmbientContext, EmailDelayedSender> delayedSenderProvider, IEccFilePathMapper filePathMapper) : base(context, signInManager, userManager, null)
        {
            DelayedSenderProvider = delayedSenderProvider;
            FilePathMapper = filePathMapper;
        }

        [HttpPost("GetText")]
        public async Task<string> GetText()
        {
            var email = "somemail@mail.com";

            var testModel = EccController.GetTestModel(email)
                .ToSendEmailModel(FilePathMapper);

            if(!testModel.IsSucceeded)
            {
                return null;
            }

            var resp = testModel.ResponseObject;

            var t = DelayedSender.ToMailMessage(new SendMailMessage 
            {
                Email = email,
                Body = resp.Body,
                Subject = resp.Subject
            });

            await AmbientContext.RepositoryFactory.SaveChangesAsync();

            return t.Item1.MessageText;
        }
    }
}