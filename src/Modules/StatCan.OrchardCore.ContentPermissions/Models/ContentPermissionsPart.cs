using OrchardCore.ContentManagement;
using System;

namespace Etch.OrchardCore.ContentPermissions.Models
{
    public class ContentPermissionsPart : ContentPart
    {
        public bool Enabled { get; set; }
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
