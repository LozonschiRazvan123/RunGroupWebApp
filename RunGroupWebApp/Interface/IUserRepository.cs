using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetUserId(string id);
        bool Add(AppUser user);
        bool Deleted(AppUser user);
        bool Update(AppUser user);
        bool Save();
    }
}
