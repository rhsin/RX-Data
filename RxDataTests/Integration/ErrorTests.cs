using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            var response = await _client.DeleteAsync("api/RxPrices/100000");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<Error>(stringResponse);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal(500, error.StatusCode);
            Assert.Equal("Value cannot be null. (Parameter 'entity')", error.Message);
            Assert.Contains("RxPricesController.DeleteRxPrice", error.StackTrace);
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
            var vendor = new Vendor { Id = 1000, Name = "Test", Url = "Test.com" };
            var json = JsonConvert.SerializeObject(vendor);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("api/Vendors/100000", data);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ValidatonError()
        {
            var vendor = new Vendor { Name = "T", Url = "Test.com" };
            var json = JsonConvert.SerializeObject(vendor);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Vendors", data);
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
