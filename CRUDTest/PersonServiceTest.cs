using Enities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUDTest
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;

        public PersonServiceTest()
        {
            _personService = new PersonService();
            _countriesService = new CountriesService();
        }
        #region AddPerson

        //it should throw ArgumentNull when personAddReques is assign null
        [Fact]
        public void AddPerson_NullArg()
        {
            //Arrange

            PersonAddRequest? personAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() => _personService.AddPerson(personAddRequest));
        }

        [Fact]
        //when Name is null, it should throw ArgumentException
        public void AddPerson_NullName()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _personService.AddPerson(personAddRequest);
            });
        }

        [Fact]
        //when supplies proper arg, it should return obj of PersonReponse which include the newly generated Id 
        public void Add_Person_ProperArg()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "JaPan" };
            CountryReponse countryReponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Man",
                Address = "Gia Lai",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                Email = "Dorakid@gmail.com",
            };

            //Act
            PersonReponse personReponse = _personService.AddPerson(personAddRequest);

            //Assert
            Assert.True(personReponse.PersonID != Guid.Empty);


        }

        #endregion

        #region GetPersonId

        [Fact]
        //When PersonId is null, it should return null
        public void GetPersonById_NullId()
        {
            //Arrange
            Guid? personId = null;

            PersonReponse? personReponse = _personService.GetPersonById(personId);

            //Assert

            Assert.Null(personReponse);


        }

        [Fact]
        //When supplies wrong id , it should return null obj
        public void GetPersonById_WrongId()
        {
            //Arrange
            Guid? personId = Guid.NewGuid();

            PersonReponse? personReponse = _personService.GetPersonById(personId);

            //Assert

            Assert.Null(personReponse);


        }


        [Fact]
        //When supplies proper PersonId , it should return null obj
        public void GetPersonById_ProperId()
        {
            //Arrange
            PersonAddRequest person_Add = new PersonAddRequest()
            {
                PersonName = "Man",
                Address = "Gia Lai",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                Email = "Dorakid@gmail.com",
            };

            PersonReponse person_from_add = _personService.AddPerson(person_Add);
            Guid? personId = person_from_add.PersonID;

            //Act
            PersonReponse? personReponse = _personService.GetPersonById(personId);

            //Assert

            Assert.Equal(person_from_add, personReponse);


        }

        #endregion

        #region GetAllPerson
        [Fact]
        // List of person shoulde emty before add person
        public void GetAllPerson_BeforeAdd()
        {

            //Arrange 
            List<PersonReponse>? list_person_from_getAll = _personService.GetAllPerson();


            //Assert
            Assert.Empty(list_person_from_getAll);

        }

        [Fact]
        // when applies proper persons should return proper list PersonReponse
        public void GetAllPerson_ProperArgs()
        {
            //Arrange 
            List<PersonAddRequest> personAddRequest1 = new List<PersonAddRequest>
            {new PersonAddRequest()
            {
                PersonName = "Man",
                Address = "Gia Lai",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                Email = "Dorakid@gmail.com",
            }, new PersonAddRequest()
                {
                    PersonName = "Le",
                    Address = "Gia Lai",
                    DateOfBirth = DateTime.Parse("2000-01-01"),
                    Gender = GenderOptions.Male,
                    ReceiveNewsLetters = true,
                    Email = "Dorakid@gmail.com",
                },  new PersonAddRequest()
                    {
                        PersonName = "Quang",
                        Address = "Gia Lai",
                        DateOfBirth = DateTime.Parse("2000-01-01"),
                        Gender = GenderOptions.Male,
                        ReceiveNewsLetters = true,
                        Email = "Dorakid@gmail.com",
                    }

            };
            List<PersonReponse> list_from_add = new List<PersonReponse>();
            List<PersonReponse> list_from_getAll = new List<PersonReponse>();

            //Add person into list
            foreach (var person in personAddRequest1)
            {
                list_from_add.Add(_personService.AddPerson(person));
            }

            //Acts
            list_from_getAll = _personService.GetAllPerson();

            //Assert
            foreach (var person_reponse in list_from_add)
            {
                Assert.Contains(person_reponse, list_from_getAll);
            }
        }
        #endregion

        #region GetFilterPerson
        [Fact]
        ///If the search text is emty and search by "PersonName", it should return all person
        public void GetFilterPerson_EmtySearch()
        {
            //Arrange
            List<PersonAddRequest> personAddRequest1 = new List<PersonAddRequest>
            {new PersonAddRequest()
            {
                PersonName = "Man",
                Address = "Gia Lai",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                Email = "Dorakid@gmail.com",
            }, new PersonAddRequest()
                {
                    PersonName = "Le",
                    Address = "Gia Lai",
                    DateOfBirth = DateTime.Parse("2000-01-01"),
                    Gender = GenderOptions.Male,
                    ReceiveNewsLetters = true,
                    Email = "Dorakid@gmail.com",
                },  new PersonAddRequest()
                    {
                        PersonName = "Quang",
                        Address = "Gia Lai",
                        DateOfBirth = DateTime.Parse("2000-01-01"),
                        Gender = GenderOptions.Male,
                        ReceiveNewsLetters = true,
                        Email = "Dorakid@gmail.com",
                    }

            };
            List<PersonReponse> list_from_add = new List<PersonReponse>();
            List<PersonReponse> list_from_getAll = new List<PersonReponse>();

            //Add person into list
            foreach (var person in personAddRequest1)
            {
                list_from_add.Add(_personService.AddPerson(person));
            }

            //Acts
            List<PersonReponse> list_to_from_add = _personService.GetFilterPerson(nameof(Person.PersonName), "");

            //Asserts
            foreach (var person_reponse in list_from_add)
            {
                Assert.Contains(person_reponse, list_to_from_add);
            }
        }


        //Fisrt we add a few person; then we will search based on person name with some search string
        //it should return matching person
        [Fact]
        public void GetFilterPerson_SearchByPersonName()
        {
            //Arrange
            List<PersonAddRequest> personAddRequest1 = new List<PersonAddRequest>
            {new PersonAddRequest()
            {
                PersonName = "Man",
                Address = "Gia Lai",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                Email = "Dorakid@gmail.com",
            }, new PersonAddRequest()
                {
                    PersonName = "Le",
                    Address = "Gia Lai",
                    DateOfBirth = DateTime.Parse("2000-01-01"),
                    Gender = GenderOptions.Male,
                    ReceiveNewsLetters = true,
                    Email = "Dorakid@gmail.com",
                },  new PersonAddRequest()
                    {
                        PersonName = "Quang",
                        Address = "Gia Lai",
                        DateOfBirth = DateTime.Parse("2000-01-01"),
                        Gender = GenderOptions.Male,
                        ReceiveNewsLetters = true,
                        Email = "Dorakid@gmail.com",
                    }

            };
            List<PersonReponse> list_from_add = new List<PersonReponse>();
            List<PersonReponse> list_from_getAll = new List<PersonReponse>();

            //Add person into list
            foreach (var person in personAddRequest1)
            {
                list_from_add.Add(_personService.AddPerson(person));
            }

            //Acts
            List<PersonReponse> list_to_from_add = _personService.GetFilterPerson(nameof(Person.PersonName), "");

            //Asserts
            foreach (var person_reponse in list_from_add)
            {
                if (person_reponse.PersonName != null)
                {
                    if (person_reponse.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person_reponse, list_to_from_add);
                    }
                }
            }
        }


        #endregion
    }
}
