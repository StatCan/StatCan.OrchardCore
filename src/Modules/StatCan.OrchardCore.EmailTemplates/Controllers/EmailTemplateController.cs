using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Fluid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Email;
using OrchardCore.Liquid;
using OrchardCore.Navigation;
using OrchardCore.Routing;
using OrchardCore.Settings;
using StatCan.OrchardCore.EmailTemplates.Models;
using StatCan.OrchardCore.EmailTemplates.Services;
using StatCan.OrchardCore.EmailTemplates.ViewModels;

namespace StatCan.OrchardCore.EmailTemplates.Controllers
{
    [Admin]
    public class EmailTemplateController : Controller
    {

        private readonly IAuthorizationService _authorizationService;
        private readonly EmailTemplatesManager _templatesManager;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly ILiquidTemplateManager _liquidTemplateManager;
        private readonly ISiteService _siteService;
        private readonly INotifier _notifier;
        private readonly ISmtpService _smtpService;
        private readonly IStringLocalizer S;
        private readonly IHtmlLocalizer H;
        private readonly dynamic New;

        public EmailTemplateController(
            IAuthorizationService authorizationService,
            EmailTemplatesManager templatesManager,
            IShapeFactory shapeFactory,
            ISiteService siteService,
            IStringLocalizer<EmailTemplateController> stringLocalizer,
            IHtmlLocalizer<EmailTemplateController> htmlLocalizer,
            INotifier notifier,
            ISmtpService smtpService,
            ILiquidTemplateManager liquidTemplateManager,
            HtmlEncoder htmlEncoder
        )
        {
            _authorizationService = authorizationService;
            _templatesManager = templatesManager;
            New = shapeFactory;
            _siteService = siteService;
            _notifier = notifier;
            _smtpService = smtpService;
            S = stringLocalizer;
            H = htmlLocalizer;
            _liquidTemplateManager = liquidTemplateManager;
            _htmlEncoder = htmlEncoder;
        }

        public async Task<IActionResult> Index(ContentOptions options, PagerParameters pagerParameters)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }

            var siteSettings = await _siteService.GetSiteSettingsAsync();
            var pager = new Pager(pagerParameters, siteSettings.PageSize);
            var templatesDocument = await _templatesManager.GetEmailTemplatesDocumentAsync();

            var templates = templatesDocument.Templates.ToList();

            if (!string.IsNullOrWhiteSpace(options.Search))
            {
                templates = templates.Where(x => x.Key.Contains(options.Search)).ToList();
            }

            var count = templates.Count;

            templates = templates.OrderBy(x => x.Key)
                .Skip(pager.GetStartIndex())
                .Take(pager.PageSize).ToList();

            var pagerShape = (await New.Pager(pager)).TotalItemCount(count);

            var model = new EmailTemplateIndexViewModel
            {
                Templates = templates.ConvertAll(x => new TemplateEntry { Name = x.Key, Template = x.Value }),
                Options = options,
                Pager = pagerShape
            };

            model.Options.ContentsBulkAction = new List<SelectListItem>() {
                new SelectListItem() { Text = S["Delete"], Value = nameof(ContentsBulkAction.Remove) }
            };

