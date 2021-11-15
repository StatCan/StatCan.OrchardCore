using System.Threading.Tasks;
using System.Collections.Generic;
using OrchardCore.Security.Services;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services
{
    public class FormOptionsProvider
    {
        private readonly IRoleService _roleService;

        public FormOptionsProvider(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<FormOptionModel> GetOptionsAsync(string entityType)
        {
            if (entityType == "topics")
            {
                return await GetRoleOptionsAsync();
            }

            return null;
        }

        private async Task<FormOptionModel> GetRoleOptionsAsync()
        {
            var roles = await _roleService.GetRoleNamesAsync();

            var formOptionModel = new FormOptionModel()
            {
                RoleOptions = new LinkedList<string>(),
            };

            foreach (var role in roles)
            {
                formOptionModel.RoleOptions.Add(role);
            }

            return formOptionModel;
        }
    }
}
