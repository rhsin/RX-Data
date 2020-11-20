using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RxData.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RxDataTests.Integration
{
    public class RxPriceTests : IClassFixture<WebApplicationFactory<RxData.Startup>>
    {
        private readonly HttpClient _client;

        public RxPriceTests(WebApplicationFactory<RxData.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetRxPrices()
        {
            var response = await _client.GetAsync("api/RxPrices");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rxPrices = JsonConvert.DeserializeObject<List<RxPrice>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(0, rxPrices.Count());
        }

        //[Fact]
        //public async Task GetRxPrice()
        //{
        //    var response = await _client.GetAsync($"api/RxPrices/1");
        //    var stringResponse = await response.Content.ReadAsStringAsync();
        //    var rxPrice = JsonConvert.DeserializeObject<RxPrice>(stringResponse);

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}
    }
}