using Enities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly ApplicationDbContext _db;
        public PersonService(ApplicationDbContext db)
        {
            _db = db;


        }
        public async Task<PersonReponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if personAdd is null
            if (personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));


            //Validator PersonAddRequest
            ValidationHelper.ModelValition(personAddRequest);


            //Convert, generator id and  add it into list
            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();
            await _db.Persons.AddAsync(person);
            await _db.SaveChangesAsync();

            //convert to Person Reponse
            PersonReponse personReponse = person.ToPersonReoponse();

            //Populate Country into Person and return it
            return personReponse;
        }

        public async Task<PersonReponse>? GetPersonById(Guid? personId)
        {
            //check if personId is null
            if (personId == null) return null;

            Person? person = await _db.Persons.Include("Country").FirstOrDefaultAsync(temp => temp.PersonID == personId);

            // check if person is null 
            if (person == null) return null;

            return person.ToPersonReoponse();
        }

        public async Task<List<PersonReponse>> GetAllPerson()
        {
            return await _db.Persons.Include("Country").Select(u => u.ToPersonReoponse()).ToListAsync();
        }

        public async Task<List<PersonReponse>> GetFilterPerson(string searchBy, string? searchString)
        {
            List<PersonReponse> allPersons = await GetAllPerson();
            List<PersonReponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(searchBy))
            {
                return matchingPersons;

            }
            switch (searchBy)
            {
                case nameof(PersonReponse.PersonName):
                    matchingPersons = allPersons.Where(temp =>
                    string.IsNullOrEmpty(temp.PersonName) ? true :
                    temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonReponse.Email):
                    matchingPersons = allPersons.Where(temp =>
                    string.IsNullOrEmpty(temp.Email) ? true :
                    temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonReponse.Country):
                    matchingPersons = allPersons.Where(temp =>
                    string.IsNullOrEmpty(temp.Country) ? true :
                    temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonReponse.Gender):
                    matchingPersons = allPersons.Where(temp =>
                    string.IsNullOrEmpty(temp.Gender) ? true :
                    temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonReponse.DateOfBirth):
                    matchingPersons = allPersons.Where(temp =>
                    (temp.DateOfBirth == null) ? true :
                    temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonReponse.Address):
                    matchingPersons = allPersons.Where(temp =>
                    string.IsNullOrEmpty(temp.Address) ? true :
                    temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                default:
                    matchingPersons = allPersons;
                    break;
            }

            return matchingPersons;
        }
        public async Task<List<PersonReponse>> GetSortedPerson(List<PersonReponse> allPersons, string SortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(SortBy)) return allPersons;

            List<PersonReponse> sortedPersons = (SortBy, sortOrder) switch
            {
                (nameof(PersonReponse.PersonName), SortOrderOptions.ASC)
                => allPersons.OrderBy(temp => temp.PersonName).ToList(),
                (nameof(PersonReponse.PersonName), SortOrderOptions.DESC)
               => allPersons.OrderByDescending(temp => temp.PersonName).ToList(),

                (nameof(PersonReponse.Email), SortOrderOptions.ASC)
                => allPersons.OrderBy(temp => temp.Email).ToList(),
                (nameof(PersonReponse.Email), SortOrderOptions.DESC)
               => allPersons.OrderByDescending(temp => temp.Email).ToList(),

                (nameof(PersonReponse.Address), SortOrderOptions.ASC)
                => allPersons.OrderBy(temp => temp.Address).ToList(),
                (nameof(PersonReponse.Address), SortOrderOptions.DESC)
               => allPersons.OrderByDescending(temp => temp.Address).ToList(),

                (nameof(PersonReponse.DateOfBirth), SortOrderOptions.ASC)
               => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),
                (nameof(PersonReponse.DateOfBirth), SortOrderOptions.DESC)
               => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),


                (nameof(PersonReponse.Age), SortOrderOptions.ASC)
                => allPersons.OrderBy(temp => temp.Age).ToList(),
                (nameof(PersonReponse.Age), SortOrderOptions.DESC)
               => allPersons.OrderByDescending(temp => temp.Age).ToList(),
            };

            return sortedPersons;
        }

        public async Task<PersonReponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) throw new ArgumentNullException("arg cant be null");

            //Validate property in personUpdateRequest
            ValidationHelper.ModelValition(personUpdateRequest);

            //find person 
            Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == personUpdateRequest.PersonId);

            if (matchingPerson == null) throw new ArgumentException("invalid PersonId");

            //Update property

            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();

            await _db.SaveChangesAsync();//UPDATE
            return matchingPerson.ToPersonReoponse();

        }

        public async Task<bool> DeletePerosn(Guid? personId)
        {
            if (personId == null) throw new ArgumentNullException("id cant be null");

            Person? person = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == personId);

            if (person == null) return false;

            _db.Persons.Remove(person);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
