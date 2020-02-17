using Croco.Core.Abstractions.Models;
using Croco.Core.Application;
using CrocoLanding.Controllers;
using Ecc.Contract.Models.Emails;
using Ecc.Logic.Abstractions;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers
{
    public class SendGridEmailSender : IEmailSender
    {
        SendGridClient Client { get; }

        SendGridEmailSettings Settings { get;  }

        public SendGridEmailSender()
        {
            Settings = CrocoApp.Application.SettingsFactory.GetSetting<SendGridEmailSettings>();

            Client = new SendGridClient(Settings.ApiKey);
        }

        public async Task<(TModel, BaseApiResponse)> Execute<TModel>(TModel model, Func<TModel, SendEmailModel> mapper)
        {
            var mappedModel = mapper(model);

            var from = new EmailAddress(Settings.FromAddress, Settings.FromAddress);

            var to = new EmailAddress(mappedModel.Email, mappedModel.Email);

            var msg = MailHelper.CreateSingleEmail(from, to, mappedModel.Subject, mappedModel.Body, mappedModel.Body);

            var response = await Client.SendEmailAsync(msg);

            return (model, new BaseApiResponse(response.StatusCode == System.Net.HttpStatusCode.OK, response.StatusCode.ToString()));
        }

        public async Task<List<(TModel, BaseApiResponse)>> SendEmails<TModel>(IEnumerable<TModel> messages, Func<TModel, SendEmailModel> mapper)
        {
            var result = await Task.WhenAll(messages.Select(x => Execute(x, mapper)));

            return result.ToList();
        }
    }
}
