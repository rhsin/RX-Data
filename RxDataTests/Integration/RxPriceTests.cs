using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RxData.Models;
using System.Collections.Generic;
using System;
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
            Assert.Equal(19, rxPrices.Count());
            Assert.All(rxPrices, rp => Assert.NotNull(rp.Name));
            Assert.All(rxPrices, rp => Assert.True(rp.Quantity > 0));
            Assert.All(rxPrices, rp => Assert.True(rp.Price > 0));
            Assert.Contains("baclofen", stringResponse);
        }

        [Fact]
        public async Task GetRxPrice()
        {
            var response = await _client.GetAsync($"api/RxPrices/1");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rxPrice = JsonConvert.DeserializeObject<RxPrice>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("baclofen", rxPrice.Name);
            Assert.Equal(13.06, Math.Round(rxPrice.Price, 2));
            Assert.Equal("geniusrx", rxPrice.Location);
            Assert.Equal(1, rxPrice.VendorId);
        }

        [Fact]
        public async Task FetchRxPrices()
        {
            var response = await _client.GetAsync("api/RxPrices/Fetch/Baclofen");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rxPrices = JsonConvert.DeserializeObject<List<RxPrice>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(8, rxPrices.Count());
            Assert.All(rxPrices, rp => Assert.Equal("baclofen", rp.Name));
            Assert.All(rxPrices, rp => Assert.True(rp.Quantity >= 15));
            Assert.All(rxPrices, rp => Assert.True(rp.Price > 12));
            Assert.Contains("walmart pharmacy", stringResponse);
        }

        [Fact]
        public async Task FetchRxPricesCanada()
        {
            var response = await _client.GetAsync("api/RxPrices/Fetch/Canada/Baclofen");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rxPrices = JsonConvert.DeserializeObject<List<RxPrice>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(11, rxPrices.Count());
            Assert.All(rxPrices, rp => Assert.Equal("baclofen", rp.Name));
            Assert.All(rxPrices, rp => Assert.True(rp.Quantity >= 30));
            Assert.All(rxPrices, rp => Assert.True(rp.Dose >= 10));
            Assert.All(rxPrices, rp => Assert.True(rp.Price > 50));
        }

        [Fact]
        public async Task SeedRxPrices()
        {
            var response = await _client.PostAsync($"api/RxPrices/Seeder/Baclofen", null);
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("RxPrices Already Seeded: Baclofen!", stringResponse);
        }
    }
}