using RxData.Models;
using RxData.Services;
using System.Collections.Generic;
using Xunit;

namespace RxDataTests.Unit
{
    public class PriceCalculatorTests
    {
        private readonly PriceCalculator _priceCalculator;

        public PriceCalculatorTests()
        {
            _priceCalculator = new PriceCalculator();
        }

        [Fact]
        public void PricePerMg()
        {
            var rxPrices = new List<RxPrice>
            {
                new RxPrice { Quantity = 20, Dose = 10, Price = 400 },
                new RxPrice { Quantity = 30, Dose = 5, Price = 150 }
            };

            var result = _priceCalculator.PricePerMg(rxPrices);

            Assert.Collection(result,
                item =>
                {
                    Assert.Equal(1, item.Price);
                    Assert.Equal(30, item.Quantity);
                },
                item =>
                {
                    Assert.Equal(2, item.Price);
                    Assert.Equal(20, item.Quantity);
                });
        }
    }
}
