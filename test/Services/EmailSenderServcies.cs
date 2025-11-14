using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
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
        private readonly IOptions<SendGridOptions> _options;
        public EmailSenderServcies(IOptions<SendGridOptions> options)
        {
            _options = options;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var apiKey = _options.Value.ApiKey;
            var client = new SendGridClient(apiKey);

            // This email MUST be the one you just verified with SendGrid
            var from = new EmailAddress("ahmedhossamahmed22@gmail.com", "HappyPaws Haven");

            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);

            await client.SendEmailAsync(msg);
        }
    }
}
