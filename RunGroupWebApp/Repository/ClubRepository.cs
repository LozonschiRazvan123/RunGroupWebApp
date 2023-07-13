using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interface;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext _context;

        public ClubRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Club club)
        {
            _context.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _context.Clubs.Include(a=>a.Address).Where(c=>c.Address.City==city).ToListAsync();
        }

        public async Task<Club> GetClubByIdAsync(int id)
        {
            return await _context.Clubs.Include(a=> a.Address).FirstOrDefaultAsync(c=> c.Id==id);
        }

        public async Task<IEnumerable<Club>> GetClubs()
        {
            return await _context.Clubs.ToListAsync();
        }

        public bool Save()
        {
            var changes = _context.SaveChanges();
            return changes >0 ? true : false;
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save();
        }
    }
}
