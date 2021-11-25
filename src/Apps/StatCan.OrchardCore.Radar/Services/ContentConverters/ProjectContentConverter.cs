using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ContentConverters
{
    public class ProjectContentConverter : BaseContentConverter
    {
        public ProjectContentConverter(BaseContentConverterDependency baseContentConverterDependency) : base(baseContentConverterDependency)
        {

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
                        TermContentItemIds = MapDictListToStringList(projectFormModel.Topics, topic => topic["value"]),
                        TagNames = MapDictListToStringList(projectFormModel.Topics, topic => topic["label"])
                    },
                    Publish = new
                    {
                        Value = GetPublishStatus(projectFormModel.PublishStatus),
                    }
                },
                ContentPermissionsPart = new
                {
                    Enabled = true,
                    Roles = projectFormModel.Roles
                },
                ProjectMember = new
                {
                    ContentItems = await GetMembersContentAsync(projectFormModel.ProjectMembers, "ProjectMember", member =>
                    {
                        var userObject = (JObject)member["user"];
                        var memberObject = new
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
                                    Text = member["role"]
                                }
                            }
                        };

                        return memberObject;
                    })
                },
                AutoroutePart = new
                {
                    RouteContainedItems = true,
                }
            };

            return JObject.FromObject(projectContentObject);
        }
    }
}
