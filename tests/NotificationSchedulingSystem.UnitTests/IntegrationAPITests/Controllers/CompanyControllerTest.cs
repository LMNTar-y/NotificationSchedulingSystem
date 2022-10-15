using System.Net;
using System.Text;
using System.Text.Json;
using NotificationSchedulingSystem.Business.Models;

namespace NotificationSchedulingSystem.UnitTests.IntegrationAPITests.Controllers
{
    [Collection("WebApplicationFactory collection")]
    public class CompanyControllerTest
    {
        private readonly WebApplicationFactorySetupMock _setupMock;
        private readonly HttpClient _client;

        public CompanyControllerTest(WebApplicationFactorySetupMock setupMock)
        {
            _setupMock = setupMock;
            _client = _setupMock.Setup();
            _client.BaseAddress = new Uri("https://localhost/api/Company/");
        }

        [Fact]
        public async Task GetAllRequest_ReturnSuccess()
        {
            //Arrange 
            //Act
            var response = await _client.GetAsync("");

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetRequest_404NotFound_WhenWrongEndpoint()
        {
            //Arrange 
            //Act
            var response = await _client.GetAsync("anyWrongEndpoint");

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Content);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetByIdRequest_ReturnSuccess_WhenValidGuid()
        {
            //Arrange 
            Guid id = Guid.NewGuid();
            //Act
            var response = await _client.GetAsync($"{id}");

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostRequest_PassValidation_ReturnCreated()
        {
            //Arrange 
            var companyRequest = new CompanyRequest()
            {
                Name = "TestName",
                Number = 1123456,
                CompanyType = "Small",
                Market = "Denmark"
            };
            //Act
            var response = await _client.PostAsync("",
                new StringContent(JsonSerializer.Serialize(companyRequest), Encoding.UTF8, "application/json"));

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PostRequest_DoNotPassValidation_ReturnBadRequest()
        {
            //Arrange 
            var companyRequest = new CompanyRequest()
            {
                Name = "Test",
                Number = -1,
                CompanyType = "Test",
                Market = "Test"
            };
            //Act
            var response = await _client.PostAsync("",
                new StringContent(JsonSerializer.Serialize(companyRequest), Encoding.UTF8, "application/json"));
            
            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}