            return View("Index", model);
        }

        public async Task<IActionResult> Create(string returnUrl = null)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(EmailTemplateViewModel model, string submit, string returnUrl = null)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }

            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var templatesDocument = await _templatesManager.GetEmailTemplatesDocumentAsync();

                if (templatesDocument.Templates.ContainsKey(model.Name))
                {
                    ModelState.AddModelError(nameof(EmailTemplateViewModel.Name), S["A email template with the same name already exists."]);
                }
            }

            if (ModelState.IsValid)
            {
                var template = new EmailTemplate
                {
                    Description = model.Description,
                    AuthorExpression = model.AuthorExpression,
                    SenderExpression = model.SenderExpression,
                    ReplyToExpression = model.ReplyToExpression,
                    RecipientsExpression = model.RecipientsExpression,
                    SubjectExpression = model.SubjectExpression,
                    Body = model.Body,
                    IsBodyHtml = model.IsBodyHtml,
                };

                await _templatesManager.UpdateTemplateAsync(model.Name, template);

                _notifier.Success(H["The \"{0}\" email template has been created.", model.Name]);

                if (submit == "SaveAndContinue")
                {
                    return RedirectToAction(nameof(Edit), new { name = model.Name, returnUrl });
                }
                else
                {
                    return RedirectToReturnUrlOrIndex(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<IActionResult> Edit(string name, string returnUrl = null)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }

            var templatesDocument = await _templatesManager.GetEmailTemplatesDocumentAsync();

            if (!templatesDocument.Templates.ContainsKey(name))
            {
                return RedirectToAction("Create", new { name, returnUrl });
            }

            var template = templatesDocument.Templates[name];

            var model = new EmailTemplateViewModel
            {
                Name = name,
                Body = template.Body,
                Description = template.Description,
                AuthorExpression = template.AuthorExpression,
                SenderExpression = template.SenderExpression,
                ReplyToExpression = template.ReplyToExpression,
                RecipientsExpression = template.RecipientsExpression,
                SubjectExpression = template.SubjectExpression,
                IsBodyHtml = template.IsBodyHtml,
            };

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string sourceName, EmailTemplateViewModel model, string submit, string returnUrl = null)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }


            var templatesDocument = await _templatesManager.LoadEmailTemplatesDocumentAsync();

            if (ModelState.IsValid && !model.Name.Equals(sourceName, StringComparison.OrdinalIgnoreCase) && templatesDocument.Templates.ContainsKey(model.Name))
            {
                ModelState.AddModelError(nameof(EmailTemplateViewModel.Name), S["A email template with the same name already exists."]);
            }

            if (!templatesDocument.Templates.ContainsKey(sourceName))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var template = new EmailTemplate
                {
                    Description = model.Description,
                    AuthorExpression = model.AuthorExpression,
                    ReplyToExpression = model.ReplyToExpression,
                    SenderExpression = model.SenderExpression,
                    RecipientsExpression = model.RecipientsExpression,
                    SubjectExpression = model.SubjectExpression,
                    Body = model.Body,
                    IsBodyHtml = model.IsBodyHtml,
                };

                await _templatesManager.RemoveTemplateAsync(sourceName);

                await _templatesManager.UpdateTemplateAsync(model.Name, template);

                if (submit != "SaveAndContinue")
                {
                    return RedirectToReturnUrlOrIndex(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string name, string returnUrl)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }

            var templatesDocument = await _templatesManager.LoadEmailTemplatesDocumentAsync();

            if (!templatesDocument.Templates.ContainsKey(name))
            {
                return NotFound();
            }

            await _templatesManager.RemoveTemplateAsync(name);

            _notifier.Success(H["Email template deleted successfully"]);

            return RedirectToReturnUrlOrIndex(returnUrl);
        }

        [HttpPost, ActionName("Index")]
        [FormValueRequired("submit.BulkAction")]
        public async Task<ActionResult> ListPost(ContentOptions options, IEnumerable<string> itemIds)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }

            if (itemIds?.Count() > 0)
            {
                var templatesDocument = await _templatesManager.LoadEmailTemplatesDocumentAsync();
                var checkedContentItems = templatesDocument.Templates.Where(x => itemIds.Contains(x.Key));

                switch (options.BulkAction)
                {
                    case ContentsBulkAction.None:
                        break;
                    case ContentsBulkAction.Remove:
                        foreach (var item in checkedContentItems)
                        {
                            await _templatesManager.RemoveTemplateAsync(item.Key);
                        }
                        _notifier.Success(H["Email templates successfully removed."]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> SendEmail(string name)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }

            var templatesDocument = await _templatesManager.GetEmailTemplatesDocumentAsync();

            if (!templatesDocument.Templates.ContainsKey(name))
            {
                return RedirectToAction("Index");
            }

            var template = templatesDocument.Templates[name];

            var model = new SendEmailTemplateViewModel
            {
                Name = name,
                AuthorExpression = await RenderLiquid(template.AuthorExpression),
                SenderExpression = await RenderLiquid(template.SenderExpression),
                ReplyToExpression = await RenderLiquid(template.ReplyToExpression),
                RecipientsExpression = await RenderLiquid(template.RecipientsExpression),
                SubjectExpression = await RenderLiquid(template.SubjectExpression),
                Body = await RenderLiquid(template.Body),
                IsBodyHtml = template.IsBodyHtml,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(SendEmailTemplateViewModel model)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                var message = CreateMessageFromViewModel(model);

                var result = await _smtpService.SendAsync(message);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("*", error.ToString());
                    }
                }
                else
                {
                    _notifier.Success(H["Message sent successfully"]);
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        private MailMessage CreateMessageFromViewModel(SendEmailTemplateViewModel sendEmail)
        {
            var message = new MailMessage
            {
                To = sendEmail.RecipientsExpression,
                // Bcc = sendEmail.BccExpression,
                // Cc = sendEmail.Cc,
                ReplyTo = sendEmail.ReplyToExpression
            };

            var author = sendEmail.AuthorExpression;
            var sender = sendEmail.SenderExpression;

            if(!string.IsNullOrWhiteSpace(author) || ! string.IsNullOrWhiteSpace(sender))
            {
                message.From = author?.Trim() ?? sender?.Trim();
            }

            if (!String.IsNullOrWhiteSpace(sender))
            {
                message.Sender = sender;
            }

            if (!String.IsNullOrWhiteSpace(sendEmail.SubjectExpression))
            {
                message.Subject = sendEmail.SubjectExpression;
            }

            if (!String.IsNullOrWhiteSpace(sendEmail.Body))
            {
                message.Body = sendEmail.Body;
            }

             message.IsBodyHtml = sendEmail.IsBodyHtml;

            return message;
        }

        private async Task<string> RenderLiquid(string liquid)
        {
            if(!string.IsNullOrWhiteSpace(liquid))
            {
                return await _liquidTemplateManager.RenderAsync(liquid, _htmlEncoder, null, null);
            }
            return liquid;
        }

        private IActionResult RedirectToReturnUrlOrIndex(string returnUrl)
        {
            if ((String.IsNullOrEmpty(returnUrl) == false) && (Url.IsLocalUrl(returnUrl)))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
