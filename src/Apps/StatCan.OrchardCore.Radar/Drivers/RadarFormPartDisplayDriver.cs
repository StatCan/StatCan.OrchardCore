using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.Views;
using StatCan.OrchardCore.Radar.Models;

namespace StatCan.OrchardCore.Radar.Drivers
{
    public class RadarFormPartDisplayDriver : ContentPartDisplayDriver<RadarFormPart>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IContentManager _contentManager;

        public RadarFormPartDisplayDriver(IHttpContextAccessor httpContextAccessor, IContentManager contentManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _contentManager = contentManager;
        }

        public override async Task<IDisplayResult> DisplayAsync(RadarFormPart part, BuildPartDisplayContext context)
        {
            var requestPath = _httpContextAccessor.HttpContext.Request.Path;

            var entityType = "";
            var id = "";

            if (!IsFormPath(requestPath) || !TryExtractTypeFromPath(requestPath, out entityType))
            {
                return null;
            }

            var initialValues = "";

            if (!TryExtractIdFromPath(requestPath, out id))
            {
                // This means the form is for create
            }
            else
            {
                // This means update
                var contentItem = await _contentManager.GetAsync(id);

                if (contentItem == null)
                {
                    _httpContextAccessor.HttpContext.Response.Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}/not-found", false);
                }

                initialValues = JsonConvert.SerializeObject(contentItem).ToString();
            }

            return Initialize<RadarFormPart>(GetDisplayShapeType(context), viewModel => viewModel.InitialValues = initialValues).Location("Detail", "Content:10");
        }

        // Tries to extract the type of entity. The path is expected to have the form /{entity}/{create,update}/{id}
        private bool TryExtractTypeFromPath(string path, out string entityName)
        {
            var values = path.Substring(1).Split("/");

            // < 3 means some values are missing
            if (values.Length < 2)
            {
                entityName = "";
                return false;
            }

            entityName = values[0];

            return true;
        }

        private bool TryExtractIdFromPath(string path, out string id)
        {
            var values = path.Substring(1).Split("/");

            // < 3 means some values are missing
            if (values.Length < 3)
            {
                id = "";
                return false;
            }

            id = values[values.Length - 1];

            return true;
        }

        private bool IsFormPath(string path)
        {
            var values = path.Split("/");

            return Array.Exists(values, pathValue => pathValue == "create" || pathValue == "update");
        }
    }
}
