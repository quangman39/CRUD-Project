using AutoFixture;
using CRUD.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Xunit;
namespace CRUDTest
{
    public class HomeControllerTest
    {
        private readonly ICountriesService _countriesService;
        private readonly IPersonService _personService;

        private readonly Mock<IPersonService> _personServiceMock;
        private readonly Mock<ICountriesService> _countriesServiceMock;
        private readonly Mock<ILogger<HomeController>> _loggerMock;
        private readonly Fixture _fixture;
        private readonly HomeController homeController;
        private ILogger<HomeController> _logger;

        public HomeControllerTest()
        {
            _fixture = new Fixture();
            _personServiceMock = new Mock<IPersonService>();
            _countriesServiceMock = new Mock<ICountriesService>();
            _loggerMock = new Mock<ILogger<HomeController>>();
            _logger = _loggerMock.Object;

            _personService = _personServiceMock.Object;
            _countriesService = _countriesServiceMock.Object;
            homeController = new HomeController(_countriesService, _personService, _logger);
        }

        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonList()
        {
            //Arrange
            List<PersonReponse> person_reponse_list = _fixture.Create<List<PersonReponse>>();


            _personServiceMock.Setup(temp => temp.GetFilterPerson(It.IsAny<string>(), It.IsAny<String>()))
            .ReturnsAsync(person_reponse_list);

            _personServiceMock.Setup(temp => temp.GetSortedPerson(It.IsAny<List<PersonReponse>>(), It.IsAny<String>(),
                It.IsAny<SortOrderOptions>())).ReturnsAsync(person_reponse_list);

            //Acts

            IActionResult result = await homeController.Index(
                _fixture.Create<string>(), _fixture.Create<string>(),
                _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonReponse>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(person_reponse_list);
        }

        [Fact]
        public async Task Create_IfNoModelErrors_ToReturnCreateView()
        {
            //Arrange
            PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

            PersonReponse person_response = _fixture.Create<PersonReponse>();

            List<CountryReponse> list_country = _fixture.Create<List<CountryReponse>>();


            _countriesServiceMock.Setup(temp => temp.GetAll())
            .ReturnsAsync(list_country);

            _personServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_response);

            //Acts

            homeController.ModelState.AddModelError("PersonName", "Name cant be blank");

            IActionResult result = await homeController.Create(person_add_request);



            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().Be(person_add_request);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
        }
        [Fact]
        public async Task Create_IfNoErros_RedirectToIndex()
        {
            //Arrange
            PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

            PersonReponse person_response = _fixture.Create<PersonReponse>();

            List<CountryReponse> list_country = _fixture.Create<List<CountryReponse>>();


            _countriesServiceMock.Setup(temp => temp.GetAll())
            .ReturnsAsync(list_country);

            _personServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_response);

            //Acts
            IActionResult result = await homeController.Create(person_add_request);



            //Assert
            RedirectToActionResult viewResult = Assert.IsType<RedirectToActionResult>(result);

            viewResult.ActionName.Should().Be("Index");
        }
    }
}