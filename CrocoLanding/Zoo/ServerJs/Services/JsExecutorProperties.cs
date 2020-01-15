using System;
using System.Collections.Generic;
using Croco.Core.Abstractions;
using Jint;
using Zoo.ServerJs.Abstractions;

namespace Zoo.ServerJs.Services
{
    public class JsExecutorProperties
    {
        public ICrocoAmbientContext AmbientContext { get; set; }
        public Action<Engine> EngineProperties { get; set; }

        public List<Func<ICrocoAmbientContext, IJsWorker>> JsWorkers { get; set; }
    }
}
