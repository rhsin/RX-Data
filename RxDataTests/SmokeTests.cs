using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RxDataTests
{
    public class SmokeTests : IClassFixture<WebApplicationFactory<RxData.Startup>>
    {
        private readonly HttpClient _client;

        public SmokeTests(WebApplicationFactory<RxData.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("api/RxPrices")]
        [InlineData("api/RxPrices/1")]
        [InlineData("api/RxPrices/Fetch/Baclofen")]
        [InlineData("api/RxPrices/Fetch/Canada/Baclofen")]
        [InlineData("api/RxPrices/Fetch/Alt/Canada/Prednisone")]
        [InlineData("api/RxPrices/Find/Baclofen")]
        [InlineData("api/RxPrices/Values/Baclofen?column=dose&value=10")]
        [InlineData("api/RxPrices/Medication?name=zanaflex&location=walmart&price=13")]
        [InlineData("api/RxPrices/Price/Mg?name=baclofen")]
        [InlineData("api/Users")]
        [InlineData("api/Users/1")]
        [InlineData("api/Vendors")]
        [InlineData("api/Vendors/1")]
        [InlineData("api/Vendors/Find?medication=sone&location=wal")]
        public async Task TestGetEndpoints(string url)
        {
            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("api/RxPrices")]
        [InlineData("api/Vendors")]
        public async Task TestAuthPostEndpoints(string url)
        {
            var response = await _client.PostAsync(url, null);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/RxPrices/600")]
        [InlineData("api/Vendors/600")]
        public async Task TestAuthPutEndpoints(string url)
        {
            var response = await _client.PutAsync(url, null);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/RxPrices/600")]
        [InlineData("api/Vendors/600")]
        public async Task TestAuthDeleteEndpoints(string url)
        {
            var response = await _client.DeleteAsync(url);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}