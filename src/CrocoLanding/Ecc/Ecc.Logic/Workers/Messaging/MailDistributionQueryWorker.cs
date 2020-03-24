using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models.Search;
using Croco.Core.Search.Extensions;
using Ecc.Logic.Models.Messaging;
using Ecc.Model.Entities.Interactions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Messaging
{
    public class MailDistributionQueryWorker : ApplicationInteractionWorker
    {
        public MailDistributionQueryWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }

        public Task<MailMessageDetailedModel> GetMailMessageDetailed(string id)
        {
            return Query<MailMessageInteraction>().Select(MailMessageDetailedModel.SelectExpression).FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<GetListResult<MailMessageModel>> GetClientMailMessages(GetClientInteractions model)
        {
            var queryWithStatus = GetQueryWithStatus(Query<MailMessageInteraction>().BuildQuery(model.GetCriterias()));

            return EFCoreExtensions.GetAsync(model, queryWithStatus.OrderByDescending(x => x.Interaction.CreatedOn), MailMessageModel.SelectExpression);
        }
    }
}