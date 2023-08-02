using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using RunGroupWebApp.Interface;
using RunGroupWebApp.Services;
using RunGroupWebApp.ViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace RunGroupWebApp.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailRepository _emailService;

        public EmailController(IEmailRepository emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        [Route("email")]
        public async Task<IActionResult> SendEmail([FromForm]SendEmailRequest request)
        {
            await _emailService.SendEmailAsync(request);
            return Ok();
        }
        public IActionResult GenerateInviteMessage()
        {
            var response = new SendEmailRequest();
            return View();
        }

        [HttpPost]
        [Route("inviteEmail/{name}")]
        public async Task<IActionResult> GenerateInviteMessage([FromBody] SendEmailRequest request, string name)
        {
            var senderName = name;
            var message = "Hi there! I would like to invite you to join HabitTracker, an amazing application that helps you develop and track your habits. With HabitTracker, you can set goals, track your progress, and stay motivated along the way. To get started, simply click on the following link ";
            var Body = $"<h1>Invite to HabitTracker from {senderName}</h1>" +
                                           $"Message: {message} ";

            var emailRequest = new SendEmailRequest
            {
                EmailTo = request.EmailTo,
                Subject = "HabitTracker - Invite user",
                Body = Body
            };

            await _emailService.SendEmailAsync(emailRequest);
            /*var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("olga.jenkins@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("olga.jenkins@ethereal.email"));
            email.Subject = "Invite to the application";
            email.Body = new TextPart(TextFormat.Html) { Text = "Hi welcome to here" };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email.", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("olga.jenkins@ethereal.email", "3VtBUZcKQSVec3KHBM");
            smtp.Send(email);
            smtp.Disconnect(true);*/
            return Ok();

        }
    }
}
