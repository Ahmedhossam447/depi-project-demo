using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;

namespace test.Services
{
    public class EmailSenderServcies : IEmailSender
    {
        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            // This email MUST be the one you just verified with SendGrid
            var from = new EmailAddress("ahmedhossamahmed22@gmail.com", "ahmed hossam");

            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);

            await client.SendEmailAsync(msg);
        }
    }
}
