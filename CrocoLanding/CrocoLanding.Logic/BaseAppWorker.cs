using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Croco.Core.Logic.Workers;
using CrocoLanding.Logic.Extensions;

namespace CrocoLanding.Logic
{
    public class BaseAppWorker : BaseCrocoWorker<LandingWebApplication>
    {
        public BaseAppWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }

        /// <summary>
        /// Валидировать модель и проверить пользователя на то что он админ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected BaseApiResponse ValidateModelAndUserIsAdmin(object model)
        {
            return ValidateModel(model, () => User.IsAdmin() ? new BaseApiResponse(true, "Ok") : new BaseApiResponse(false, Resources.ValidationMessages.YouAreNotAnAdministrator));
        }
    }
}