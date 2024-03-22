using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represent buniness logic for manipulating Country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Add a country object to list of countries
        /// </summary>
        /// <param name="countryAddRequest"> country object use to add</param>
        /// <returns> return country obj after adding it(include generated country id)</returns>
       CountryReponse AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Get all countries from the list
        /// </summary>
        /// <returns> All countries from the list as list of CountryReponse </returns>
       List<CountryReponse> GetAll();

        /// <summary>
        /// Get correctponding country obj by Id
        /// </summary>
        /// <param name="id"> (Guid) id to search</param>
        /// <returns> matching counrtry as country object</returns>
        CountryReponse? GetByID(Guid? id);

        
    }
}
