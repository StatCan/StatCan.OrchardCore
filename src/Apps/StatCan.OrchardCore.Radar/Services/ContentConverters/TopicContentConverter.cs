using System.Globalization;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ContentConverters
{
    public class TopicContentConverter : BaseContentConverter
    {
        public TopicContentConverter(BaseContentConverterDependency baseContentConverterDependency) : base(baseContentConverterDependency)
        {

        }
        public override JObject ConvertFromFormModel(FormModel formModel, dynamic context)
        {
            var topicFormModel = (TopicFormModel)formModel;

            if (context != null)
            {
                var topicUpdateObject = new
                {
                    Topic = new
                    {
                        Name = new { Text = UpdateLocalizedString(context.Existing.Content.Topic.Name.Text.ToString(), topicFormModel.Name, CultureInfo.CurrentCulture.Name) },
                        Description = new { Text = UpdateLocalizedString(context.Existing.Content.Topic.Description.Text.ToString(), topicFormModel.Description, CultureInfo.CurrentCulture.Name) }
                    },
                    ContentPermissionsPart = new
                    {
                        Enabled = true,
                        Roles = topicFormModel.Roles,
                    }
                };

                return JObject.FromObject(topicUpdateObject);
            }
            else
            {
                var topicCreateObject = new
                {
                    Topic = new
                    {
                        Name = new { Text = CreateLocalizedString(topicFormModel.Name, CultureInfo.CurrentCulture.Name) },
                        Description = new { Text = CreateLocalizedString(topicFormModel.Description, CultureInfo.CurrentCulture.Name) }
                    },
                    ContentPermissionsPart = new
                    {
                        Enabled = true,
                        Roles = topicFormModel.Roles,
                    }
                };

                return JObject.FromObject(topicCreateObject);
            }
        }
    }
}
