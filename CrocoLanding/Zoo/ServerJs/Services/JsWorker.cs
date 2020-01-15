using System.Linq;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Application;
using Croco.Core.Logic.Workers;
using Croco.Core.Utils;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;

namespace Zoo.ServerJs.Services
{
    public abstract class JsWorker<TApplication> : BaseCrocoWorker<TApplication>, IJsWorker where TApplication: class, ICrocoApplication
    {
        protected JsWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }

        /// <summary>
        /// Получить скрипт для данного рабочего класса
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetScript(string methodName, params object[] parameters)
        {
            var paramString = string.Join(",", parameters.Select(x => Tool.JsonConverter.Serialize(x)));

            return $"{JsConsts.ApiObjectName}.{JsConsts.CallFunctionName}('{JsWorkerDocs().WorkerName}', '{methodName}', {paramString})";
        }

        public abstract JsWorkerDocumentation JsWorkerDocs();
    }
}
