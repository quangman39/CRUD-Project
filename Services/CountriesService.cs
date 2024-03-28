using Enities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ApplicationDbContext _db;
        public CountriesService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<CountryReponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            //check property of countryAddRequest
            if (countryAddRequest == null) throw new ArgumentNullException(nameof(countryAddRequest));
            if (string.IsNullOrEmpty(countryAddRequest.CountryName)) throw new ArgumentException(nameof(countryAddRequest.CountryName));

            //validation: Country cant be duplicate


            if (_db.Countries.Any(c => c.CountryName == countryAddRequest.CountryName))
                throw new ArgumentException("Country name already exists.", nameof(countryAddRequest.CountryName));


            //convert obj from countryAddRequest to Country 
            Country country = countryAddRequest.ToCountry();

            //Generate Country Id and add in list
            country.CountryId = Guid.NewGuid();
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            //covert to countryReponse

            return country.ToCountryReponse();
        }

        public async Task<List<CountryReponse>> GetAll()
        {
            return await _db.Countries.Select(temp => temp.ToCountryReponse()).ToListAsync();

        }

        public async Task<CountryReponse?> GetByID(Guid? id)
        {
            if (id == null) return null;
            Country? country = await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryId == id);
            if (country == null) return null;

            return country.ToCountryReponse();
        }
    }
}
