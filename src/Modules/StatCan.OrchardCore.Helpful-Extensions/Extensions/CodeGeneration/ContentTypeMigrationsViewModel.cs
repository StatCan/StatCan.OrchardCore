using System;

namespace Lombiq.HelpfulExtensions.Extensions.CodeGeneration
{
    public class ContentTypeMigrationsViewModel
    {
        internal Lazy<string> MigrationCodeLazy { get; set; }
        public string MigrationCode => MigrationCodeLazy.Value;
    }
}
