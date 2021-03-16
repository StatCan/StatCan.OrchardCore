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
using OrchardCore;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
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
        private readonly IContentManager _contentManager;
        private readonly IEmailTemplatesService _emailTemplatesService;

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
            HtmlEncoder htmlEncoder,
            IContentManager contentManager,
            IEmailTemplatesService emailTemplatesService
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
            _contentManager = contentManager;
            _emailTemplatesService = emailTemplatesService;
        }

        public async Task<IActionResult> Index(ViewModels.ContentOptions options, PagerParameters pagerParameters)
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
                templates = templates.Where(x => x.Value.Name.ToLower().Contains(options.Search.ToLower())).ToList();
            }

            var count = templates.Count;

            templates = templates.OrderBy(x => x.Value.Name)
                .Skip(pager.GetStartIndex())
                .Take(pager.PageSize).ToList();

            var pagerShape = (await New.Pager(pager)).TotalItemCount(count);

            var model = new EmailTemplateIndexViewModel
            {
                Templates = templates.ConvertAll(x => new TemplateEntry { Id = x.Key, Template = x.Value }),
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

            var templatesDocument = await _templatesManager.GetEmailTemplatesDocumentAsync();

            if (templatesDocument.Templates.Values.Any(x => x.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(nameof(EmailTemplateViewModel.Name), S["An email template with the same name already exists."]);
            }

            if (ModelState.IsValid)
            {
                var id = IdGenerator.GenerateId();

                var template = new EmailTemplate
                {
                    Name = model.Name,
                    Description = model.Description,
                    AuthorExpression = model.AuthorExpression,
                    SenderExpression = model.SenderExpression,
                    ReplyToExpression = model.ReplyToExpression,
                    RecipientsExpression = model.RecipientsExpression,
                    CCExpression = model.CCExpression,
                    BCCExpression = model.BCCExpression,
                    SubjectExpression = model.SubjectExpression,
                    Body = model.Body,
                    IsBodyHtml = model.IsBodyHtml,
                };

                await _templatesManager.UpdateTemplateAsync(id, template);

                _notifier.Success(H["The \"{0}\" email template has been created.", model.Name]);

                if (submit == "SaveAndContinue")
                {
                    return RedirectToAction(nameof(Edit), new { id, returnUrl });
                }
                else
                {
                    return RedirectToReturnUrlOrIndex(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<IActionResult> Edit(string id, string returnUrl = null)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }

            var templatesDocument = await _templatesManager.GetEmailTemplatesDocumentAsync();

            if (!templatesDocument.Templates.ContainsKey(id))
            {
                return RedirectToAction("Create", new { id, returnUrl });
            }

            var template = templatesDocument.Templates[id];

            var model = new EmailTemplateViewModel
            {
                Id = id,
                Name = template.Name,
                Body = template.Body,
                Description = template.Description,
                AuthorExpression = template.AuthorExpression,
                SenderExpression = template.SenderExpression,
                ReplyToExpression = template.ReplyToExpression,
                RecipientsExpression = template.RecipientsExpression,
                CCExpression = template.CCExpression,
                BCCExpression = template.BCCExpression,
                SubjectExpression = template.SubjectExpression,
                IsBodyHtml = template.IsBodyHtml,
            };

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmailTemplateViewModel model, string submit, string returnUrl = null)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }

            var templatesDocument = await _templatesManager.LoadEmailTemplatesDocumentAsync();

            if (ModelState.IsValid && templatesDocument.Templates.Any(x => x.Key != model.Id && x.Value.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(nameof(EmailTemplateViewModel.Name), S["An email template with the same name already exists."]);
            }

            if (!templatesDocument.Templates.ContainsKey(model.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var template = new EmailTemplate
                {
                    Name = model.Name,
                    Description = model.Description,
                    AuthorExpression = model.AuthorExpression,
                    ReplyToExpression = model.ReplyToExpression,
                    SenderExpression = model.SenderExpression,
                    RecipientsExpression = model.RecipientsExpression,
                    CCExpression = model.CCExpression,
                    BCCExpression = model.BCCExpression,
                    SubjectExpression = model.SubjectExpression,
                    Body = model.Body,
                    IsBodyHtml = model.IsBodyHtml,
                };

                await _templatesManager.RemoveTemplateAsync(model.Id);

                await _templatesManager.UpdateTemplateAsync(model.Id, template);

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
        public async Task<IActionResult> Delete(string id, string returnUrl)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }

            var templatesDocument = await _templatesManager.LoadEmailTemplatesDocumentAsync();

            if (!templatesDocument.Templates.ContainsKey(id))
            {
                return NotFound();
            }

            await _templatesManager.RemoveTemplateAsync(id);

            _notifier.Success(H["Email template deleted successfully"]);

            return RedirectToReturnUrlOrIndex(returnUrl);
        }

        [HttpPost, ActionName("Index")]
        [FormValueRequired("submit.BulkAction")]
        public async Task<ActionResult> ListPost(ViewModels.ContentOptions options, IEnumerable<string> itemIds)
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
        public async Task<IActionResult> SendEmail(string id, string contentItemId, string returnUrl)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }

            var templatesDocument = await _templatesManager.GetEmailTemplatesDocumentAsync();

            if (!templatesDocument.Templates.ContainsKey(id))
            {
                return Redirect(returnUrl);
            }

            var template = templatesDocument.Templates[id];
            var contentItem = await _contentManager.GetAsync(contentItemId);

            var model = new SendEmailTemplateViewModel
            {
                Name = template.Name,
                Author = await _emailTemplatesService.RenderLiquid(template.AuthorExpression, contentItem),
                Sender = await _emailTemplatesService.RenderLiquid(template.SenderExpression, contentItem),
                ReplyTo = await _emailTemplatesService.RenderLiquid(template.ReplyToExpression, contentItem),
                Recipients = await _emailTemplatesService.RenderLiquid(template.RecipientsExpression, contentItem),
                CC = await _emailTemplatesService.RenderLiquid(template.CCExpression, contentItem),
                BCC = await _emailTemplatesService.RenderLiquid(template.BCCExpression, contentItem),
                Subject = await _emailTemplatesService.RenderLiquid(template.SubjectExpression, contentItem),
                Body = await _emailTemplatesService.RenderLiquid(template.Body, contentItem),
                IsBodyHtml = template.IsBodyHtml,
            };

            ViewData["returnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(SendEmailTemplateViewModel model, string returnUrl)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageEmailTemplates))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                var message = _emailTemplatesService.CreateMessageFromViewModel(model);

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
                    return Redirect(returnUrl);
                }
            }

            return View(model);
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
