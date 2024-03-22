using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTest
{
    public class CountryServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountryServiceTest()
        {
            _countriesService = new CountriesService();
        }
        #region AddCountry 
        [Fact]
        // when CountryAddRequest is null, it should throw ArgumentNullException
        public void AddCountry_NullAgr()
        {
            //Arrange 
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _countriesService.AddCountry(request);
            });
        }
        [Fact]
        // When Country Name is nuli, it should throw ArgumnetException
        public void AddCountry_NullName()
        {
            //Arrange 
            CountryAddRequest request = new CountryAddRequest() { CountryName = null };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request);
            });

        }

        [Fact]
        //When the CountryName is duplicate, it should throw Argument Exception
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange 
            CountryAddRequest request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest request2 = new CountryAddRequest() { CountryName = "USA" };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });

        }

        //When supply proper country name, it should add country to the existing list and return corresponding countryResponse
        [Fact]
        public void AddCountry_Propername()
        {
            //Arrange 
            CountryAddRequest request = new CountryAddRequest() { CountryName = "USA" };

            //Act
            CountryReponse respone = _countriesService.AddCountry(request);
            List<CountryReponse> list_country = _countriesService.GetAll();

            //Assert
            Assert.True(respone.CountryId != Guid.Empty);
            Assert.Contains(respone, list_country);
        }

        #endregion

        #region GetAllCountry

        //List of countries should be empty before add any country 
        [Fact]
        public void GetAllCountry_EmtyList()
        {
            //Arrange
            List<CountryReponse> list_country_from_GetAll = _countriesService.GetAll();
            //Assert
            Assert.Empty(list_country_from_GetAll);

        }


        [Fact]
        //Return proper list once it has added several countries  
        public void GetAllCountry_Proper()
        {
            //Arrange 
            List<CountryAddRequest> list_country_reponse = new List<CountryAddRequest>()
            { new CountryAddRequest(){CountryName = "USA" },
               new CountryAddRequest(){CountryName = "Japan" }
            };
            List<CountryReponse> list_country_from_add_country = new List<CountryReponse>();

            //Act
            //Add country in list
            foreach (var country in list_country_reponse)
            {
                list_country_from_add_country.Add(_countriesService.AddCountry(country));
            }

            List<CountryReponse> acctualCountryReponseList = _countriesService.GetAll();

            //Assert
            foreach (CountryReponse expectCountry in list_country_from_add_country)
            {
                Assert.Contains(expectCountry, acctualCountryReponseList);
            }
        }


        #endregion


        #region GetCountryById

        //when id is null should throw ArgumentNullException
        [Fact]
        public void GetCountryById_Null()
        {
            //Arrange
            Guid? id = null;

            //Act
            CountryReponse? reponse = _countriesService.GetByID(id);

            //Assert
            Assert.Null(reponse);
        }

        //when supplie vali Id should throw correctpoding
        [Fact]
        public void GetCountryById_ValidCountry()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = "USA" };
            CountryReponse country_from_add = _countriesService.AddCountry(request);
            List<CountryReponse> list_country = _countriesService.GetAll();
            Guid? id = country_from_add.CountryId;

            //Act
            CountryReponse? country_from_GetId = _countriesService.GetByID(id);

            //Assret
            Assert.Equal(country_from_GetId, country_from_add);
            Assert.Contains(country_from_GetId, list_country);


        }



        #endregion
    }
}
