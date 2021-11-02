using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Mvc.Utilities;

namespace StatCan.OrchardCore.VueForms.Controllers
{
    [Route("api/contents")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly IContentManager _contentManager;

        public TestController(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }
    }
}
