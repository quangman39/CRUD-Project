using ServiceContracts.DTO;

namespace ServiceContracts.IPersonsServices
{
    /// <summary>
    /// Reperensent buniness logic for manipulating Person entity
    /// </summary>
    public interface IPersonsAdderService
    {
        /// <summary>
        /// Add person in list of Person
        /// </summary>
        /// <param name="personAddRequest">A obj of person that is used to add</param>
        Task<PersonReponse> AddPerson(PersonAddRequest? personAddRequest);
    }
}
