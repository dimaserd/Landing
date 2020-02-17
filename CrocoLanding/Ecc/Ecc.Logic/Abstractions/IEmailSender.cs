using Croco.Core.Abstractions.Models;
using Ecc.Contract.Models.Emails;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecc.Logic.Abstractions
{
    public interface IEmailSender
    {
        Task<List<(TModel, BaseApiResponse)>> SendEmails<TModel>(IEnumerable<TModel> messages, System.Func<TModel, SendEmailModel> mapper);
    }
}