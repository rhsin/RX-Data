using RxData.Models;
using RxData.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RxDataTests.Unit
{
    public class WebScraperTests
    {
        private readonly WebScraper _webScraper;

        public WebScraperTests()
        {
            _webScraper = new WebScraper();
        }

        [Fact]
        public void GetRxPrices()
        {
            var rxPrices = _webScraper.GetRxPrices("Baclofen").Result;

            Assert.IsType<List<RxPrice>>(rxPrices);
            Assert.Equal(8, rxPrices.Count());
            Assert.All(rxPrices, rp => Assert.NotNull(rp.Name));
            Assert.All(rxPrices, rp => Assert.True(rp.Price > 12));
        }
    }
}
