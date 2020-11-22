﻿using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RxData.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RxDataTests.Integration
{
    public class ErrorTests : IClassFixture<WebApplicationFactory<RxData.Startup>>
    {
        private readonly HttpClient _client;

        public ErrorTests(WebApplicationFactory<RxData.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ErrorResponse()
        {
            var response = await _client.DeleteAsync("api/RxPrices/100000");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<Error>(stringResponse);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal(500, error.StatusCode);
            Assert.Equal("Value cannot be null. (Parameter 'entity')", error.Message);
            Assert.Contains("RxPricesController.DeleteRxPrice", error.StackTrace);
        }

        [Fact]
        public async Task GetRxPriceError()
        {
            var response = await _client.GetAsync("api/RxPrices/100000");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task FindRxPricesError()
        {
            var response = await _client.GetAsync("api/RxPrices/Find/Ba?column=dddddddddd");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<Error>(stringResponse);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal("Invalid Column Argument!", error.Message);
        }

        [Fact]
        public async Task GetVendorError()
        {
            var response = await _client.GetAsync("api/Vendors/100000");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
