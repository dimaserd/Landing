using Croco.Core.Application;
using CrocoLanding.Logic.Extensions;
using CrocoShop.Model.Contexts;

namespace CrocoLanding.Api.Controllers.Base
{
    /// <inheritdoc />
    /// <summary>
    /// Базовый абстрактный контроллер в котором собраны общие методы и свойства
    /// </summary>
    public class BaseApiController : CrocoGenericController<LandingDbContext>
    {
        /// <inheritdoc />
        public BaseApiController(LandingDbContext context) : base(context, x => x.Identity.GetUserId())
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