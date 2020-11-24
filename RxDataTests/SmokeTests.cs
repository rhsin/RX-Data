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
        [InlineData("api/RxPrices/Find/Baclofen?column=dose&value=10")]
        [InlineData("api/RxPrices/Medication?name=zanaflex&location=walmart&price=13")]
        [InlineData("api/RxPrices/Price/Mg?name=baclofen")]
        [InlineData("api/Vendors")]
        [InlineData("api/Vendors/1")]
        [InlineData("api/Vendors/Find?medication=sone&location=wal")]
        public async Task TestEndpoints(string url)
        {
            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}