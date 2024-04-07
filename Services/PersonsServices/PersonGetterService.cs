using Enities;
using Microsoft.Extensions.Logging;
using RepositoriesContracts;
using ServiceContracts.DTO;
using ServiceContracts.IPersonsServices;

namespace Services.PersonsServices
{
    public class PersonGetterService : IPersonsGetterService
    {
        private readonly IPersonsRepository _personsRepository;
        private ILogger<PersonGetterService> _logger;
        public PersonGetterService(IPersonsRepository personsRepository, ILogger<PersonGetterService> logger)
        {
            _logger = logger;
            _personsRepository = personsRepository;
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


    }
}
