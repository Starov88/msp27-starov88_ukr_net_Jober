using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jober.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<EmailSenderOptions> options)
        {
            Options = options.Value;
        }

        public EmailSenderOptions Options { get; } //set only via Secret Manager

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", Options.User));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(Options.Host, Options.Port, true);
                await client.AuthenticateAsync(Options.User, Options.Key);
                try
                {
                    await client.SendAsync(emailMessage);
                }
                catch (SmtpCommandException ex)
                {

                }
                await client.DisconnectAsync(true);
            }
        }

        public async Task SendEmailsAsync(IList<string> emails, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", Options.User));
            if(emails == null || emails.Count == 0)
            {
                return;
            }
            foreach (var item in emails)
            {
                emailMessage.To.Add(new MailboxAddress("", item));
            }
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(Options.Host, Options.Port, true);
                await client.AuthenticateAsync(Options.User, Options.Key);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

    }
}
