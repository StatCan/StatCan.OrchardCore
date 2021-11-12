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
using StatCan.OrchardCore.Radar.Services;

namespace StatCan.OrchardCore.Radar.Drivers
{
    public class RadarFormPartDisplayDriver : ContentPartDisplayDriver<RadarFormPart>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IContentManager _contentManager;
        private FormValueProvider _formValueProvider;

        public RadarFormPartDisplayDriver(IHttpContextAccessor httpContextAccessor, IContentManager contentManager, FormValueProvider formValueProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _contentManager = contentManager;
            _formValueProvider = formValueProvider;
        }

        public override IDisplayResult Display(RadarFormPart part, BuildPartDisplayContext context)
        {
            var requestPath = _httpContextAccessor.HttpContext.Request.Path;

            var entityType = "";

            if (!IsFormPath(requestPath) || !TryExtractTypeFromPath(requestPath, out entityType))
            {
                return null;
            }

            return Initialize<RadarFormPart>(GetDisplayShapeType(context), async model =>
            {
                var id = ExtractIdFromPath(requestPath);

                var initialValues = await _formValueProvider.GetInitialValues(entityType, id);

                if (initialValues == null)
                {
                    _httpContextAccessor.HttpContext.Response.StatusCode = 404;
                    _httpContextAccessor.HttpContext.Response.Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}/not-found", false);
                }

                model.InitialValues = JsonConvert.SerializeObject(initialValues).ToString();
            }).Location("Detail", "FormValue:1");
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

        private string ExtractIdFromPath(string path)
        {
            var values = path.Substring(1).Split("/");

            // < 3 means some values are missing
            if (values.Length < 3)
            {
                return null;
            }

            return values[values.Length - 1];
        }

        private bool IsFormPath(string path)
        {
            var values = path.Split("/");

            return Array.Exists(values, pathValue => pathValue == "create" || pathValue == "update");
        }
    }
}
