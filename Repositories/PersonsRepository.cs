using Enities;
using Microsoft.EntityFrameworkCore;
using RepositoriesContracts;
using System.Linq.Expressions;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _db;
        public PersonsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Person> AddPerson(Person person)
        {

            _db.Add(person);
            await _db.SaveChangesAsync();
            return person;

        }

        public async Task<bool> DeletePersonByPersonID(Guid? personId)
        {
            _db.Persons.RemoveRange(_db.Persons.Where(temp => temp.PersonID == personId));
            int rowDeleted = await _db.SaveChangesAsync();
            return rowDeleted > 0;

        }

        public async Task<List<Person>> GetAllPersons()
        {
            return await _db.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilterPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _db.Persons.Include("Country").Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonById(Guid? personId)
        {
            return await _db.Persons.Include("Country").FirstOrDefaultAsync(temp => temp.PersonID == personId);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(tmp => tmp.PersonID == person.PersonID);

            if (matchingPerson == null) { return matchingPerson; }

            matchingPerson.PersonName = person.PersonName;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.Address = person.Address;
            matchingPerson.Gender = person.Gender;
            matchingPerson.Email = person.Email;
            matchingPerson.CountryId = person.CountryId;
            matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;
            int counUpdated = await _db.SaveChangesAsync();

            return matchingPerson;
        }
    }
}
