using Enities;
using System.Linq.Expressions;

namespace RepositoriesContracts
{
    /// <summary>
    /// Repersent data access logic for managing Person ID
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// Adds a person obj to data store
        /// </summary>
        /// <param name="person"> person object to add</param>
        /// <returns> person obj after add it to table </returns>
        Task<Person> AddPerson(Person person);


        /// <summary>
        /// Return list of existing person in table 
        /// </summary>
        /// <returns> return list person </returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// Return matching person base on the given id
        /// </summary>
        /// <param name="personId"> the given personId used to search</param>
        /// <returns> Matching person obj </returns>
        Task<Person?> GetPersonById(Guid? personId);

        /// <summary>
        /// Return all object based on the given expression
        /// </summary>
        /// <param name="predicate"> Linq Expression to check </param>
        /// <returns> All matching Persons with given condition  </returns>
        Task<List<Person>> GetFilterPersons(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Deletes a person object based on the person Id
        /// </summary>
        /// <param name="personId"> PersonId is used to search </param>
        /// <returns> Return true, if the deletion is successfully  otherwise false </returns>
        Task<bool> DeletePersonByPersonID(Guid? personId);

        /// <summary>
        /// Update a person object (name or other details) based on the given person id
        /// </summary>
        /// <param name="person"> person obj is used to Update</param>
        /// <returns>Return the updated person obj</returns>
        Task<Person> UpdatePerson(Person person);
    }
}
