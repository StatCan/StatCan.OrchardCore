using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data;
using YesSql.Indexes;

namespace StatCan.OrchardCore.Candev.Indexes
{
    public class HackathonItemsIndex : MapIndex
    {
        public string ContentItemId { get; set; }
        public string ContentItemVersionId { get; set; }
        public string LocalizationSet { get; set; }
        public string Culture { get; set; }
        public bool Published { get; set; }
        public bool Latest { get; set; }
        public string ContentType { get; set; }
        public DateTime? ModifiedUtc { get; set; }
        public DateTime? PublishedUtc { get; set; }
        public DateTime? CreatedUtc { get; set; }
        public string Owner { get; set; }
        public string Author { get; set; }
        public string DisplayText { get; set; }
        public string Email { get; set; }
        public string TeamContentItemId { get; set; }
        public string CaseLocalizationSet { get; set; }
    }

    public class HackathonItemsIndexProvider : IndexProvider<ContentItem>, IScopedIndexProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private IContentDefinitionManager _contentDefinitionManager;

        public HackathonItemsIndexProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<HackathonItemsIndex>()
                .Map(contentItem =>
                {
                    if (!contentItem.Published && !contentItem.Latest)
                    {
                        return null;
                    }
                    var indexValue = new HackathonItemsIndex
                    {
                        ContentType = contentItem.ContentType,
                        ContentItemId = contentItem.ContentItemId,
                        Owner = contentItem.Owner,
                        Latest = contentItem.Latest,
                        Published = contentItem.Published,
                        ContentItemVersionId = contentItem.ContentItemVersionId,
                        ModifiedUtc = contentItem.ModifiedUtc,
                        PublishedUtc = contentItem.PublishedUtc,
                        CreatedUtc = contentItem.CreatedUtc,
                        Author = contentItem.Author,
                        DisplayText = contentItem.DisplayText
                    };

                    // Assign LocalizationPart fields to index
                    var locPart = contentItem.Content.LocalizationPart;
                    if (locPart != null)
                    {
                        indexValue.LocalizationSet = locPart.LocalizationSet?.Value;
                        indexValue.Culture = locPart.Culture?.Value;
                    }

                    if (contentItem.ContentType == "Case")
                    {
                        indexValue.CaseLocalizationSet = (string)contentItem.Content.LocalizationPart.LocalizationSet?.Value;
                        return indexValue;
                    }

                    // Lazy initialization because of ISession cyclic dependency
                    _contentDefinitionManager ??= _serviceProvider.GetRequiredService<IContentDefinitionManager>();
                    // Gets the list of field definition that have a Hackathon LocalizedContentPickerField
                    var hackathonElementsFields = _contentDefinitionManager
                        .GetTypeDefinition(contentItem.ContentType)
                        .Parts.SelectMany(x => x.PartDefinition.Fields.Where(f =>
                            (f.FieldDefinition.Name == nameof(LocalizationSetContentPickerField) && f.Name == "Hackathon")))
                        .ToArray();

                    var caseElementsFields = _contentDefinitionManager
                        .GetTypeDefinition(contentItem.ContentType)
                        .Parts.SelectMany(x => x.PartDefinition.Fields.Where(f =>
                            (f.FieldDefinition.Name == nameof(LocalizationSetContentPickerField) && f.Name == "Case")))
                        .ToArray();
                    // assign the CaseLocalizationSet to the index
                    foreach (var fieldDefinition in caseElementsFields)
                    {
                        var jPart = (JObject)contentItem.Content[fieldDefinition.PartDefinition.Name];

                        if (jPart == null)
                        {
                            continue;
                        }
                        var jField = (JObject)jPart[fieldDefinition.Name];
                        if (jField == null)
                        {
                            continue;
                        }
                        var field = jField.ToObject<LocalizationSetContentPickerField>();
                        indexValue.CaseLocalizationSet = field.LocalizationSets?.FirstOrDefault();
                    }
                    // Special case for Judges, the CaseLocalizationSet comes from the AssignedCase field.
                    if (contentItem.ContentType == "Volunteer" && contentItem.Content.VolunteerJudge?.IsJudge?.Value == true)
                    {
                        indexValue.CaseLocalizationSet = contentItem.Content.VolunteerJudge?.AssignedCase?.LocalizationSets?.First;
                        //indexValue.IsJudge = true;
                    }

                    // Attempt to grab the Team and Email fields from the type definition
                    var participantFields = _contentDefinitionManager
                        .GetTypeDefinition(contentItem.ContentType)
                        .Parts.SelectMany(x => x.PartDefinition.Fields.Where(f =>
                            (f.FieldDefinition.Name == nameof(TextField) && f.Name == "Email") ||
                            (f.FieldDefinition.Name == nameof(ContentPickerField) && f.Name == "Team")))
                        .ToArray();

                    foreach (var fieldDefinition in participantFields)
                    {
                        var jPart = (JObject)contentItem.Content[fieldDefinition.PartDefinition.Name];
                        if (jPart == null)
                        {
                            continue;
                        }
                        var jField = (JObject)jPart[fieldDefinition.Name];
                        if (jField == null)
                        {
                            continue;
                        }
                        if (fieldDefinition.Name == "Team")
                        {
                            var team = jField.ToObject<ContentPickerField>();
                            if (team != null)
                            {
                                indexValue.TeamContentItemId = team.ContentItemIds?.FirstOrDefault();
                            }
                        }
                        if (fieldDefinition.Name == "Email")
                        {
                            var email = jField.ToObject<TextField>();
                            if (email != null)
                            {
                                indexValue.Email = email.Text;
                            }
                        }
                    }

                    if (contentItem.ContentType == "JudgingEntry")
                    {
                        //indexValue.JudgeId = (string)contentItem.Content.JudgingEntry.Judge?.ContentItemIds.First;
                    }

                    // Special case for Team, assign the TeamContentItemId.
                    if (contentItem.ContentType == "Team")
                    {
                        indexValue.TeamContentItemId = contentItem.ContentItemId;
                    }
                    return indexValue;
                });
        }
    }
}
