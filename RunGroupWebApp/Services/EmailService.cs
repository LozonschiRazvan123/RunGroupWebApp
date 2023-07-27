using RunGroupWebApp.Interface;
using RunGroupWebApp.ViewModels;
using SendGrid;
using SendGrid.Helpers.Mail;

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
            var apiKey = _configuration.GetSection("EmailAPIKey").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_configuration.GetSection("EmailUsername").Value);
            var subject = request.Subject;
            var to = new EmailAddress(request.EmailTo);
            var plainTextContent = request.Body;
            var htmlContent = $"<strong>{request.Body}</strong>";
            var msg = MailHelper.CreateSingleEmail(from,to,subject,plainTextContent,htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
