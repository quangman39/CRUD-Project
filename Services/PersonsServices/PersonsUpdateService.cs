using Enities;
using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoriesContracts;
using ServiceContracts.DTO;
using ServiceContracts.IPersonsServices;
using Services.Helpers;

namespace Services.PersonsServices
{
    public class PersonsUpdateService : IPersonsUpdateService
    {
        private readonly IPersonsRepository _personsRepository;
        private ILogger<PersonsUpdateService> _logger;
        public PersonsUpdateService(IPersonsRepository personsRepository, ILogger<PersonsUpdateService> logger)
        {
            _logger = logger;
            _personsRepository = personsRepository;
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


    }
}
