using System.Threading.Tasks;
using OrchardCore.Documents;
using StatCan.OrchardCore.EmailTemplates.Models;

namespace StatCan.OrchardCore.EmailTemplates.Services
{
    public class EmailTemplatesManager
    {
        private readonly IDocumentManager<EmailTemplatesDocument> _documentManager;

        public EmailTemplatesManager(IDocumentManager<EmailTemplatesDocument> documentManager) => _documentManager = documentManager;

        /// <summary>
        /// Loads the templates document from the store for updating and that should not be cached.
        /// </summary>
        public Task<EmailTemplatesDocument> LoadEmailTemplatesDocumentAsync() => _documentManager.GetOrCreateMutableAsync();

        /// <summary>
        /// Gets the templates document from the cache for sharing and that should not be updated.
        /// </summary>
        public Task<EmailTemplatesDocument> GetEmailTemplatesDocumentAsync() => _documentManager.GetOrCreateImmutableAsync();

        public async Task RemoveTemplateAsync(string id)
        {
            var document = await LoadEmailTemplatesDocumentAsync();
            document.Templates.Remove(id);
            await _documentManager.UpdateAsync(document);
        }

        public async Task UpdateTemplateAsync(string id, EmailTemplate template)
        {
            var document = await LoadEmailTemplatesDocumentAsync();
            document.Templates[id] = template;
            await _documentManager.UpdateAsync(document);
        }
    }
}
