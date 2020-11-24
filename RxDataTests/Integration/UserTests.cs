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
    public class UserTests : IClassFixture<WebApplicationFactory<RxData.Startup>>
    {
        private readonly HttpClient _client;

        public UserTests(WebApplicationFactory<RxData.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetUsers()
        {
            var response = await _client.GetAsync("api/Users");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(users.Count() >= 1);
            Assert.Contains("Ryan", stringResponse);
            Assert.Contains("rxPriceId", stringResponse);
            Assert.Contains("vendorId", stringResponse);
        }

        [Fact]
        public async Task GetUser()
        {
            var response = await _client.GetAsync($"api/Users/1");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, user.Id);
            Assert.Equal("Ryan", user.Name);
            Assert.True(user.RxPriceUsers.Count() >= 1);
            Assert.Contains("location", stringResponse);
        }
    }
}