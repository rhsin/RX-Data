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
    public class VendorTests : IClassFixture<WebApplicationFactory<RxData.Startup>>
    {
        private readonly HttpClient _client;

        public VendorTests(WebApplicationFactory<RxData.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetVendors()
        {
            var response = await _client.GetAsync("api/Vendors");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var vendors = JsonConvert.DeserializeObject<List<Vendor>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, vendors.Count());
            Assert.Contains("SingleCare", stringResponse);
            Assert.Contains("CanadaRx24h", stringResponse);
            Assert.Contains("baclofen", stringResponse);
        }

        [Fact]
        public async Task GetVendor()
        {
            var response = await _client.GetAsync($"api/Vendors/1");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var vendor = JsonConvert.DeserializeObject<Vendor>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("SingleCare", vendor.Name);
            Assert.Equal("https://www.singlecare.com", vendor.Url);
            Assert.Contains("baclofen", stringResponse);
            Assert.Contains("geniusrx", stringResponse);
        }

        [Fact]
        public async Task SeedVendors()
        {
            var response = await _client.PostAsync($"api/Vendors/Seeder", null);
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Vendors Already Seeded!", stringResponse);
        }
    }
}