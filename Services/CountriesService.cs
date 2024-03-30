using Enities;
using RepositoriesContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository _countriesRepository;
        public CountriesService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<CountryReponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            //check property of countryAddRequest
            if (countryAddRequest == null) throw new ArgumentNullException(nameof(countryAddRequest));
            if (string.IsNullOrEmpty(countryAddRequest.CountryName)) throw new ArgumentException(nameof(countryAddRequest.CountryName));

            //validation: Country cant be duplicate


            if (_countriesRepository.GetByCountryName(countryAddRequest.CountryName) != null)
                throw new ArgumentException("Country name already exists.", nameof(countryAddRequest.CountryName));


            //convert obj from countryAddRequest to Country 
            Country country = countryAddRequest.ToCountry();

            //Generate Country Id and add in list
            country.CountryId = Guid.NewGuid();

            await _countriesRepository.AddCountry(country);
            //covert to countryReponse

            return country.ToCountryReponse();
        }

        public async Task<List<CountryReponse>> GetAll()
        {
            return (await _countriesRepository.GetAll()).Select(temp => temp.ToCountryReponse()).ToList();

        }

        public async Task<CountryReponse?> GetByID(Guid? id)
        {
            if (id == null) return null;
            Country? country = await _countriesRepository.GetByID(id);
            if (country == null) return null;

            return country.ToCountryReponse();
        }
    }
}
