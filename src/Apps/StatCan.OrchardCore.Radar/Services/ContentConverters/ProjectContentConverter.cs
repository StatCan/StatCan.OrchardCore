using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Queries;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ContentConverters
{
    public class ProjectContentConverter : BaseContentConverter
    {
        public ProjectContentConverter(BaseContentConverterDenpency baseContentConverterDenpency) : base(baseContentConverterDenpency)
        {

        }

        public override JObject ConvertFromFormModel(FormModel formModel, object context)
        {
            ProjectFormModel projectFormModel = (ProjectFormModel)formModel;

            var projectContentObject = new
            {
                Project = new
                {
                    Type = new
                    {
                        TaxonomyContentItemId = GetTaxonomyIdAsync("Project Types"),
                        TermContentItemIds = new string[] { projectFormModel.Type["value"] },
                        TagNames = new string[] { projectFormModel.Type["label"] }
                    }
                },
                RadarEntityPart = new
                {
                    Name = new
                    {
                        Text = projectFormModel.Name
                    },
                    Description = new
                    {
                        Text = projectFormModel.Description
                    },
                    Topics = new
                    {
                        TaxonomyContentItemId = GetTaxonomyIdAsync("Topics"),
                        TermContentItemIds = MapStringDictListToStringList(projectFormModel.Topics, topic => topic["value"]),
                        TagNames = MapStringDictListToStringList(projectFormModel.Topics, topic => topic["label"])
                    }
                },
                ContentPermissionsPart = new
                {
                    Enabled = true,
                    Roles = projectFormModel.Roles
                },
                ProjectMembers = new
                {
                    // ContentItems =
                }
            };

            return JObject.FromObject(projectContentObject);
        }
    }
}
