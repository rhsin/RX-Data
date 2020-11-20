﻿using Microsoft.AspNetCore.Mvc.Testing;
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
        //[InlineData("api/RxPrices/1")]
        [InlineData("api/RxPrices/Fetch/Baclofen")]
        [InlineData("api/RxPrices/Fetch/Canada/Baclofen")]
        [InlineData("api/Vendors")]
        [InlineData("api/Vendors/1")]
        public async Task TestGetEndpoints(string url)
        {
            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("api/Vendors/Seeder")]
        public async Task TestSeederEndpoints(string url)
        {
            var response = await _client.PostAsync(url, null);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}