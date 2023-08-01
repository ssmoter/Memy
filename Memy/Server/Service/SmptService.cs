using MailKit.Net.Smtp;

using Memy.Server.Data;

using MimeKit;

namespace Memy.Server.Service
{
    public class SmtpService
    {
        private readonly IConfiguration _configuration;

        public SmtpService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string to, string subject, string body)
        {
            try
            {
                var section = _configuration.GetSection("EmailSender");
                var emailSender = section.Get<EmailSender>();

                ArgumentNullException.ThrowIfNull(emailSender);
                
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(emailSender.Email));
                email.To.Add(MailboxAddress.Parse(to));

                email.Subject=subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();
                smtp.Connect(emailSender.Host, emailSender.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(emailSender.Email, emailSender.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private class EmailSender
        {
            public string Host { get; set; } = "";
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
            public int Port { get; set; }
        }

    }
}
