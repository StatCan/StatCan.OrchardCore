using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Infrastructure.Html;
using OrchardCore.Scripting;

namespace StatCan.OrchardCore.Radar.Scription
{
    /// <summary>
    /// This does not need to be registered in the DI as these methods are only used in the VueFormController
    /// </summary>
    // public class RadarFormMethodsProvider : IGlobalMethodProvider
    // {
    //     private readonly GlobalMethod _createTopic;
    //     private readonly GlobalMethod _createProject;
    //     private readonly GlobalMethod _createEvent;
    //     private readonly GlobalMethod _createProposal;
    //     private readonly GlobalMethod _createCommunity;

    //     public RadarFormMethodsProvider()
    //     {

    //     }

    //     public IEnumerable<GlobalMethod> GetMethods()
    //     {
    //         return new[] { _createTopic, _createProject, _createEvent, _createProposal, _createProject };
    //     }
    // }
}
