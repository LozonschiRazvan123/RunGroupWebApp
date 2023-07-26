using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interface;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext _context;

        public RaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Race race)
        {
            _context.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetRaceByCity(string city)
        {
            return await _context.Races.Include(a=>a.Address).Where(c => c.Address.City == city).ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetRaces()
        {
            return await _context.Races.ToListAsync();
        }

        public async Task<Race> GetRacesByIdAsync(int id)
        {
            return await _context.Races.Include(a=>a.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Race> GetRacesByIdAsyncNoTracking(int id)
        {
            return await _context.Races.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Race>> GetRacesByUser(string id)
        {
            return await _context.Races.Where(r => r.AppUserId == id).ToListAsync();
        }

        public bool Save()
        {
            var changes = _context.SaveChanges();
            return changes > 0 ? true : false;
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }
    }
}
