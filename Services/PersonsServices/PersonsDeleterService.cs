using Enities;
using Microsoft.Extensions.Logging;
using RepositoriesContracts;
using ServiceContracts.IPersonsServices;

namespace Services.PersonsServices
{
    public class PersonsDeleterService : IPersonsDeleterService
    {
        private readonly IPersonsRepository _personsRepository;
        private ILogger<PersonsDeleterService> _logger;
        public PersonsDeleterService(IPersonsRepository personsRepository, ILogger<PersonsDeleterService> logger)
        {
            _logger = logger;
            _personsRepository = personsRepository;
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
