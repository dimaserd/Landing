using Croco.Core.Application;
using CrocoLanding.Logic.Extensions;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using CrocoLanding.Model.Entities.Clt.Default;
using Microsoft.AspNetCore.Http;

namespace CrocoLanding.Api.Controllers.Base
{
    /// <inheritdoc />
    /// <summary>
    /// Базовый абстрактный контроллер в котором собраны общие методы и свойства
    /// </summary>
    public class BaseApiController : CrocoGenericController<LandingDbContext, ApplicationUser>
    {
        /// <inheritdoc />
        public BaseApiController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor) : base(context, signInManager, userManager, x => x.Identity.GetUserId(), httpContextAccessor)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Удаление объекта из памяти
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            //Логгируем контекст запроса
            CrocoApp.Application.RequestContextLogger.LogRequestContext(RequestContext);

            base.Dispose(disposing);
        }
    }
}