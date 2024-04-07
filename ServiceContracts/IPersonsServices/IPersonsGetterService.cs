using ServiceContracts.DTO;

namespace ServiceContracts.IPersonsServices
{
    /// <summary>
    /// Reperensent buniness logic for manipulating Person entity
    /// </summary>
    public interface IPersonsGetterService
    {
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


    }
}
