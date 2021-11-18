using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Queries;
using StatCan.OrchardCore.Radar.FormModels;


namespace StatCan.OrchardCore.Radar.Services.ContentConverters
{
    public abstract class BaseContentConverter : IContentConverter
    {
        private readonly IQueryManager _queryManager;

        public BaseContentConverter(BaseContentConverterDependency baseContentConverterDependency)
        {
            _queryManager = baseContentConverterDependency.GetQueryManager();
        }

        public Task<JObject> ConvertAsync(FormModel formModel, dynamic context)
        {
            return ConvertFromFormModelAsync(formModel, context);
        }

        public virtual JObject ConvertFromFormModel(FormModel formModel, dynamic context)
        {
            return null;
        }

        public virtual Task<JObject> ConvertFromFormModelAsync(FormModel formModel, dynamic context)
        {
            return Task.FromResult(ConvertFromFormModel(formModel, context));
        }

        protected async Task<string> GetTaxonomyIdAsync(string type)
        {
            var topicQuery = await _queryManager.GetQueryAsync("AllTaxonomiesSQL");
            var topicResult = await _queryManager.ExecuteQueryAsync(topicQuery, new Dictionary<string, object> { { "type", type } });

            if (topicResult == null)
            {
                return null;
            }

            var topicTaxonomy = topicResult.Items.First() as ContentItem;

            return topicTaxonomy.ContentItemId;
        }

        protected bool GetPublishStatus(string statusString)
        {
            if(statusString == "Publish" || statusString == "Publier")
            {
                return true;
            } else
            {
                return false;
            }
        }

        // Maps object with all string properties to string list
        protected ICollection<string> MapStringDictListToStringList(ICollection<IDictionary<string, string>> list, Func<IDictionary<string, string>, string> func)
        {
            ICollection<string> stringList = new LinkedList<string>();

            foreach (var item in list)
            {
                string value = func(item);
                stringList.Add(value);
            }

            return stringList;
        }

        protected string CreateLocalizedString(string content, string currentCulture)
        {
            var supportedCultures = new string[] { "en", "fr" }; // Assumming only en and fr
            StringBuilder sb = new StringBuilder();

            foreach (var supportedCulture in supportedCultures)
            {
                sb.Append($"[locale {supportedCulture}]");
                sb.Append(content);
                sb.Append("[/locale]");
            }

            return sb.ToString();
        }

        protected IDictionary<string, string> ExtractLocalizedString(string localizedString)
        {
            var supportedCultures = new string[] { "en", "fr" }; // Assumming only en and fr
            var localizedStrings = new Dictionary<string, string>();

            foreach (var supportedCulture in supportedCultures)
            {
                var leftTag = $"[locale {supportedCulture}]";
                var rightTag = "[/locale]";

                var startingIndex = localizedString.IndexOf(leftTag) + leftTag.Length;
                var endingIndex = localizedString.IndexOf(rightTag, startingIndex);

                var content = localizedString.Substring(startingIndex, endingIndex - startingIndex);

                localizedStrings.Add(supportedCulture, content);
            }

            return localizedStrings;
        }

        protected string UpdateLocalizedString(string orginal, string insert, string currentCulture)
        {
            IDictionary<string, string> localizedStrings = ExtractLocalizedString(orginal);
            var supportedCultures = new string[] { "en", "fr" }; // Assumming only en and fr
            StringBuilder sb = new StringBuilder();

            foreach (var supportedCulture in supportedCultures)
            {
                sb.Append($"[locale {supportedCulture}]");
                if (currentCulture == supportedCulture)
                {
                    sb.Append(insert);
                }
                else
                {
                    sb.Append(localizedStrings[supportedCulture]);
                }
                sb.Append("[/locale]");
            }

            return sb.ToString();
        }
    }
}
