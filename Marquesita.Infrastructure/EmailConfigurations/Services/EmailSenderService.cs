using MailKit.Net.Smtp;
using Marquesita.Infrastructure.EmailConfigurations.Interfaces;
using Marquesita.Infrastructure.EmailConfigurations.Models;
using MimeKit;
using System.Linq;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.EmailConfigurations.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IEmailsTextService _emailText;

        public EmailSenderService(EmailConfiguration emailConfig, IEmailsTextService emailText)
        {
            _emailConfig = emailConfig;
            _emailText = emailText;
        }

        public async Task SendEmailConfirmationAsync(Message message)
        {
            var mailMessage = CreateEmailConfirmationMessage(message);

            await SendAsync(mailMessage);
        }

        private MimeMessage CreateEmailConfirmationMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody =  _emailText.ConfirmMailTextEcommerce(message)};
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        public async Task SendEmailConfirmationShopAsync(Message message)
        {
            var mailMessage = CreateEmailConfirmationMessageShop(message);
            await SendAsync(mailMessage);
        }

        private MimeMessage CreateEmailConfirmationMessageShop(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = _emailText.ConfirmMailTextShop(message) };
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        public async Task SendRecoveryPasswordEmailAsync(Message message)
        {
            var mailMessage = CreateRecoveryPasswordEmailMessage(message);
            await SendAsync(mailMessage);
        }

        private MimeMessage CreateRecoveryPasswordEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = _emailText.RecoveryPasswordText(message) };
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        public async Task SendEmailSaleConfirmationAsync(Message message)
        {
            var mailMessage = CreateEmailSaleConfirmationMessage(message);

            await SendAsync(mailMessage);
        }

        private MimeMessage CreateEmailSaleConfirmationMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = _emailText.SaleConfirmationText(message) };

            if (message.Attachments != null && message.Attachments.Any())
            {
                bodyBuilder.Attachments.Add("Confirmación de compra", message.Attachments, ContentType.Parse("application/pdf"));
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                await client.SendAsync(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception, or both.
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
