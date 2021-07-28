using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Croco.Core.Logic.Services;
using CrocoLanding.Logic.Models;
using CrocoLanding.Model.Contexts;
using CrocoLanding.Model.Entities.Ecc;
using Ecc.Contract.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrocoLanding.Logic.Services
{
    public class CallBackRequestWorker : BaseCrocoService<LandingDbContext>
    {
        readonly int MinutesBefore = 10;

        public CallBackRequestWorker(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication app) : base(ambientContext, app)
        {
        }

        public Task<List<CallBackRequest>> GetCallBackRequests()
        {
            return Query<CallBackRequest>().OrderByDescending(x => x.CreatedOn).ToListAsync();
        }

        public async Task<BaseApiResponse> CreateCallBackRequest(CreateCallBackRequest model, bool isDevelopment)
        {
            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var dateBefore = Application.DateTimeProvider.Now.AddMinutes(-MinutesBefore);

            if (!isDevelopment && await Query<CallBackRequest>().AnyAsync(x => x.IpAddress == model.Ip && x.CreatedOn >= dateBefore))
            {
                return new BaseApiResponse(false, $"С вашего Ip адреса уже была отправлена заявка в течение {MinutesBefore} минут, если вы устали ждать связи, мы просим прощения");
            }

            CreateHandled(new CallBackRequest
            {
                EmailOrPhoneNumber = model.EmailOrPhoneNumber,
                IpAddress = model.Ip,
            });

            var res = await TrySaveChangesAndReturnResultAsync("Заявка создана. Мы вам скоро перезвоним");

            if (res.IsSucceeded)
            {
                await PublishMessageAsync(new SendMailMessage
                {
                    Email = "dimaserd96@yandex.ru",
                    Body = $"Перезвонилка на {model.EmailOrPhoneNumber}",
                    Subject = "Заявка на перезвон"
                });
            }

            return res;
        }
    }
}
