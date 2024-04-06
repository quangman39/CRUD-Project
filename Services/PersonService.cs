using Enities;
using Exceptions;
using Microsoft.Extensions.Logging;
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
        private ILogger<PersonService> _logger;
        public PersonService(IPersonsRepository personsRepository, ILogger<PersonService> logger)
        {
            _logger = logger;
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
            _logger.LogInformation("GetAll in PersonService");

            return (await _personsRepository.GetAllPersons()).Select(u => u.ToPersonReoponse()).ToList();
        }

        public async Task<List<PersonReponse>> GetFilterPerson(string searchBy, string? searchString)
        {

            _logger.LogInformation("GetFilterPerson in PersonService");
            List<Person> matchingPersons;
            switch (searchBy)
            {
                case nameof(PersonReponse.PersonName):
                    matchingPersons = await _personsRepository.GetFilterPersons(temp => temp.PersonName.Contains(searchString));
                    break;

                case nameof(PersonReponse.Email):
                    matchingPersons = await _personsRepository.GetFilterPersons(temp => temp.Email.Contains(searchString));
                    break;


                case nameof(PersonReponse.DateOfBirth):
                    DateTime.TryParse(searchString, out DateTime dtDate);
                    matchingPersons = await _personsRepository.GetFilterPersons(temp => temp.DateOfBirth.Value == dtDate);
                    break;



                case nameof(PersonReponse.CountryId):
                    matchingPersons = await _personsRepository.GetFilterPersons(temp => temp.Country.CountryName.Contains(searchString));
                    break;

                case nameof(PersonReponse.Address):
                    matchingPersons = await _personsRepository.GetFilterPersons(temp => temp.Address.Contains(searchString));
                    break;

                default:
                    matchingPersons = await _personsRepository.GetAllPersons();
                    break;
            }

            return matchingPersons.Select(temp => temp.ToPersonReoponse()).ToList();
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

            if (matchingPerson == null) throw new InvalidIdException("invalid PersonId");

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
