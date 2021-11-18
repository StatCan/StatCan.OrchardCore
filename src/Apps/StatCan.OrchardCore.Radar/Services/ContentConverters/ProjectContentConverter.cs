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
        private readonly IContentManager _contentManager;
        public ProjectContentConverter(IContentManager contentManager, BaseContentConverterDependency baseContentConverterDependency) : base(baseContentConverterDependency)
        {
            _contentManager = contentManager;
        }

        public override async Task<JObject> ConvertFromFormModelAsync(FormModel formModel, object context)
        {
            ProjectFormModel projectFormModel = (ProjectFormModel)formModel;

            var projectContentObject = new
            {
                Published = GetPublishStatus(projectFormModel.PublishStatus),
                Project = new
                {
                    Type = new
                    {
                        TaxonomyContentItemId = await GetTaxonomyIdAsync("Project Types"),
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
                        TaxonomyContentItemId = await GetTaxonomyIdAsync("Topics"),
                        TermContentItemIds = MapStringDictListToStringList(projectFormModel.Topics, topic => topic["value"]),
                        TagNames = MapStringDictListToStringList(projectFormModel.Topics, topic => topic["label"])
                    }
                },
                ContentPermissionsPart = new
                {
                    Enabled = true,
                    Roles = projectFormModel.Roles
                },
                ProjectMember = new
                {
                    ContentItems = await GetProjectMembersContent(projectFormModel.ProjectMembers)
                }
            };

            return JObject.FromObject(projectContentObject);
        }

        private async Task<ICollection<ContentItem>> GetProjectMembersContent(ICollection<IDictionary<string, object>> projectMembers)
        {
            // Contents in bag parts has to be ContentItem

            ICollection<ContentItem> projectMembersContent = new LinkedList<ContentItem>();

            foreach (var projectMember in projectMembers)
            {
                var userObject = (JObject)projectMember["user"];
                var member = new
                {
                    ProjectMember = new
                    {
                        Member = new
                        {
                            UserIds = new string[] { userObject["value"].Value<string>() },
                            UserNames = new string[] { userObject["label"].Value<string>() }
                        },
                        Role = new
                        {
                            Text = projectMember["role"]
                        }
                    }
                };

                var contentItem = await _contentManager.NewAsync("ProjectMember");
                contentItem.Merge(member);

                projectMembersContent.Add(contentItem);
            }

            return projectMembersContent;
        }
    }
}
