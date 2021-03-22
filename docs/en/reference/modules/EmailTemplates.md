# EmailTemplates (`StatCan.OrchardCore.EmailTemplates`)

The EmailTemplates module aims to simplify and accelerate the process of sending emails by using templates.

## EmailTemplate

The EmailTemplate feature allow you do define reusable email templates. The fields of the templates support the Liquid language, which allows you to create dynamic emails for any content.

## EmailTemplate Document

| Field | Definition |
|-------|------------|
| Templates | Dictionnary of email templates. |
| Name | Name of the email template. |
| Description | Description of the email template. |
| Author | The author's email address that defines who the email is from.  With Liquid support. |
| Sender | The sender's email address. With Liquid support. |
| ReplyTo | The "reply to" email address. With Liquid support. |
| Recipients | The comma-separated list of recipient email addresses. With Liquid support. |
| CC | The comma-separated list of CC recipient email addresses. With Liquid support. |
| BCC | The comma-separated list of BCC recipient email addresses. With Liquid support. |
| Subject | The subject of the email message. With Liquid support. |
| Body | The body of the email message. With Liquid support. |
| IsBodyHtml | If checked, indicates the body of the email message is HTML. If unchecked, indicates the body of the email message is plain text. |

## EmailTemplate Part

The EmailTemplate Part allows you to link one or multiple email templates to any content type. Any instanciated content item from that content type will then act as the model for the email template.

## EmailTemplate Part Settings

| Field | Definition |
|-------|------------|
| SelectedEmailTemplates | Array of templates ids that are currently linked to the content type. |

## Summary Admin Integration

Every content item with the Email Template part attached to its content type will be displayed with a dropdown of selected email templates. This will redirect you to the Send screen and will render the email using the selected content item as the model of the template.

## Workflow Intergration

You will need to select an email template and provide a JavaScript expression that returns an object. This object item will be used as the model of the template. By default, the field is assigned the expression `workflow().input["ContentItem"]` which returns the content item from the content created event. You can change it if needed.

Please see the [workflow](../Workflows.md#emailtemplates-statcanorchardcoreemailtemplates) documentation.
