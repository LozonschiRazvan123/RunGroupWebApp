using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interface;
using RunGroupWebApp.Services;
using RunGroupWebApp.ViewModels;

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
        public IActionResult SendEmail(SendEmailRequest request)
        {
            _emailService.SendEmailAsync(request);
            return Ok();
        }

        [HttpPost]
        [Route("inviteEmail/{name}")]
        public async Task<IActionResult> GenerateInviteMessage([FromBody] SendEmailRequest request, string name)
        {
            var senderName = name;
            var message = "Hi there! I would like to invite you to join HabitTracker, an amazing application that helps you develop and track your habits. With HabitTracker, you can set goals, track your progress, and stay motivated along the way. To get started, simply click on the following link ";
            var link = $"{request.Body}/sign-up";
            var Body = $"<h1>Invite to HabitTracker from {senderName}</h1>" +
                                           $"Message: {message} <a href='{link}'>here</a> .";

            var emailRequest = new SendEmailRequest
            {
                EmailTo = request.EmailTo,
                Subject = "HabitTracker - Invite user",
                Body = Body
            };

            await _emailService.SendEmailAsync(emailRequest);
            return Ok();

        }
    }
}
