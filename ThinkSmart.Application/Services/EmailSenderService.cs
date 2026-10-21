using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        public EmailSettings _emailSettings { get; }
        private readonly ILogger<EmailSenderService> _logger;
        public EmailSenderService(IOptions<EmailSettings> emailSettings, ILogger<EmailSenderService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmail(string txtfileName)
        {
            try
            {
                if (_emailSettings.EnableEmailNotifications)
                {
                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse(_emailSettings.FromEmail));
                    string address = _emailSettings.ToEmail;

                    //foreach (var address in _emailSettings.ToEmail.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    //{
                        email.To.Add(MailboxAddress.Parse(address));
                   // }
                    //email.To.Add(MailboxAddress.Parse(_emailSettings.ToEmail));
                    email.Subject = "HSKASPエラーログ";

                    string template = Path.GetTempPath();
                    string template1 = template + "\\" + txtfileName;
                    string length = template1.Length.ToString();
                    var errorText = File.ReadAllText(template1.Trim());
                    errorText = errorText.ToString();

                    email.Body = new TextPart(TextFormat.Plain) { Text = errorText }; 
                    // send email
                    using var smtp = new SmtpClient();
                    smtp.Connect(_emailSettings.PrimaryDomain, 587, SecureSocketOptions.Auto);
                    smtp.Authenticate(_emailSettings.FromEmail, _emailSettings.UsernamePassword);
                    string result = smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
