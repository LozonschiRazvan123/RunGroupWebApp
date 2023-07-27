namespace RunGroupWebApp.ViewModels
{
    public class SendEmailRequest
    {
        public string EmailTo { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
