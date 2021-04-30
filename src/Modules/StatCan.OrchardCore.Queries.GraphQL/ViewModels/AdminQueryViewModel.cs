using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;

namespace StatCan.OrchardCore.Queries.GraphQL.ViewModels
{
    public class AdminQueryViewModel
    {
        public string DecodedQuery { get; set; }
        public string Parameters { get; set; } = "";

        [BindNever]
        public string RawGraphQL { get; set; }

        [BindNever]
        public TimeSpan Elapsed { get; set; } = TimeSpan.Zero;

        [BindNever]
        public IEnumerable<JObject> Documents { get; set; } = Enumerable.Empty<JObject>();

    }
}
