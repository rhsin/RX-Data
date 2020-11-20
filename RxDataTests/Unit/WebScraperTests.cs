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
            Assert.All(rxPrices, rp => Assert.Equal("baclofen", rp.Name));
            Assert.All(rxPrices, rp => Assert.True(rp.Price > 12));
            Assert.All(rxPrices, rp => Assert.NotNull(rp.Location));
            Assert.All(rxPrices, rp => Assert.Equal(1, rp.VendorId));
        }

        [Fact]
        public void GetRxPricesCanada()
        {
            var rxPrices = _webScraper.GetRxPricesCanada("Baclofen").Result;

            Assert.IsType<List<RxPrice>>(rxPrices);
            Assert.Equal(11, rxPrices.Count());
            Assert.All(rxPrices, rp => Assert.Equal("baclofen", rp.Name));
            Assert.All(rxPrices, rp => Assert.True(rp.Dose >= 10));
            Assert.All(rxPrices, rp => Assert.True(rp.Price > 50));
            Assert.All(rxPrices, rp => Assert.Equal("online", rp.Location));
            Assert.All(rxPrices, rp => Assert.Equal(2, rp.VendorId));
        }
    }
}
