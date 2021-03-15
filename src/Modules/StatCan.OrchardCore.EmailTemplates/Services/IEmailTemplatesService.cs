using OrchardCore.ContentManagement;
using OrchardCore.Email;
using StatCan.OrchardCore.EmailTemplates.ViewModels;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.EmailTemplates.Services
{
    public interface IEmailTemplatesService
    {
        MailMessage CreateMessageFromViewModel(SendEmailTemplateViewModel sendEmail);
        Task<string> RenderLiquid(string liquid, ContentItem contentItem);
    }
}
