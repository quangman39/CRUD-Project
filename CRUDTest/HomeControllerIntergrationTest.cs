using FluentAssertions;
using Xunit;

namespace CRUDTest
{
    public class HomeControllerIntergrationTest : IClassFixture<CustomWebApplicationFactorry>
    {

        private readonly HttpClient _httpClient;

        public HomeControllerIntergrationTest(CustomWebApplicationFactorry factory)
        {
            _httpClient = factory.CreateClient();
        }

        #region Index

        [Fact]
        public async void Index_ToReturnView()
        {
            //Arrange

            //Act
            HttpResponseMessage httpResponse = await _httpClient.GetAsync("/Home/Index");
            //Asserts
            httpResponse.Should().BeSuccessful();
        }
        #endregion

    }
}
