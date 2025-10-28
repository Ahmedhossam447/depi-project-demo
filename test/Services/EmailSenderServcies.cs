using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;
using test.Helpers;

namespace test.Services
{
    public class EmailSenderServcies : IEmailSender
    {
        private readonly SendGridOptions _options;
        public EmailSenderServcies(SendGridOptions options)
        {
            _options = options;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var apiKey = _options.ApiKey;
            var client = new SendGridClient(apiKey);

            // This email MUST be the one you just verified with SendGrid
            var from = new EmailAddress("ahmedhossamahmed22@gmail.com", "ahmed hossam");

            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);

            await client.SendEmailAsync(msg);
        }
    }
}
