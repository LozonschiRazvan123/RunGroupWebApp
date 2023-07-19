using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interface
{
    public interface IDashboardRepository
    {
        Task<List<Race>> GetAllUserRaces();
        Task<List<Club>> GetAllUserClubs();
    }
}
