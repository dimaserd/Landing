using Ecc.Contract.Models.Emails;
using Ecc.Implementation.Services;
using Ecc.Implementation.Settings;
using Ecc.Logic.Core.Workers;
using Ecc.Logic.Extensions;
using System;

namespace Landing.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            var urlSubstr = new AppEccEmailLinkSubstitutor("https://crocosoft.ru/Redirect/To/{0}")

            var sendGridSender = new SendGridEmailSender(new SendGridEmailSettings());

            sendGridSender.SendEmail(new SendEmailModel
            {
            })

            Console.WriteLine("Hello World!");
        }
    }
}