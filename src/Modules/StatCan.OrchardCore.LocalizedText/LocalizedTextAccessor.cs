using System.Collections.Generic;
using System.Linq;
using StatCan.OrchardCore.LocalizedText.Fields;

namespace StatCan.OrchardCore.LocalizedText
{
    public interface ILocalizedTextAccessor
    {
        /// <summary>
        /// Adds a LocalizedTextPart to the list of parts available to search for translations
        /// </summary>
        void AddLocalizedItem(LocalizedTextPart part);
        string GetTranslation(string culture, string key);
    }
    public class LocalizedTextAccessor : ILocalizedTextAccessor
    {
        private readonly List<LocalizedTextPart> parts = new List<LocalizedTextPart>();
        public void AddLocalizedItem(LocalizedTextPart part)
        {
            parts.Add(part);
        }

        public string GetTranslation(string culture, string key)
        {
            for (int i = parts.Count - 1; i >= 0; i--)
            {
                 var value = parts[i].Data.FirstOrDefault(lt => lt.Name == key)?.LocalizedItems.FirstOrDefault(li => li.Culture == culture)?.Value;
                if(value != null)
                {
                    return value;
                }
            }
            return null;
        }
    }
}