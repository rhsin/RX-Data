using RxData.Services;
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

        //[Fact]
        //public void GetRxPrices()
        //{
        //    var rxPrices = _webScraper.GetRxPrices("Baclofen").Result;

        //    Assert.All(rxPrices, rp => Assert.Equal("baclofen", rp.Name));
        //    Assert.All(rxPrices, rp => Assert.True(rp.Quantity >= 15));
        //    Assert.All(rxPrices, rp => Assert.True(rp.Dose >= 5));
        //    Assert.All(rxPrices, rp => Assert.True(rp.Price >= 12));
        //    Assert.All(rxPrices, rp => Assert.NotNull(rp.Location));
        //    Assert.All(rxPrices, rp => Assert.Equal(1, rp.VendorId));
        //}

        [Fact]
        public void GetRxPricesCanada()
        {
            var rxPrices = _webScraper.GetRxPricesCanada("Baclofen").Result;

            Assert.All(rxPrices, rp => Assert.Equal("baclofen", rp.Name));
            Assert.All(rxPrices, rp => Assert.True(rp.Quantity >= 30));
            Assert.All(rxPrices, rp => Assert.True(rp.Dose >= 10));
            Assert.All(rxPrices, rp => Assert.True(rp.Price >= 50));
            Assert.All(rxPrices, rp => Assert.Equal("online", rp.Location));
            Assert.All(rxPrices, rp => Assert.Equal(2, rp.VendorId));
        }

        [Fact]
        public void GetRxPricesCanadaAlt()
        {
            var rxPrices = _webScraper.GetRxPricesCanadaAlt("Prednisone").Result;

            Assert.All(rxPrices, rp => Assert.Equal("prednisone", rp.Name));
            Assert.All(rxPrices, rp => Assert.True(rp.Quantity >= 50));
            Assert.All(rxPrices, rp => Assert.True(rp.Dose >= 5));
            Assert.All(rxPrices, rp => Assert.True(rp.Price >= 25));
            Assert.All(rxPrices, rp => Assert.Equal("online", rp.Location));
            Assert.All(rxPrices, rp => Assert.Equal(1002, rp.VendorId));
        }

        [Theory]
        [InlineData("text10")]
        [InlineData("$10 USD")]
        [InlineData("10mg")]
        public void GetInteger(string input)
        {
            var result = _webScraper.GetInteger(input);

            Assert.Equal(10, result);
        }

        [Theory]
        [InlineData("text15.50")]
        [InlineData("$15.50 USD")]
        [InlineData("15.50mg")]
        public void GetFloat(string input)
        {
            var result = _webScraper.GetFloat(input);

            Assert.Equal(15.50, result);
        }
    }
}
