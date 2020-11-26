using System.Collections.Generic;

namespace RxData.DTO
{
    public class RxPriceDTO
    {
        public string Method { get; set; }
        public int Count { get; set; }

        public IEnumerable<dynamic> RxPrices { get; set; }
    }
}
