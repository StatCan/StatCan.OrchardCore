using System.Globalization;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ContentConverters
{
    public class TopicContentConverter : BaseContentConverter
    {
        public TopicContentConverter(BaseContentConverterDenpency baseContentConverterDenpency) : base(baseContentConverterDenpency)
        {

        }
        public override JObject ConvertFromFormModel(FormModel formModel, dynamic context)
        {
            var topicFormModel = (TopicFormModel)formModel;

            if (context.IsUpdate)
            {
                var topicUpdateObject = new
                {
                    Topic = new
                    {
                        Name = new { Text = UpdateLocalizedString(context.existing.Content.Topic.Name.Text.ToString(), topicFormModel.Name, CultureInfo.CurrentCulture.Name) },
                        Description = new { Text = UpdateLocalizedString(context.existing.Content.Topic.Description.Text.ToString(), topicFormModel.Description, CultureInfo.CurrentCulture.Name) }
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
