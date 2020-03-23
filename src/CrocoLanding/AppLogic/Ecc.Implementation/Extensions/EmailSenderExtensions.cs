using Croco.Core.Abstractions.Models;
using Ecc.Contract.Models.Emails;
using Ecc.Logic.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Extensions
{
    public static class EmailSenderExtensions
    {
        public static async Task<(TModel, BaseApiResponse)> SendEmail<TModel>(this IEmailSender emailSender, TModel message, Func<TModel, SendEmailModel> mapper)
        {
            return (await emailSender.SendEmails(new[] { message }, mapper)).First();
        }
    }
}