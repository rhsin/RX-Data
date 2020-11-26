using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RxData.DTO;
using RxData.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RxDataTests.Integration
{
    public class ErrorTests : IClassFixture<WebApplicationFactory<RxData.Startup>>
    {
        private readonly HttpClient _client;

        public ErrorTests(WebApplicationFactory<RxData.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ErrorResponse()
        {
            var response = await _client.DeleteAsync("api/Users/100000");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<Error>(stringResponse);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal(500, error.StatusCode);
            Assert.Equal("Value cannot be null. (Parameter 'entity')", error.Message);
            Assert.Contains("UsersController.DeleteUser", error.StackTrace);
        }

        [Fact]
        public async Task NotFoundError()
        {
            var response = await _client.GetAsync("api/RxPrices/100000");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task BadRequestError()
        {
            var user = new User { Id = 500, Name = "T", Email = "test.com" };
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("api/Users/1000", data);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UnauthorizedError()
        {
            var rxPrice = new RxPrice { Id = 500, Name = "Test", Price = 10 };
            var json = JsonConvert.SerializeObject(rxPrice);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("api/RxPrices/500", data);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ValidatonError()
        {
            var user = new User { Name = "T", Email = "test.com" };
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Users", data);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var error = JObject.Parse(stringResponse);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("One or more validation errors occurred.", error["title"]);
            Assert.NotEmpty(error["errors"]["Name"]);
            Assert.Contains("minimum length of 3", stringResponse);
        }

        [Fact]
        public async Task FetchRxPricesError()
        {
            var response = await _client.GetAsync("api/RxPrices/Fetch/aaaaaa");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<Error>(stringResponse);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal("Medication Not Found: aaaaaa!", error.Message);
        }

        [Fact]
        public async Task FindValuesError()
        {
            var response = await _client.GetAsync("api/RxPrices/Values/Ba?column=dddddddddd");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<Error>(stringResponse);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal("Invalid Column Argument: dddddddddd!", error.Message);
        }
    }
}
