using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Reperensent buniness logic for manipulating Person entity
    /// </summary>
    public interface IPersonService
    {
        /// <summary>
        /// Add person in list of Person
        /// </summary>
        /// <param name="personAddRequest">A obj of person that is used to add</param>

        Task<PersonReponse> AddPerson(PersonAddRequest? personAddRequest);
        /// <summary>
        /// Get Person By personId 
        /// </summary>
        /// <param name="personId"> personId is used find person object </param>
        /// <returns>Proper person obj as PersonReponse obj</returns>
        Task<PersonReponse?> GetPersonById(Guid? personId);
        /// <summary>
        ///  get all person form list
        /// </summary>
        /// <returns> return list of person as lisf of PersonReponse object</returns>
        Task<List<PersonReponse>> GetAllPerson();

        /// <summary>
        /// Return all person obj that matches with the given search fields and search string
        /// </summary>
        /// <param name="searchBy"> Search Field to search</param>
        /// <param name="searchString"> Search String to Search</param>
        /// <returns> return all matching obj based on the given search </returns>
        Task<List<PersonReponse>> GetFilterPerson(string searchBy, string? searchString);
        /// <summary>
        /// Return sorted list of Person
        /// </summary>
        /// <param name="allPersons"> Represent list of Person to sort</param>
        /// <param name="SortBy"> Name of the property (Key),based on which the persons should be sorted </param>
        /// <param name="sortOrder"> ASC or ESC  </param>
        /// <returns> Returns sorted Person as List of Person </returns>
        Task<List<PersonReponse>> GetSortedPerson(List<PersonReponse> allPersons, string SortBy, SortOrderOptions sortOrder);

        /// <summary>
        ///Update the specified person details based on the given personId
        /// </summary>
        /// <param name="PersonId"> person obj details  to update existing obj, including person Id</param>
        /// <returns> Updated PersonReponse</returns>
        Task<PersonReponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Delete matching person is found by the given person Id 
        /// </summary>
        /// <param name="personId"> personId to find person object </param>
        /// <returns> true if successfully remove obj and fail if it fails </returns>
        Task<bool> DeletePerosn(Guid? personId);
    }
}
