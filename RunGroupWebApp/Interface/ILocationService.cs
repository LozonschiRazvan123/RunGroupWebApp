using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interface
{
    public interface ILocationService
    {
        Task<List<City>> GetLocationSearch(string location);
        Task<City> GetCityByZipCode(int zipCode);
    }
}
