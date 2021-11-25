using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ContentConverters
{
    public class ProposalContentConverter : BaseContentConverter
    {
        public ProposalContentConverter(BaseContentConverterDependency baseContentConverterDependency) : base(baseContentConverterDependency)
        {

        }

        public override async Task<JObject> ConvertFromFormModelAsync(FormModel formModel, dynamic context)
        {
            ProposalFormModel proposalFormModel = (ProposalFormModel)formModel;

            var proposalContentObject = new
            {
                Proposal = new
                {
                    Type = new
                    {
                        TaxonomyContentItemId = await GetTaxonomyIdAsync("Proposal Types"),
                        TermContentItemIds = new string[] { proposalFormModel.Type["value"] },
                        TagNames = new string[] { proposalFormModel.Type["label"] }
                    }
                },
                RadarEntityPart = new
                {
                    Name = new
                    {
                        Text = proposalFormModel.Name
                    },
                    Description = new
                    {
                        Text = proposalFormModel.Description
                    },
                    Topics = new
                    {
                        TaxonomyContentItemId = await GetTaxonomyIdAsync("Topics"),
                        TermContentItemIds = MapDictListToStringList(proposalFormModel.Topics, topic => topic["value"]),
                        TagNames = MapDictListToStringList(proposalFormModel.Topics, topic => topic["label"])
                    },
                    RelatedEntity = new
                    {
                        LocalizationSets = MapDictListToStringList(proposalFormModel.RelatedEntities, entity => entity["value"])
                    },
                    Publish = new
                    {
                        Value = GetPublishStatus(proposalFormModel.PublishStatus),
                    }
                },
                ContentPermissionsPart = new
                {
                    Enabled = true,
                    Roles = proposalFormModel.Roles
                },
                AutoroutePart = new
                {
                    RouteContainedItems = true,
                }
            };

            return JObject.FromObject(proposalContentObject);
        }
    }
}
