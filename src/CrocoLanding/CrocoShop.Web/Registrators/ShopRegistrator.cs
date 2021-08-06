using Croco.Core.Application;
using Croco.Core.Logic.Files;
using Dlv.Logic;
using Prd.Logic;

namespace CrocoShop.Web.Registrators
{
    public static class ShopRegistrator
    {
        public static void Register(CrocoApplicationBuilder appBuilder)
        {
            appBuilder.RegisterDbFileManager();
            PrdLogicRegistrator.Register(appBuilder);
            DlvLogicRegistrator.Register(appBuilder);
        }
    }
}