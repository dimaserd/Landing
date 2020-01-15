using System;
using System.Collections.Generic;

namespace Zoo.ServerJs.Models
{
    public class JsWorkerMethodDocs
    {
        /// <summary>
        /// Типы входных параметров
        /// </summary>
        public List<Type> Parameters { get; set; }

        /// <summary>
        /// Тип возвращаемого значения 
        /// </summary>
        public Type Response { get; set; }

        /// <summary>
        /// Ссылка на метод
        /// </summary>
        public JsWorkerMethodBase Method { get; set; }

        /// <summary>
        /// Название метода
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Описание метода
        /// </summary>
        public string Description { get; set; }


        public static JsWorkerMethodDocs GetMethod<TResult>(JsWorkerMethodDocsOptions options, Func<TResult> func)
        {
            return new JsWorkerMethodDocs
            {
                MethodName = options.MethodName,
                Description = options.Description,
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = p => new JsWorkerMethodResult
                    {
                        Result = func()
                    }
                },
                Parameters = new List<Type> {  },
                Response = typeof(TResult)
            };
        }

        public static JsWorkerMethodDocs GetMethod<TParam, TResult>(JsWorkerMethodDocsOptions options, Func<TParam, TResult> func)
        {
            return new JsWorkerMethodDocs
            {
                MethodName = options.MethodName,
                Description = options.Description,
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = p => new JsWorkerMethodResult
                    {
                        Result = func(p.GetParameter<TParam>())
                    }
                },
                Parameters = new List<Type> { typeof(TParam) },
                Response = typeof(TResult)
            };
        }
    }
}
