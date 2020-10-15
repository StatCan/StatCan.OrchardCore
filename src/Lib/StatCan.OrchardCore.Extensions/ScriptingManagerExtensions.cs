using OrchardCore.Scripting;

namespace StatCan.OrchardCore.Extensions
{
    public static class ScriptingManagerExtensions
    {
        /// <summary>
        /// Executes javascript by prefixing the script .
        /// </summary>
        /// <param name="script">The script to evaluate</param>
        /// <param name="scopedMethodProviders">A list of method providers scoped to the script evaluation.</param>
        /// <returns>The result of the script if any.</returns>
        public static object EvaluateJs(this IScriptingManager  scriptingManager, string script, params IGlobalMethodProvider[] scopedMethodProviders)
        {
            var directive = $"js:{script}";
            return scriptingManager.Evaluate(directive, null, null, scopedMethodProviders);
        }
    }
}
