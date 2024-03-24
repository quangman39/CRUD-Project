using Enities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _listPerson;
        private readonly ICountriesService _countriesService;
        public PersonService()
        {
            _listPerson = new List<Person>();
            _countriesService = new CountriesService();
        }
        private PersonReponse PopulateCounry(PersonReponse personReponse)
        {
            CountryReponse? countryReponse = _countriesService.GetByID(personReponse.CountryId);
            personReponse.Country = countryReponse?.CountryName;
            return personReponse;
        }
        public PersonReponse AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if personAdd is null
            if (personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));


            //Validator PersonAddRequest
            ValidationHelper.ModelValition(personAddRequest);


            //Convert, generator id and  add it into list
            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();
            _listPerson.Add(person);

            //convert to Person Reponse
            PersonReponse personReponse = person.ToPersonReoponse();

            //Populate Country into Person and return it
            return PopulateCounry(personReponse);
        }

        public PersonReponse? GetPersonById(Guid? personId)
        {
            //check if personId is null
            if (personId == null) return null;

            Person? person = _listPerson.FirstOrDefault(temp => temp.PersonID == personId);

            // check if person is null 
            if (person == null) return null;

            return person.ToPersonReoponse();
        }

        public List<PersonReponse> GetAllPerson()
        {
            return _listPerson.Select(u => u.ToPersonReoponse()).ToList();
        }

        public List<PersonReponse> GetFilterPerson(string searchBy, string? searchString)
        {
            List<PersonReponse> allPersons = GetAllPerson();
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
        public List<PersonReponse> GetSortedPerson(List<PersonReponse> allPersons, string SortBy, SortOrderOptions sortOrder)
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

                (nameof(PersonReponse.Age), SortOrderOptions.ASC)
                => allPersons.OrderBy(temp => temp.Age).ToList(),
                (nameof(PersonReponse.Age), SortOrderOptions.DESC)
               => allPersons.OrderByDescending(temp => temp.Age).ToList(),
            };

            return sortedPersons;
        }

        public PersonReponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) throw new ArgumentNullException("arg cant be null");

            //Validate property in personUpdateRequest
            ValidationHelper.ModelValition(personUpdateRequest);

            //find person 
            Person? matchingPerson = _listPerson.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonId);

            if (matchingPerson == null) throw new ArgumentException("invalid PersonId");

            //Update property

            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();

            return matchingPerson.ToPersonReoponse();

        }

        public bool DeletePerosn(Guid? personId)
        {
            if (personId == null) throw new ArgumentNullException("id cant be null");

            Person? person = _listPerson.FirstOrDefault(temp => temp.PersonID == personId);

            if (person == null) return false;

            return _listPerson.Remove(person); ;


        }
    }
}
