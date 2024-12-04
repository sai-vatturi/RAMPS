using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using RecipeMeal.Core.Interfaces;

namespace RecipeMeal.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["SenderEmail"]));
            email.To.Add(new MailboxAddress(to, to));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body // Set the email body as HTML
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                // Connect with StartTls for port 587
                await smtp.ConnectAsync(
                    emailSettings["SmtpServer"],
                    int.Parse(emailSettings["Port"]),
                    SecureSocketOptions.StartTls
                );

                // Authenticate with SMTP server
                await smtp.AuthenticateAsync(emailSettings["Username"], emailSettings["Password"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw; // Re-throw the exception for higher-level handling
            }
        }
    }
}
