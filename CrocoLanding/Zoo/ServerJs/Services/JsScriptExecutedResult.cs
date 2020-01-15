using System;
using System.Collections.Generic;

namespace Zoo.ServerJs.Services
{
    public class JsScriptExecutedResult
    {
        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public double ExecutionMSecs { get; set; }

        public List<List<object>> Logs { get; set; }

        public string ExceptionStackTrace { get; set; }
    }
}
