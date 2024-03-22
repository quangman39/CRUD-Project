using Enities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
        }

        public CountryReponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //check property of countryAddRequest
            if (countryAddRequest == null) throw new ArgumentNullException(nameof(countryAddRequest));
            if (string.IsNullOrEmpty(countryAddRequest.CountryName)) throw new ArgumentException(nameof(countryAddRequest.CountryName));

            //validation: Country cant be duplicate
            

            if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException(nameof(CountryAddRequest.CountryName));
            }


            //convert obj from countryAddRequest to Country 
            Country country = countryAddRequest.ToCountry();

            //Generate Country Id and add in list
            country.CountryId = Guid.NewGuid();
            _countries.Add(country);

            //covert to countryReponse

            return country.ToCountryReponse();

        }

        public List<CountryReponse> GetAll()
        {
            return _countries.Select(temp => temp.ToCountryReponse()).ToList();

        }

        public CountryReponse? GetByID(Guid? id)
        {
            if (id == null) return null;
            Country? country = _countries.FirstOrDefault(temp => temp.CountryId == id);
            if (country == null) return null;

            return country.ToCountryReponse();
        }
    }
}
