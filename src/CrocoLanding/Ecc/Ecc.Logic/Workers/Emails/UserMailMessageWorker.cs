using System.Linq;
using System.Threading.Tasks;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Croco.Core.Logic.Workers;
using Croco.Core.Search.Models;
using Ecc.Logic.Models.Users;
using Ecc.Model.Entities.Interactions;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Logic.Workers.Emails
{
    public class UserMailMessageWorker : BaseCrocoWorker
    {
        public UserMailMessageWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }

        public Task<GetListResult<UserMailMessageModel>> GetMailsAsync(GetListSearchModel model)
        {
            var initQuery = Query<MailMessageInteraction>().OrderByDescending(x => x.CreatedOn);

            return GetListResult<UserMailMessageModel>.GetAsync(model, initQuery, UserMailMessageModel.SelectExpression);
        }

        public async Task<BaseApiResponse> DeterminingDateOfOpening(string id)
        {
            var repo = GetRepository<MailMessageInteraction>();

            var userMailMessage = await repo.Query().FirstOrDefaultAsync(p => p.Id == id);

            if (userMailMessage == null)
            {
                return new BaseApiResponse(false, "Сообщение не найдено по указанному идентификатору");
            }

            if (userMailMessage.ReadOn.HasValue)
            {
                return new BaseApiResponse(false, "Собщение было открыто прежде");
            }

            userMailMessage.ReadOn = Application.DateTimeProvider.Now;

            repo.UpdateHandled(userMailMessage);

            return await TrySaveChangesAndReturnResultAsync("Пользователь открыл сообщение");
        }
    }
}