using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Taxonomies.Models;
using OrchardCore.Security.Services;
using OrchardCore.Queries;
using OrchardCore.Shortcodes.Services;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services
{
    public class FormOptionsProvider
    {
        private readonly IRoleService _roleService;
        private readonly IQueryManager _queryManager;
        private readonly IShortcodeService _shortcodeService;

        public FormOptionsProvider(IRoleService roleService, IQueryManager queryManager, IShortcodeService shortcodeService)
        {
            _roleService = roleService;
            _queryManager = queryManager;
            _shortcodeService = shortcodeService;
        }

        public async Task<FormOptionModel> GetOptionsAsync(string entityType)
        {
            if (entityType == "topics")
            {
                return await GetTopicFormOptionsAsync();
            }
            else if (entityType == "projects")
            {
                return await GetProjectFormOptionsAsync();
            }

            return null;
        }

        private async Task<FormOptionModel> GetTopicFormOptionsAsync()
        {
            var roles = await _roleService.GetRoleNamesAsync();

            var formOptionModel = new FormOptionModel()
            {
                RoleOptions = new LinkedList<string>(),
            };

            await FillRoleOptionsAsync(formOptionModel);

            return formOptionModel;
        }

        private async Task<FormOptionModel> GetProjectFormOptionsAsync()
        {
            var projectFormModel = new EntityFormOptionModel()
            {
                RoleOptions = new LinkedList<string>(),
                TypeOptions = new LinkedList<IDictionary<string, string>>(),
            };

            await FillRoleOptionsAsync(projectFormModel);
            await FillTypeOptionsAsync(projectFormModel, "Project Types");
            await FillVisibilityOptionsAsync(projectFormModel);

            return projectFormModel;
        }

        private async Task FillVisibilityOptionsAsync(EntityFormOptionModel formOptionModel)
        {
            var localizedPublishOption = await _shortcodeService.ProcessAsync("[locale en]Publish[/locale][locale fr]Publier[/locale]");
            var localizedDraftOption = await _shortcodeService.ProcessAsync("[locale en]Draft[/locale][locale fr]Brouillon[/locale]");

            formOptionModel.PublishOptions = new string[] { localizedPublishOption, localizedDraftOption };
        }
        private async Task FillRoleOptionsAsync(FormOptionModel formOptionModel)
        {
            var roles = await _roleService.GetRoleNamesAsync();

            foreach (var role in roles)
            {
                formOptionModel.RoleOptions.Add(role);
            }
        }

        private async Task FillTypeOptionsAsync(EntityFormOptionModel formOptionModel, string type)
        {
            var taxonomyQuery = await _queryManager.GetQueryAsync("AllTaxonomiesSQL");
            var taxonomyResult = await _queryManager.ExecuteQueryAsync(taxonomyQuery, new Dictionary<string, object> { { "type", type } });

            if (taxonomyResult != null)
            {
                var taxonomyPart = (taxonomyResult.Items.First() as ContentItem).As<TaxonomyPart>();

                foreach (var taxonomy in taxonomyPart.Terms)
                {
                    var optionPair = new Dictionary<string, string>()
                    {
                        {"value", taxonomy.ContentItemId},
                        {"label", await _shortcodeService.ProcessAsync(taxonomy.DisplayText)}
                    };

                    formOptionModel.TypeOptions.Add(optionPair);
                }
            }
        }
    }
}
