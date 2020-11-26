using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RxData.DTO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RxDataTests.Integration
{
    public class TokenTests : IClassFixture<WebApplicationFactory<RxData.Startup>>
    {
        private readonly HttpClient _client;

        public TokenTests(WebApplicationFactory<RxData.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateToken()
        {
            var login = new Login { Email = "admin@test.com", Password = "test" };
            var json = JsonConvert.SerializeObject(login);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Tokens", data);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<AuthToken>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, token.User.Id);
            Assert.Equal("admin@test.com", token.User.Email);
            Assert.Equal("Admin", token.User.Role);
            Assert.True(token.Token.Length >= 500);
        }

        [Fact]
        public async Task Unauthorized()
        {
            var login = new Login { Email = "test@test.com", Password = "test" };
            var json = JsonConvert.SerializeObject(login);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Tokens", data);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
