using Microsoft.Extensions.Logging;
using RepositoriesContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using ServiceContracts.IPersonsServices;

namespace Services
{
    public class PersonsSorterService : IPersonsSorterService
    {
        private readonly IPersonsRepository _personsRepository;
        private ILogger<PersonsSorterService> _logger;
        public PersonsSorterService(IPersonsRepository personsRepository, ILogger<PersonsSorterService> logger)
        {
            _logger = logger;
            _personsRepository = personsRepository;
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
    }
}
