using System;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Environment.Shell.Scope;
using OrchardCore.Scripting;
using OrchardCore.Scripting.JavaScript;

namespace StatCan.OrchardCore.VueForms.Services
{
    public class ZodEvaluator
    {
        private readonly IScriptingManager _scriptingManager;
        private readonly IServiceProvider _serviceProvider;

        // The scope is built lazily once per request.
        private JavaScriptScope _scope;
        private JavaScriptEngine _engine;


        public ZodEvaluator(IScriptingManager scriptingManager, IServiceProvider serviceProvider)
        {
            _scriptingManager = scriptingManager;
            _serviceProvider = serviceProvider;
            _engine ??= _scriptingManager.GetScriptingEngine("js") as JavaScriptEngine;
            _scope ??= _engine.CreateScope(_scriptingManager.GlobalMethodProviders.SelectMany(x => x.GetMethods()), _serviceProvider, null, null) as JavaScriptScope;

            _scope.Engine.

        }
    }
}
