using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Interface
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(SendEmailRequest request);
    }
}
