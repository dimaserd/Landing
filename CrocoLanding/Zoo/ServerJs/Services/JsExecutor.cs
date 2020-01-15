using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Application;
using Croco.Core.Application;
using Croco.Core.Logic.Workers;
using Croco.Core.Models;
using Jint;
using Zoo.ServerJs.Abstractions;

namespace Zoo.ServerJs.Services
{
    public class JsExecutor<TApplication> : BaseCrocoWorker<TApplication> where TApplication : class, ICrocoApplication
    {
        private readonly Action<Engine> _engineProps;
        public List<IJsWorker> EnumerateWorkers() => JsWorkers.Select(x => x(AmbientContext)).ToList();

        public List<Func<ICrocoAmbientContext, IJsWorker>> JsWorkers { get; }

        #region Конструкторы
        public JsExecutor(JsExecutorProperties properties) : base(properties.AmbientContext)
        {
            _engineProps = properties.EngineProperties;
            JsWorkers = properties.JsWorkers;
            _callHandler = new HandleJsCallWorker<TApplication>(AmbientContext, JsWorkers);
        }
        
        #endregion

        #region Поля
        private Engine _engine;

        private readonly HandleJsCallWorker<TApplication> _callHandler;

        private List<List<object>> _logs;
        #endregion

        #region Свойства
        
        private List<List<object>> Logs => _logs ?? (_logs = new List<List<object>>());

        private Engine Engine
        {
            get
            {
                if (_engine != null)
                {
                    return _engine;
                }

                _engine = new Engine();

                _engine.SetValue(JsConsts.ApiObjectName, new
                {
                    //Данное название функции должно быть неизменным относительно
                    Call = new Func<string, string, object[], object>(Call)
                });

                _engine.SetValue("console", new 
                {
                    log = new Action<object[]>(Log)
                });

                _engineProps(_engine);
                
                return _engine;
            }
        }
        
        #endregion

        #region Методы
        public BaseApiResponse<JsScriptExecutedResult> RunScript(string jsScript)
        {
            var startDate = CrocoApp.Application.DateTimeProvider.Now;

            try
            {
                Engine.Execute(jsScript);

                var finishDate = CrocoApp.Application.DateTimeProvider.Now;

                return new BaseApiResponse<JsScriptExecutedResult>(true, "Скрипт выполнен успешно", new JsScriptExecutedResult
                {
                    StartDate = startDate,
                    FinishDate = finishDate,
                    ExecutionMSecs = (finishDate - startDate).TotalMilliseconds,
                    Logs = Logs,
                } );
            }

            catch(Exception ex)
            {
                var finishDate = CrocoApp.Application.DateTimeProvider.Now;

                return new BaseApiResponse<JsScriptExecutedResult>(false, "Ошибка при выполнении скрипта. " + ex.Message, new JsScriptExecutedResult
                {
                    StartDate = startDate,
                    FinishDate = finishDate,
                    ExecutionMSecs = (finishDate - startDate).TotalMilliseconds,
                    ExceptionStackTrace = UnWrapWholeStackTrace(ex)
                });
            }
            
        }



        #endregion

        #region Подключенные методы
        

        protected object Call(string workerName, string methodName, params object[] parameters)
        {
            return _callHandler.Call(workerName, methodName, parameters);
        }

        protected void Log(params object[] objs)
        {
            Logs.Add(objs.ToList());
        }

        #endregion

        private static string UnWrapWholeStackTrace(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }
    }
}
