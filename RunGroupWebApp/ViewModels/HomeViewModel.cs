using RunGroupWebApp.Models;

namespace RunGroupWebApp.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Club> Clubs { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public HomeUserCreativeModel Register {  get; set; } = new HomeUserCreativeModel();
    }
}
