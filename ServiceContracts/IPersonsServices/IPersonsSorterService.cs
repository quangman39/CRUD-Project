using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts.IPersonsServices
{
    /// <summary>
    /// Reperensent buniness logic for manipulating Person entity
    /// </summary>
    public interface IPersonsSorterService
    {


        /// <summary>
        /// Return sorted list of Person
        /// </summary>
        /// <param name="allPersons"> Represent list of Person to sort</param>
        /// <param name="SortBy"> Name of the property (Key),based on which the persons should be sorted </param>
        /// <param name="sortOrder"> ASC or ESC  </param>
        /// <returns> Returns sorted Person as List of Person </returns>
        Task<List<PersonReponse>> GetSortedPerson(List<PersonReponse> allPersons, string SortBy, SortOrderOptions sortOrder);


    }
}
