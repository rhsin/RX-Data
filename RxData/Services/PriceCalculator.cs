using RxData.Models;
using System.Collections.Generic;
using System.Linq;

namespace RxData.Services
{
    public interface IPriceCalculator
    {
        public IEnumerable<RxPrice> PricePerMg(IEnumerable<RxPrice> rxPrices);
    }

    public class PriceCalculator : IPriceCalculator
    {
        public IEnumerable<RxPrice> PricePerMg(IEnumerable<RxPrice> rxPrices)
        {
            foreach (var rp in rxPrices)
            {
                rp.Price = rp.Price / (rp.Quantity * rp.Dose);
            }

            return rxPrices.OrderBy(rp => rp.Price).ToList();
        }
    }
}
