using Enities;
using RepositoriesContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonsRepository _personsRepository;
        public PersonService(IPersonsRepository personsRepository)
        {
            _personsRepository = personsRepository;
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
            await _personsRepository.AddPerson(person);

            //convert to Person Reponse
            PersonReponse personReponse = person.ToPersonReoponse();

            //Populate Country into Person and return it
            return personReponse;
        }

        public async Task<PersonReponse?> GetPersonById(Guid? personId)
        {
            //check if personId is null
            if (personId == null) return null;

            Person? person = await _personsRepository.GetPersonById(personId);

            // check if person is null 
            if (person == null) return null;

            return person.ToPersonReoponse();
        }

        public async Task<List<PersonReponse>> GetAllPerson()
        {
            return (await _personsRepository.GetAllPersons()).Select(u => u.ToPersonReoponse()).ToList();
        }

        public async Task<List<PersonReponse>> GetFilterPerson(string searchBy, string? searchString)
        {
            List<Person> persons = searchBy switch
            {
                nameof(PersonReponse.PersonName) =>
                await _personsRepository.GetFilterPersons(temp =>
                temp.PersonName.Contains(searchString)),

                nameof(PersonReponse.Email) =>
                await _personsRepository.GetFilterPersons(temp =>
                temp.Email.Contains(searchString)),

                nameof(PersonReponse.DateOfBirth) =>
                await _personsRepository.GetFilterPersons(temp =>
                temp.DateOfBirth.Value.ToShortDateString().Contains(searchString)),

                nameof(PersonReponse.Address) =>
                await _personsRepository.GetFilterPersons(temp =>
                temp.Address.Contains(searchString)),


                nameof(PersonReponse.Country) =>
                await _personsRepository.GetFilterPersons(temp =>
                temp.Country.CountryName.Contains(searchString)),

                _ => await _personsRepository.GetAllPersons()
            };

            return persons.Select(temp => temp.ToPersonReoponse()).ToList();
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
            Person? matchingPerson = await _personsRepository.GetPersonById(personUpdateRequest.PersonId);

            if (matchingPerson == null) throw new ArgumentException("invalid PersonId");

            //Update property

            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();

            await _personsRepository.UpdatePerson(matchingPerson);//UPDATE
            return matchingPerson.ToPersonReoponse();

        }

        public async Task<bool> DeletePerosn(Guid? personId)
        {
            if (personId == null) throw new ArgumentNullException("id cant be null");

            Person? person = await _personsRepository.GetPersonById(personId);

            if (person == null) return false;

            await _personsRepository.DeletePersonByPersonID(personId);

            return true;
        }
    }
}
