using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using CrocoLanding.Logic;
using CrocoLanding.Model.Entities.Ecc;
using Ecc.Contract.Models;
using Ecc.Logic.TaskGivers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers
{
    public class CallBackRequestWorker : BaseAppWorker
    {
        public CallBackRequestWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }

        public Task<List<CallBackRequest>> GetCallBackRequests()
        {
            return Query<CallBackRequest>().OrderByDescending(x => x.CreatedOn).ToListAsync();
        }

        public async Task<BaseApiResponse> CreateCallBackRequest(CreateCallBackRequest model)
        {
            var validation = ValidateModel(model);

            if(!validation.IsSucceeded)
            {
                return validation;
            }

            var hourBefore = Application.DateTimeProvider.Now.AddHours(-1);

            if(!Application.IsDevelopment && await Query<CallBackRequest>().AnyAsync(x => x.IpAddress == model.Ip && x.CreatedOn >= hourBefore))
            {
                return new BaseApiResponse(false, "С вашего Ip адреса уже была отправлена заявка в течение часа, если вы устали ждать связи, мы просим прощения");
            }

            CreateHandled(new CallBackRequest
            {
                EmailOrPhoneNumber = model.EmailOrPhoneNumber,
                IpAddress = model.Ip,
            });

            var res = await TrySaveChangesAndReturnResultAsync("Заявка создана");

            if(res.IsSucceeded)
            {
                Application.JobManager.Enqueue<SendEmailTaskGiver>();
            }

            return res;
        }
    }
}