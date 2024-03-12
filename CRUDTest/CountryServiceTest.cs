using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

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
            CountryAddRequest request1 = new CountryAddRequest() { CountryName= "USA" };
            CountryAddRequest request2 = new CountryAddRequest() { CountryName= "USA" };
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
            CountryAddRequest request = new CountryAddRequest() { CountryName = "USA"};

            //Act
            CountryReponse respone = _countriesService.AddCountry(request);

            //Assert
            Assert.True(respone.CountryId != Guid.Empty);
            
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
            foreach(CountryReponse expectCountry in list_country_from_add_country)
            {
                Assert.Contains(expectCountry, acctualCountryReponseList);
            }
        }


        #endregion

    }
}
