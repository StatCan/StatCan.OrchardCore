using System.Globalization;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ContentConverters
{
    public class ArtifactContentConverter : BaseContentConverter
    {
        public ArtifactContentConverter(BaseContentConverterDependency baseContentConverterDependency) : base(baseContentConverterDependency)
        {

        }
        public override JObject ConvertFromFormModel(FormModel formModel, dynamic context)
        {
            var artifactFormModel = (ArtifactFormModel)formModel;

            if (context != null)
            {
                var artifactUpdateObject = new
                {
                    Artifact = new
                    {
                        URL = new { Text = artifactFormModel.Url },
                    },
                    ContentPermissionsPart = new
                    {
                        Enabled = true,
                        Roles = artifactFormModel.Roles,
                    },
                    TitlePart = new
                    {
                        Title = UpdateLocalizedString(context.Existing.DisplayText.ToString(), artifactFormModel.Name, CultureInfo.CurrentCulture.Name)
                    }
                };

                return JObject.FromObject(artifactUpdateObject);
            }
            else
            {
                var artifactCreateObject = new
                {
                    Artifact = new
                    {
                        URL = new { Text = artifactFormModel.Url },
                    },
                    ContentPermissionsPart = new
                    {
                        Enabled = true,
                        Roles = artifactFormModel.Roles,
                    },
                    TitlePart = new
                    {
                        Title = CreateLocalizedString(artifactFormModel.Name, CultureInfo.CurrentCulture.Name)
                    }
                };

                return JObject.FromObject(artifactCreateObject);
            }
        }
    }
}
