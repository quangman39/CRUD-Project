using Enities;
using Microsoft.Extensions.Logging;
using RepositoriesContracts;
using ServiceContracts.DTO;
using ServiceContracts.IPersonsServices;
using Services.Helpers;

namespace Services.PersonsServices
{
    public class PersonsAdderService : IPersonsAdderService
    {
        private readonly IPersonsRepository _personsRepository;
        private ILogger<PersonsAdderService> _logger;
        public PersonsAdderService(IPersonsRepository personsRepository, ILogger<PersonsAdderService> logger)
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
    }
}
