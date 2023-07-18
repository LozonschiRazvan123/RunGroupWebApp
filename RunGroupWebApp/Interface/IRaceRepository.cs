using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interface
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetRaces();
        Task<Race> GetRacesByIdAsync(int id);
        Task<Race> GetRacesByIdAsyncNoTracking(int id);
        Task<IEnumerable<Race>> GetRaceByCity(string city);
        bool Add(Race race);
        bool Update(Race race);
        bool Delete(Race race);
        bool Save();
    }
}
