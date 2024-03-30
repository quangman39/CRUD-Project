using Enities;
using Microsoft.EntityFrameworkCore;
using RepositoriesContracts;

namespace Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDbContext _db;
        public CountriesRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Country> AddCountry(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAll()
        {
            return await _db.Countries.ToListAsync();
        }

        public async Task<Country?> GetByID(Guid? id)
        {
            return await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryId == id);
        }
        public async Task<Country?> GetByCountryName(string? countryName)
        {
            return await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryName == countryName);
        }
    }
}
