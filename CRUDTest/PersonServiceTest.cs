using AutoFixture;
using Enities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
        private readonly IFixture fixture;

        public PersonServiceTest()
        {
            fixture = new Fixture();

            var countriesInitialData = new List<Country>() { };
            var personInitialData = new List<Person>() { };
            DbContextMock<ApplicationDbContext> dbContextMook = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );

            ApplicationDbContext dbContext = dbContextMook.Object;
            dbContextMook.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMook.CreateDbSetMock(temp => temp.Persons, personInitialData);
            _countriesService = new CountriesService(dbContext);
            _personService = new PersonService(dbContext);
        }
        #region AddPerson

        //it should throw ArgumentNull when personAddReques is assign null
        [Fact]
        public async Task AddPerson_NullArg()
        {
            // Arrange
            PersonAddRequest? personAddRequest = null;

            //Acts 
            Func<Task> action = async () => await _personService.AddPerson(personAddRequest);

            // Assert

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        //when Name is null, it should throw ArgumentException
        public async Task AddPerson_NullName()
        {
            //Arrange
            PersonAddRequest? personAddRequest = fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone@example.com")
                                                        .With(temp => temp.PersonName, null as string).Create();


            //Acts 
            Func<Task> action = async () => await _personService.AddPerson(personAddRequest);

            // Assert

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        //when supplies proper arg, it should return obj of PersonReponse which include the newly generated Id 
        public async Task Add_Person_ProperArg()
        {
            //Arrange


            CountryAddRequest? countryAddRequest = fixture.Create<CountryAddRequest>();
            CountryReponse countryReponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = fixture.Build<PersonAddRequest>()
                                                        .With(temp => temp.Email, "someone@example.com")
                                                        .With(temp => temp.CountryId, countryReponse.CountryId)
                                                        .Create();


            //Act
            PersonReponse personReponse = await _personService.AddPerson(personAddRequest);

            //Assert
            personReponse.PersonID.Should().NotBe(Guid.Empty);



        }

        #endregion

        #region GetPersonId

        [Fact]
        //When PersonId is null, it should return null
        public async Task GetPersonById_NullId()
        {
            //Arrange
            Guid? personId = null;

            PersonReponse? personReponse = await _personService.GetPersonById(personId);

            //Assert
            personReponse.Should().BeNull();

        }

        [Fact]
        //When supplies wrong id , it should return null obj
        public async Task GetPersonById_WrongId()
        {
            //Arrange
            Guid? personId = Guid.NewGuid();

            PersonReponse? personReponse = await _personService.GetPersonById(personId);

            //Assert

            personReponse.Should().BeNull();



        }


        [Fact]
        //When supplies proper PersonId , it should return null obj
        public async Task GetPersonById_ProperId()
        {
            //Arrange

            CountryAddRequest? countryAddRequest = fixture.Create<CountryAddRequest>();
            CountryReponse countryReponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = fixture.Build<PersonAddRequest>()
                                                        .With(temp => temp.Email, "someone@example.com")
                                                        .With(temp => temp.CountryId, countryReponse.CountryId)
                                                        .Create();


            PersonReponse person_from_add = await _personService.AddPerson(personAddRequest);
            Guid? personId = person_from_add.PersonID;

            //Act
            PersonReponse? personReponse = await _personService.GetPersonById(personId);

            //Assert

            personReponse.Should().Be(person_from_add);



        }

        #endregion

        #region GetAllPerson
        [Fact]
        // List of person shoulde emty before add person
        public async Task GetAllPerson_BeforeAdd()
        {

            //Arrange 
            List<PersonReponse>? list_person_from_getAll = await _personService.GetAllPerson();


            //Assert
            list_person_from_getAll.Should().BeEmpty();

        }

        [Fact]
        // when applies proper persons should return proper list PersonReponse
        public async Task GetAllPerson_ProperArgs()
        {
            //Arrange 

            CountryAddRequest? countryAddRequest = fixture.Create<CountryAddRequest>();
            CountryReponse countryReponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest1 = fixture.Build<PersonAddRequest>()
                                                        .With(temp => temp.Email, "someone@example.com")
                                                        .With(temp => temp.CountryId, countryReponse.CountryId)
                                                        .Create();
            PersonAddRequest personAddRequest2 = fixture.Build<PersonAddRequest>()
                                                       .With(temp => temp.Email, "someone@example.com")
                                                       .With(temp => temp.CountryId, countryReponse.CountryId)
                                                       .Create();
            PersonAddRequest personAddRequest3 = fixture.Build<PersonAddRequest>()
                                                       .With(temp => temp.Email, "someone@example.com")
                                                       .With(temp => temp.CountryId, countryReponse.CountryId)
                                                       .Create();

            List<PersonAddRequest> personAddRequestList = new List<PersonAddRequest>
            {personAddRequest1,personAddRequest2,personAddRequest3
            };
            List<PersonReponse> list_from_add = new List<PersonReponse>();
            List<PersonReponse> list_from_getAll = new List<PersonReponse>();

            //Add person into list
            foreach (var person in personAddRequestList)
            {
                // Await the async call
                var addedPerson = await _personService.AddPerson(person);
                list_from_add.Add(addedPerson);
            }

            //Acts
            list_from_getAll = await
                _personService.GetAllPerson();

            //Assert

            list_from_getAll.Should().BeEquivalentTo(list_from_add);
        }
        #endregion

        #region GetFilterPerson
        [Fact]
        ///If the search text is emty and search by "PersonName", it should return all person
        public async Task GetFilterPerson_EmtySearch()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = fixture.Create<CountryAddRequest>();
            CountryReponse countryReponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest1 = fixture.Build<PersonAddRequest>()
                                                        .With(temp => temp.Email, "someone@example.com")
                                                        .With(temp => temp.CountryId, countryReponse.CountryId)
                                                        .Create();
            PersonAddRequest personAddRequest2 = fixture.Build<PersonAddRequest>()
                                                       .With(temp => temp.Email, "someone@example.com")
                                                       .With(temp => temp.CountryId, countryReponse.CountryId)
                                                       .Create();
            PersonAddRequest personAddRequest3 = fixture.Build<PersonAddRequest>()
                                                       .With(temp => temp.Email, "someone@example.com")
                                                       .With(temp => temp.CountryId, countryReponse.CountryId)
                                                        .Create();

            List<PersonAddRequest> personAddRequestList = new List<PersonAddRequest>
            {personAddRequest1,personAddRequest2,personAddRequest3
            };
            List<PersonReponse> list_from_add = new List<PersonReponse>();
            List<PersonReponse> list_from_getAll = new List<PersonReponse>();

            //Add person into list
            foreach (var person in personAddRequestList)
            {
                // Await the async call
                var addedPerson = await _personService.AddPerson(person);
                list_from_add.Add(addedPerson);
            }

            //Acts
            List<PersonReponse> list_to_from_sort = await _personService.GetFilterPerson(nameof(Person.PersonName), "");

            //Asserts
            list_to_from_sort.Should().BeEquivalentTo(list_from_add);
        }


        //Fisrt we add a few person; then we will search based on person name with some search string
        //it should return matching person
        [Fact]
        public async Task GetFilterPerson_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = fixture.Create<CountryAddRequest>();
            CountryReponse countryReponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest1 = fixture.Build<PersonAddRequest>()
                                                            .With(temp => temp.PersonName, "man")
                                                        .With(temp => temp.Email, "someone@example.com")
                                                        .With(temp => temp.CountryId, countryReponse.CountryId)
                                                        .Create();
            PersonAddRequest personAddRequest2 = fixture.Build<PersonAddRequest>()
                                                            .With(temp => temp.PersonName, "abc")

                                                       .With(temp => temp.Email, "someone@example.com")
                                                       .With(temp => temp.CountryId, countryReponse.CountryId)
                                                       .Create();
            PersonAddRequest personAddRequest3 = fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, "Nma")
                                                       .With(temp => temp.Email, "someone@example.com")
                                                       .With(temp => temp.CountryId, countryReponse.CountryId)
                                                       .Create();

            List<PersonAddRequest> personAddRequestList = new List<PersonAddRequest>
            {personAddRequest1,personAddRequest2,personAddRequest3
            };

            List<PersonReponse> list_from_add = new List<PersonReponse>();
            List<PersonReponse> list_from_getAll = new List<PersonReponse>();

            //Add person into list
            foreach (var person in personAddRequestList)
            {
                // Await the async call
                var addedPerson = await _personService.AddPerson(person);
                list_from_add.Add(addedPerson);
            }

            //Acts
            List<PersonReponse> list_to_from_filed = await _personService.GetFilterPerson(nameof(Person.PersonName), "ma");

            //Asserts
            list_to_from_filed.Should().OnlyContain(temp => temp.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase));
        }


        #endregion

        #region GetSortedPerson
        [Fact]
        //when person based on PersonName in DESC, it should return persons list in descending on PersonName
        public async Task GetSortedPerson()
        {

            //Arrange 
            CountryAddRequest? countryAddRequest = fixture.Create<CountryAddRequest>();
            CountryReponse countryReponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest1 = fixture.Build<PersonAddRequest>()
                                                        .With(temp => temp.Email, "someone@example.com")
                                                        .With(temp => temp.CountryId, countryReponse.CountryId)
                                                        .Create();
            PersonAddRequest personAddRequest2 = fixture.Build<PersonAddRequest>()
                                                       .With(temp => temp.Email, "someone@example.com")
                                                       .With(temp => temp.CountryId, countryReponse.CountryId)
                                                       .Create();
            PersonAddRequest personAddRequest3 = fixture.Build<PersonAddRequest>()
                                                       .With(temp => temp.Email, "someone@example.com")
                                                       .With(temp => temp.CountryId, countryReponse.CountryId)
                                                      .Create();

            List<PersonAddRequest> personAddRequestList = new List<PersonAddRequest>
            {personAddRequest1,personAddRequest2,personAddRequest3
            };

            List<PersonReponse> list_from_add = new List<PersonReponse>();

            //Add person into list
            foreach (var person in personAddRequestList)
            {
                // Await the async call
                var addedPerson = await _personService.AddPerson(person);
                list_from_add.Add(addedPerson);
            }
            List<PersonReponse> list_from_getAll = await _personService.GetAllPerson();

            //Act 
            List<PersonReponse> persons_list_from_search = await _personService.GetSortedPerson(list_from_getAll, nameof(Person.PersonName), SortOrderOptions.ASC);

            //Assert
            persons_list_from_search.Should().BeInAscendingOrder(temp => temp.PersonName);
        }



        #endregion

        #region UpdatePerson

        //when we supply null as PersonUpdateRequest, we should throw ArgNullException
        [Fact]
        public async Task UpdatePerson_nullArg()
        {
            //Arrange
            PersonUpdateRequest personUpdateRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }
        //when we supply a invalid id,it  should throw ArgException
        [Fact]
        public async Task UpdatePerson_InvalidId()
        {
            //Arrange
            PersonUpdateRequest personUpdateRequest = new()
            {
                PersonId = Guid.NewGuid(),
            };
            //Act
            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //when PersonName is null,it  should throw ArgException
        [Fact]
        public async Task UpdatePerson_PersonNameisNull()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = fixture.Create<CountryAddRequest>();
            CountryReponse countryReponse = await _countriesService.AddCountry(countryAddRequest);
            PersonAddRequest personAdd = fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone@gmail.com")
                                                .With(temp => temp.CountryId, countryReponse.CountryId)
                                                .Create();

            PersonReponse person_reponse_from_add = await _personService.AddPerson(personAdd);
            PersonUpdateRequest person_update_request = person_reponse_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = null;


            //Assert
            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(person_update_request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // Add new person in obj, and update Name and Email
        [Fact]
        public async Task UpdatePerson_UpdateNameandEmail()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = fixture.Create<CountryAddRequest>();
            CountryReponse countryReponse = await _countriesService.AddCountry(countryAddRequest);
            PersonAddRequest personAdd = fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone@gmail.com")
                                                .With(temp => temp.CountryId, countryReponse.CountryId)
                                                .Create();

            PersonReponse person_reponse_from_add = await _personService.AddPerson(personAdd);
            PersonUpdateRequest person_update_request = person_reponse_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "Main";
            person_update_request.Email = "main@edu.vn";

            //Act
            PersonReponse person_reponse_from_update = await _personService.UpdatePerson(person_update_request);
            PersonReponse? person_reponse_from_get = await _personService.GetPersonById(person_reponse_from_update.PersonID);

            //Assert
            person_reponse_from_update.Should().Be(person_reponse_from_get);



        }


        #endregion

        #region Delete Person

        //when supply null id, it should return ArgumentNull
        [Fact]
        public async Task DeletePerson_nullArg()
        {
            //Arrange
            Guid? personId = null;

            //Acts
            Func<Task> action = async () =>
            {
                await _personService.DeletePerosn(personId);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }




        // when supplies valid value, it should delete obj in person list and return true
        [Fact]
        public async Task DeletePerson_ValidPerson()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = fixture.Create<CountryAddRequest>();
            CountryReponse countryReponse = await _countriesService.AddCountry(countryAddRequest);
            PersonAddRequest personAdd = fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone@gmail.com")
                                                .With(temp => temp.CountryId, countryReponse.CountryId)
                                                .Create();
            PersonReponse persons_from_add = await _personService.AddPerson(personAdd);

            //Act
            Boolean isDeleted = await _personService.DeletePerosn(persons_from_add.PersonID);
            PersonReponse? person_after_delete = await _personService.GetPersonById(persons_from_add.PersonID);

            //Assert
            isDeleted.Should().BeTrue();
            person_after_delete.Should().BeNull();
        }


        // when supplies invalid value, it should  return failse
        [Fact]
        public async Task DeletePerson_InValidPerson()
        {
            //Arrange
            Guid personId = Guid.NewGuid();

            //Act
            Boolean isDeleted = await _personService.DeletePerosn(personId);

            //Assert
            isDeleted.Should().BeFalse();
        }

        #endregion
    }
}
