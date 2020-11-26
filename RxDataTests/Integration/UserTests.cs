using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RxData.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
            Assert.Contains("Admin", stringResponse);
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
            Assert.Equal("Admin", user.Name);
            Assert.Equal("admin@test.com", user.Email);
            Assert.Equal("Admin", user.Role);
            Assert.True(user.RxPriceUsers.Count() >= 1);
        }

        [Fact]
        public async Task AddRxPrice()
        {
            var response = await _client.PostAsync($"api/Users/RxPrices/2100/1", null);
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("RxPrice 2100 Added To User 1!", stringResponse);
        }

        [Fact]
        public async Task RemoveRxPrice()
        {
            var response = await _client.DeleteAsync($"api/Users/RxPrices/2100/1");
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("RxPrice 2100 Removed From User 1!", stringResponse);
        }

        [Fact]
        public async Task PutUser()
        {
            var user = new User
            {
                Id = 1,
                Name = "Admin",
                Email = "admin@test.com",
                Role = "Admin"
            };
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("api/Users/1", data);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task PostUser()
        {
            var user = new User
            {
                Id = 500,
                Name = "Guest",
                Email = "guest@test.com",
                Role = "Guest"
            };
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Users", data);
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains("Guest", stringResponse);
        }

        [Fact]
        public async Task DeleteUser()
        {
            var response = await _client.DeleteAsync("api/Users/500");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}