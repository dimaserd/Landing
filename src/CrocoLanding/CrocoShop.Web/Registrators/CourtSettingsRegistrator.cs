using Croco.Core.Contract.Application;
using CrocoShop.Web.Logic.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace CrocoShop.Web.Registrators
{
    public class CourtSettingsRegistrator
    {
        public static void RegisterCourtSettings(IApplicationBuilder app)
        {
            var crocoApp = app.ApplicationServices.GetRequiredService<ICrocoApplication>();
            var filePath = crocoApp.MapPath("~/Configs/CourtSettings.json");

            var data = GetEnvironmentFileData<CourtSettings>(filePath, app.ApplicationServices.GetRequiredService<IWebHostEnvironment>());

            crocoApp.SettingsFactory.UpdateSetting(data);
        }

        public static T GetEnvironmentFileData<T>(string fileName, IWebHostEnvironment environment)
        {
            var bits = fileName.Split(".", StringSplitOptions.RemoveEmptyEntries);
            var fileExt = bits.Last();
            var initPath = bits.First();

            var environmentFileName = $"{initPath}.{environment.EnvironmentName}.{fileExt}";

            var filePath = File.Exists(environmentFileName) ? environmentFileName : fileName;

            var fileText = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<T>(fileText);
        }
    }
}