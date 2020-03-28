using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Extensions;
using Ecc.Model.Entities.LinkCatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class TestController : BaseApiController
    {
        IEccEmailLinkSubstitutor EmailLinkSubstitutor { get; }
        IEccFilePathMapper FilePathMapper { get; }

        public TestController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, 
            IEccEmailLinkSubstitutor emailLinkSubstitutor, IEccFilePathMapper filePathMapper) : base(context, signInManager, userManager, null)
        {
            EmailLinkSubstitutor = emailLinkSubstitutor;
            FilePathMapper = filePathMapper;
        }

        [HttpPost("GetText")]
        public async Task<string> GetText()
        {
            var testModel = EccController.GetTestModel("somemail@mail.com");

            var nModel = testModel.ToSendEmailModel(FilePathMapper);

            var resp = nModel.ResponseObject;

            var t = EmailLinkSubstitutor.ProcessEmailText(resp.Body, Guid.NewGuid().ToString());

            AmbientContext.RepositoryFactory.GetRepository<EmailLinkCatch>().CreateHandled(t.Item2);
            await AmbientContext.RepositoryFactory.SaveChangesAsync();

            return t.Item1;
        }
    }
}