using Clt.Contract.Services;
using Croco.Core.Abstractions.Application;
using Croco.Core.Extensions;

namespace Clt.Logic.RegistrationModule
{
    public static class CltServiceRegistrator
    {
        public static void Register(ICrocoApplication application)
        {
            application.RegisterService<IClientService, ClientService>(x => new ClientService(x));
        }
    }
}