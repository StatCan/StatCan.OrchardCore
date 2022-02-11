using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using StatCan.OrchardCore.Radar.Models;
using StatCan.OrchardCore.Radar.Services;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Drivers
{
    public class RadarFormPartDisplayDriver : ContentPartDisplayDriver<RadarFormPart>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FormValueProvider _formValueProvider;
        private readonly FormOptionsProvider _formOptionsProvider;

        public RadarFormPartDisplayDriver(IHttpContextAccessor httpContextAccessor, FormValueProvider formValueProvider, FormOptionsProvider formOptionsProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _formValueProvider = formValueProvider;
            _formOptionsProvider = formOptionsProvider;
        }

        public override IDisplayResult Display(RadarFormPart part, BuildPartDisplayContext context)
        {
            var requestPath = _httpContextAccessor.HttpContext.Request.Path;

            var parentType = "";
            var entityType = "";

            if (IsFormPath(requestPath))
            {
                bool pathResolved = TryExtractParentAndChildTypeFromPath(requestPath, out parentType, out entityType) || TryExtractTypeFromPath(requestPath, out entityType);

                if (!pathResolved)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            return Initialize<RadarFormPart>(GetDisplayShapeType(context), async model =>
            {
                FormModel initialValues = null;

                if (!string.IsNullOrEmpty(parentType))
                {
                    // Speical case for entities that have parents
                    (string parentId, string childId) = ExtractParentAndChildIdFromPath(requestPath);

                    initialValues = await _formValueProvider.GetInitialValuesAsync(entityType, parentId, childId);
                }
                else
                {
                    var id = ExtractIdFromPath(requestPath);
                    initialValues = await _formValueProvider.GetInitialValuesAsync(entityType, id);
                }

                if (initialValues == null)
                {
                    _httpContextAccessor.HttpContext.Response.StatusCode = 404;
                    _httpContextAccessor.HttpContext.Response.Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}/not-found", false);
                }

                var options = await _formOptionsProvider.GetOptionsAsync(entityType);

                model.InitialValues = JsonConvert.SerializeObject(initialValues).ToString();
                model.Options = JsonConvert.SerializeObject(options).ToString();
            }).Location("Detail", "FormValue:1");
        }

        // Tries to extract the type of entity. The path is expected to have the form /{entity}/{create,update}/{id}
        private bool TryExtractTypeFromPath(string path, out string entityName)
        {
            var values = path.Substring(1).Split("/");

            // < 2 means some values are missing
            if (values.Length < 2)
            {
                entityName = "";
                return false;
            }

            entityName = values[0];

            return true;
        }

        // Extracts the type of the parent entities. Used for contents that belong to other contents
        private bool TryExtractParentAndChildTypeFromPath(string path, out string parentName, out string childName)
        {
            var values = path.Substring(1).Split("/");

            // < 4 means some values are missing
            if (values.Length < 4)
            {
                parentName = "";
                childName = "";

                return false;
            }

            parentName = values[0];
            childName = values[2];

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

        private (string, string) ExtractParentAndChildIdFromPath(string path)
        {
            var values = path.Substring(1).Split("/");

            // < 5 means some values are missing
            if (values.Length < 4)
            {
                return (null, null);
            }
            else if(values.Length < 5)
            {
                return (values[1], null);
            }

            return (values[1], values[values.Length - 1]);
        }

        private bool IsFormPath(string path)
        {
            var values = path.Split("/");

            return Array.Exists(values, pathValue => pathValue == "create" || pathValue == "update");
        }
    }
}
