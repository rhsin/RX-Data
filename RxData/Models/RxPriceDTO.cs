using System.Collections.Generic;

namespace RxData.Models
{
    public class RxPriceDTO
    {
        public string Method { get; set; }
        public int Count { get; set; }

        public IEnumerable<object> RxPrices { get; set; }
    }
}
