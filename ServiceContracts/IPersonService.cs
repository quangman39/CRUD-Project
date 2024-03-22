using ServiceContracts.DTO;

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

        PersonReponse AddPerson(PersonAddRequest? personAddRequest);
        /// <summary>
        /// Get Person By personId 
        /// </summary>
        /// <param name="personId"> personId is used find person object </param>
        /// <returns>Proper person obj as PersonReponse obj</returns>
        PersonReponse? GetPersonById(Guid? personId);
        /// <summary>
        ///  get all person form list
        /// </summary>
        /// <returns> return list of person as lisf of PersonReponse object</returns>
        List<PersonReponse> GetAllPerson();

        /// <summary>
        /// Return all person obj that matches with the given search fields and search string
        /// </summary>
        /// <param name="searchBy"> Search Field to search</param>
        /// <param name="searchString"> Search String to Search</param>
        /// <returns> return all matching obj based on the given search </returns>
        List<PersonReponse> GetFilterPerson(string searchBy, string? searchString);
    }
}
