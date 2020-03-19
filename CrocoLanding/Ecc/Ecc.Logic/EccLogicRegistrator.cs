using Croco.Core.Abstractions.Application;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Handlers;

namespace Ecc.Logic
{
    public static class EccLogicRegistrator
    {
        public static void AddMessageHandlers(ICrocoApplicationOptions applicationOptions)
        {
            applicationOptions.EventSourceOptions.AddMessageHandler<SendMailsForEmailGroup, SendMailsForEmailGroupMessageHandler>();
        }
    }
}