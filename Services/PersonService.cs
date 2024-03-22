using Enities;
using ServiceContracts;
using ServiceContracts.DTO;
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
            throw new NotImplementedException();
        }
    }
}
