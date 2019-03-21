using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CodedenimWebApp.Service
{
    public class EmailSender 
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress($"noreply@Codedenim.com", "Codedenim");
            var to = new EmailAddress(email, email);
            var plainTextContent = message;
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            try
            {
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //return Task.CompletedTask;
        }
    }
}