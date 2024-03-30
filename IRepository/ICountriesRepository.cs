using Enities;

namespace RepositoriesContracts
{
    /// <summary>
    /// Represent data access logic for managing Person Entity
    /// </summary>
    public interface ICountriesRepository
    {

        /// <summary>
        /// Add a country object to list of countries
        /// </summary>
        /// <param name="countryAddRequest"> country object use to add</param>
        /// <returns> return country obj after adding it</returns>
        Task<Country> AddCountry(Country? country);

        /// <summary>
        /// Get all countries from the list
        /// </summary>
        /// <returns> All countries from the list </returns>
        Task<List<Country>> GetAll();

        /// <summary>
        /// Get correctponding country obj by Id
        /// </summary>
        /// <param name="id"> (Guid) id to search</param>
        /// <returns> matching country object</returns>
        Task<Country?> GetByID(Guid? id);

        /// <summary>
        /// Get macching coutry based on country Name
        /// </summary>
        /// <param name="id"> country Name is used to Add</param>
        /// <returns> obj Country </returns>
        Task<Country?> GetByCountryName(string? countryName);

    }
}
