using ServiceContracts.DTO;

namespace ServiceContracts.IPersonsServices
{
    /// <summary>
    /// Reperensent buniness logic for manipulating Person entity
    /// </summary>
    public interface IPersonsUpdateService
    {
        /// <summary>
        ///Update the specified person details based on the given personId
        /// </summary>
        /// <param name="PersonId"> person obj details  to update existing obj, including person Id</param>
        /// <returns> Updated PersonReponse</returns>
        Task<PersonReponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
    }
}
