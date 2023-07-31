using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using RunGroupWebApp.Interface;
using RunGroupWebApp.ViewModels;
using SendGrid;
using SendGrid.Helpers.Mail;
using MailKit.Net.Smtp;

namespace RunGroupWebApp.Services
{
    public class EmailService : IEmailRepository
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(SendEmailRequest request)
        {
            /*var apiKey = _configuration.GetSection("EmailAPIKey").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("rungroup@test.com","RunGroup");
            var subject = request.Subject;
            var to = new EmailAddress(request.EmailTo);
            var plainTextContent = request.Body;
            var htmlContent = $"<strong>{request.Body}</strong>";
            var msg = MailHelper.CreateSingleEmail(from,to,subject,plainTextContent,htmlContent);
            var response = await client.SendEmailAsync(msg);*/
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("kyla.stanton26@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(request.EmailTo));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email.", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("kyla.stanton26@ethereal.email", "64SsV7RAGeCyJrFHrK");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
