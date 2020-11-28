using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RxData.DTO;
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
            Assert.True(rxPrices.Count() >= 50);
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
            Assert.True(rxPrices.Count() >= 5);
            Assert.All(rxPrices, rp => Assert.Equal("baclofen", rp.Name));
            Assert.All(rxPrices, rp => Assert.True(rp.Quantity >= 15));
            Assert.All(rxPrices, rp => Assert.True(rp.Price >= 12));
            Assert.Contains("walmart pharmacy", stringResponse);
        }

        [Fact]
        public async Task FetchRxPricesCanada()
        {
            var response = await _client.GetAsync("api/RxPrices/Fetch/Canada/Baclofen");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rxPrices = JsonConvert.DeserializeObject<List<RxPrice>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(rxPrices.Count() >= 10);
            Assert.All(rxPrices, rp => Assert.Equal("baclofen", rp.Name));
            Assert.All(rxPrices, rp => Assert.True(rp.Quantity >= 30));
            Assert.All(rxPrices, rp => Assert.True(rp.Dose >= 10));
            Assert.All(rxPrices, rp => Assert.True(rp.Price >= 50));
        }

        [Fact]
        public async Task FetchRxPricesCanadaAlt()
        {
            var response = await _client.GetAsync("api/RxPrices/Fetch/Alt/Canada/Prednisone");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rxPrices = JsonConvert.DeserializeObject<List<RxPrice>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(rxPrices.Count() >= 5);
            Assert.All(rxPrices, rp => Assert.Equal("prednisone", rp.Name));
            Assert.All(rxPrices, rp => Assert.True(rp.Quantity >= 5));
            Assert.All(rxPrices, rp => Assert.True(rp.Dose >= 5));
            Assert.All(rxPrices, rp => Assert.True(rp.Price >= 25));
        }

        [Fact]
        public async Task FindRxPrices()
        {
            var response = await _client.GetAsync("api/RxPrices/Find/Baclofen");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rxPrices = JsonConvert.DeserializeObject<RxPriceDTO>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Find All RxPrices: Baclofen", rxPrices.Method);
            Assert.True(rxPrices.Count >= 23);
            Assert.True(rxPrices.RxPrices.Any());
            Assert.Contains("baclofen", stringResponse);
            Assert.Contains("Admin", stringResponse);
        }

        [Fact]
        public async Task FindValues()
        {
            var response = await _client.GetAsync("api/RxPrices/Values/Baclofen?column=dose&value=10");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rxPrices = JsonConvert.DeserializeObject<RxPriceDTO>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Find Baclofen By dose: 10", rxPrices.Method);
            Assert.True(rxPrices.Count >= 6);
            Assert.True(rxPrices.RxPrices.Any());
            Assert.Contains("baclofen", stringResponse);
            Assert.Contains("CanadaRx24h", stringResponse);
        }

        [Fact]
        public async Task FindMedication()
        {
            var response = await _client.GetAsync(
                "api/RxPrices/Medication?name=zanaflex&location=walmart&price=13");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rxPrices = JsonConvert.DeserializeObject<RxPriceDTO>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Get Medication: zanaflex", rxPrices.Method);
            Assert.True(rxPrices.RxPrices.Count() >= 2);
            Assert.Contains("zanaflex", stringResponse);
            Assert.Contains("walmart", stringResponse);
            Assert.Contains("SingleCare", stringResponse);
        }

        [Fact]
        public async Task GetRxPricesPerMg()
        {
            var response = await _client.GetAsync("api/RxPrices/Price/Mg/Baclofen");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rxPrices = JsonConvert.DeserializeObject<List<RxPrice>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(rxPrices.Count() >= 15);
            Assert.All(rxPrices, rp => Assert.True(rp.Price > 0));
            Assert.Contains("baclofen", stringResponse);
            Assert.Contains("0.065", stringResponse);
            Assert.Contains("0.067", stringResponse);
        }

        //[Fact]
        //public async Task SeedRxPrices()
        //{
        //    var response = await _client.PostAsync($"api/RxPrices/Seeder/Baclofen", null);
        //    var stringResponse = await response.Content.ReadAsStringAsync();

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal("RxPrices Already Seeded: Baclofen!", stringResponse);
        //}
    }
}