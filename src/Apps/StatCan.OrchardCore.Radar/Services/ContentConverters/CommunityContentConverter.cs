using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ContentConverters
{
    public class CommunityContentConverter : BaseContentConverter
    {
        public CommunityContentConverter(BaseContentConverterDependency baseContentConverterDependency) : base(baseContentConverterDependency)
        {

        }
        public override async Task<JObject> ConvertFromFormModelAsync(FormModel formModel, object context)
        {
            CommunityFormModel communityFormModel = (CommunityFormModel)formModel;

            var communityContentObject = new
            {
                Published = GetPublishStatus(communityFormModel.PublishStatus),
                Community = new
                {
                    Type = new
                    {
                        TaxonomyContentItemId = await GetTaxonomyIdAsync("Community Types"),
                        TermContentItemIds = new string[] { communityFormModel.Type["value"] },
                        TagNames = new string[] { communityFormModel.Type["label"] }
                    }
                },
                RadarEntityPart = new
                {
                    Name = new
                    {
                        Text = communityFormModel.Name
                    },
                    Description = new
                    {
                        Text = communityFormModel.Description
                    },
                    Topics = new
                    {
                        TaxonomyContentItemId = await GetTaxonomyIdAsync("Topics"),
                        TermContentItemIds = MapDictListToStringList(communityFormModel.Topics, topic => topic["value"]),
                        TagNames = MapDictListToStringList(communityFormModel.Topics, topic => topic["label"])
                    },
                    Publish = new
                    {
                        Value = GetPublishStatus(communityFormModel.PublishStatus),
                    }
                },
                ContentPermissionsPart = new
                {
                    Enabled = true,
                    Roles = communityFormModel.Roles
                },
                CommunityMember = new
                {
                    ContentItems = await GetMembersContentAsync(communityFormModel.CommunityMembers, "CommunityMember", member =>
                    {
                        var userObject = (JObject)member["user"];
                        var memberObject = new
                        {
                            CommunityMember = new
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
                }
            };

            return JObject.FromObject(communityContentObject);
        }
    }
}
