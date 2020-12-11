using System.ComponentModel;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.OpenAPI
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterTypeDefinition("OpenAPI", builder => builder
                .Stereotype("Widget") 
                .WithPart("TitlePart", part => part
                .WithSettings<TitlePartSettings>(new TitlePartSettings { RenderTitle = false }))
            );
            return 1;
        }

        internal class TitlePartSettings
        {
            public int Options { get; set; }

            public string Pattern { get; set; }

            [DefaultValue(true)]
            public bool RenderTitle { get; set; }
        }
    }
}